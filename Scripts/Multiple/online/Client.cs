using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 客户端脚本
/// </summary>
public class Client : MonoBehaviour
{
    Queue<AP_FrameData> logic;          //逻辑帧的结果队列
    Queue<CS_FrameData> render;         //渲染帧

    UdpClient broadcast;                    //向局域网广播，谁是主机
    UdpClient reply;                        //广播后收到的回复
    UdpClient join;                         //连接请求，在reply之后，连接端口同询问端口
    UdpClient sender;                       //向主机发送的帧操作数据
    UdpClient receive;                      //接收主机发来的帧数据

    string localIp;                         //本机ip
    string host_ip;                         //想要连接的主机ip
    string broadcast_ip;                    //广播地址ip

    int receive_port;                       //广播局域网后，监听回复，默认7072
    int query_port;                         //用于询问的规定端口
    int send_port;                          //主机接收帧数据的端口号
    int frame_data_port;                    //接收帧数据

    int playerId;                           //此客户端控制的玩家号

    int frame_mark;                         //当前帧号

    List<string> host_list;                 //主机ip

    float t_start;                          //这是主机端开始的时间
    float t_clock;                          //逻辑帧间隔
    float t_clock_render;                   //渲染帧间隔
    float join_time;                        //此客户端加入的时间
    float t_rem_render;                     //记录上一次渲染帧时间
    float t_rem_logic;                      //记录上一次逻辑帧发送时间

    ControllerData cd;                      //用于向主机发送帧数据

    FrameHandler fh;                        //帧处理器

    CS_FrameData res_cf;                    //记录渲染帧队尾

    bool joined;                            //已经加入房间

    Controller input;                       //输入

    public static Dictionary<int, bool> player_active;    //玩家激活状态


    private void Awake()
    {
        Init();
        player_active.Add(1, true);
    }

    private void Start()
    {
        if (IsHost.ishost == true)
        {
            joined = true;
            host_ip = localIp;
            Server.player_active[1] = true;
            playerId = 1;
        }

        StartClient();

    }

    private void Update()
    {
        input.GetInput();

        if (joined)
        {
            SendCheck();

            FrameCheck();
        }


    }

    private void FixedUpdate()
    {
        input.GetInput();
    }









    public void Init()
    {
        logic = new Queue<AP_FrameData>();
        render = new Queue<CS_FrameData>();
        fh = new FrameHandler();
        player_active = new Dictionary<int, bool>();

        cd = new ControllerData();

        localIp = "127.0.0.2";
        broadcast_ip = "127.0.0.1";
        query_port = 7070;
        send_port = 7071;
        receive_port = 7073;
        frame_data_port = 7072;
        host_ip = null;

        t_clock = 1f / 30f;
        t_clock_render = 1f / 60f;

        host_list = new List<string>();
        input = MGM.instance.Controller.GetComponent<Controller>();
    }





    /// <summary>
    /// 点击搜索房间，向局域网广播，对方端口query_port
    /// </summary>
    /// <returns></returns>
    public void StartBroadcast()
    {
        Quety_data qd = new Quety_data();
        qd.port = receive_port;
        qd.ip = localIp;
        qd.type = 1;
        byte[] ds = DataTransform.Serialize(qd);
        //序列化询问数据类
        broadcast = new UdpClient();
        broadcast.EnableBroadcast = true;
        IPAddress ipa = IPAddress.Parse(broadcast_ip);
        broadcast.SendAsync(ds, ds.Length, new IPEndPoint(ipa, query_port));
        broadcast.SendAsync(ds, ds.Length, new IPEndPoint(IPAddress.Parse(localIp), query_port));
        //广播询问
    }
    /// <summary>
    /// 持续监听广播后的回复，返回两种可能的类型，本地端口是receive_port
    /// </summary>
    /// <returns></returns>
    public async Task ReceiveReply()
    {
        if (IsHost.ishost==false)
        {
            IPEndPoint le = new IPEndPoint(IPAddress.Parse(localIp), receive_port);
            reply = new UdpClient(le);
            while (true)
            {
                UdpReceiveResult rs = await reply.ReceiveAsync();
                //等待接收
                byte[] buf = rs.Buffer;
                Query_res_data qrd = DataTransform.Deserialize<Query_res_data>(buf);


                if (qrd.type == 1 && qrd.isHost == true)
                {

                    host_list.Add(qrd.ip);
                    Room.ui.GetComponent<Room>().AddToUi(qrd.ip);
                    //这里要通知UI添加房间列表
                }
                else if (qrd.type == 2)
                {
                    Query_res_data2 qrd2 = DataTransform.Deserialize<Query_res_data2>(buf);
                    StartFrameSync(qrd2);
                }
                //Query_res_data2继承自Query_res_data，通过类型转换判定类型
            }
        }
    }
    /// <summary>
    /// 获得房间ip后，在UI上点击加入房间，向该房间和询问端口号发送连接请求，对方端口是query_port
    /// </summary>
    public void JoinRoom()
    {
        IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(host_ip), query_port);
        if (join == null) join = new UdpClient();

        Quety_data qd = new Quety_data();
        qd.port = receive_port;
        qd.ip = localIp;
        qd.type = 2;

        byte[] ds = DataTransform.Serialize(qd);

        join.Send(ds, ds.Length, ipe);

        t_start = Time.time;

        joined = true;
    }
    /// <summary>
    /// 向主机发送操作数据，此时sender应该已经初始化了
    ///     发送ControllerData，对方端口是 send_port
    /// </summary>
    public void SendControllerData()
    {
        IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(host_ip), send_port);

        var cmp = MGM.instance.Controller.GetComponent<Controller>();
        cd = cmp.cd;
        cd.Id = playerId;

        if (sender == null) sender = new UdpClient();
        byte[] ds = DataTransform.Serialize(cd);

        sender.SendAsync(ds, ds.Length, ipe);
    }
    /// <summary>
    /// 接收主机转发过来的AP_FrameData，本地端口是 frame_data_port
    /// </summary>
    /// <returns></returns>
    public async Task StartReceive()
    {
        IPEndPoint le = new IPEndPoint(IPAddress.Parse(localIp), frame_data_port);
        if (receive == null) receive = new UdpClient(le);
        while (true)
        {
            UdpReceiveResult rs = await receive.ReceiveAsync();
            //有消息再执行


            if (joined == false) continue;



            AP_FrameData apfd = null;
            apfd = DataTransform.Deserialize<AP_FrameData>(rs.Buffer);
            //反序列化
            string h1 = apfd.host_ip.Normalize();
            string h2 = host_ip.Normalize();


            if (h1 == h2)
            {
                joined = true;
                Handle_Framne(apfd);
            }
        }
    }






    /// <summary>
    /// 有新逻辑帧加入，以队尾帧，计算出新的结果帧，放入渲染帧
    /// </summary>
    public void Handle_Framne(AP_FrameData apfd)
    {
        for (int i = 1; i <= 4; i++)
        {
            var p = apfd.Get_resdata(i);
            if (p != null)
            {
                if (p.active == true)
                {
                    UpdatePlayerState(i);
                }
            }
        }


        if (render.Count == 0)
        {
            CS_FrameData cf = fh.GetNowFrame();
            CS_FrameData now = fh.CalcNextFrame(cf, apfd);


            res_cf = now;
            now.frame_mark = apfd.frame_mark;
            //若队列中无结果帧，使用当前帧为基础计算
            render.Enqueue(now);
        }
        else
        {
            CS_FrameData now = fh.CalcNextFrame(res_cf, apfd);
            CS_FrameData inter = fh.Interpolate(res_cf, now);
            res_cf = now;
            render.Enqueue(inter);
            render.Enqueue(now);
            //插值后加入
        }

    }


    /// <summary>
    /// 表示已经收到主机发来的帧数据,需要：
    ///         同步帧数据
    ///         初始化sender
    /// 
    /// </summary>
    public void StartFrameSync(Query_res_data2 qrd2)
    {
        this.join_time = qrd2.join_time;
        this.playerId = qrd2.playerId;
        t_rem_render = Time.time;

        var cmp = MGM.instance.Controller.GetComponent<Controller>();
        cmp.Change_Camera_Lookat(playerId);

        for (int i = 1; i <= 4; i++)
        {
            if (qrd2.FC.Get_p(i))
            {
                StateData sd = qrd2.FC.Get_state(i);
                UseStateData(sd);
            }
        }

        sender = new UdpClient();
    }


    /// <summary>
    /// 应用 UseCS_FrameData
    /// </summary>
    /// <param name="sd"></param>
    public void UseCS_FrameData(CS_FrameData sd)
    {

        for(int i = 1; i <= 4; i++)
        {
            if (sd.dic.ContainsKey(i))
            {
                if (sd.dic[i].active == true)
                {
                    //UpdatePlayerState(i);
                    UseStateData(sd.dic[i]);
                }
            }
        }
        var cmp = MGM.instance.Controller.GetComponent<Controller>();
        cmp.Change_Camera_Lookat(playerId);
    }


    /// <summary>
    /// 应用 UseStateData到实际游戏对象
    /// </summary>
    /// <param name="sd"></param>
    public void UseStateData(StateData sd)
    {
        var player = SM.Get_player(sd.Id).GetComponent<player>();
        player.SetStateData(sd);
    }


    /// <summary>
    /// 检测是否帧号是否慢于实际帧号（渲染帧）
    /// </summary>
    /// <returns></returns>
    public bool CheckFrameMark(int fm)
    {
        return fm < (int)((join_time + Time.time - t_start) / t_clock_render);
    }


    /// <summary>
    /// 开始客户端服务
    /// </summary>
    public void StartClient()
    {
        List<Task> client=new List<Task>();
        client.Add(ReceiveReply());
        client.Add(StartReceive());

        Task.WhenAll(client);
    }


    /// <summary>
    /// 执行渲染帧
    /// </summary>
    public void Next_Render_Frame()
    {
        if (render.Count > 0)
        {
            while (CheckFrameMark(render.Peek().frame_mark))
            {
                UseCS_FrameData(render.Peek());
                render.Dequeue();
                if (render.Count == 0) break;
            }
        }
    }


    /// <summary>
    /// 在Update中以固定频率执行渲染帧
    /// </summary>
    public void FrameCheck()
    {
        float now = Time.time;
        if (now - t_rem_render >= t_clock_render)
        {
            Render(now,t_rem_render);
            t_rem_render = now;
        }
    }


    /// <summary>
    /// 向主机发送数据
    /// </summary>
    public void SendCheck()
    {
        float now=Time.time;
        if (now - t_rem_logic >= t_clock)
        {
            SendControllerData();
            t_rem_logic = now;
            input.CleanUpInput();
        }
    }


    /// <summary>
    /// 渲染帧
    /// </summary>
    public void Render(float now,float rem)
    {
        Next_Render_Frame();

        EventHandle.ElseLogic(now, rem);

    }


    /// <summary>
    /// 设置要链接的ip
    /// </summary>
    /// <param name="ip"></param>
    public void Set_hostip(string ip)
    {
        host_ip = ip;
    }


    /// <summary>
    /// 增加玩家
    /// </summary>
    /// <param name="index"></param>
    public void UpdatePlayerState(int index)
    {
        if (!player_active.ContainsKey(index))
        {
            player_active.Add(index, false);
        }
        if (player_active[index] == false)
        {
            var cmp = MGM.instance.statemanager.GetComponent<SM>();
            cmp.player_active[index] = true;
            cmp.AddPlayer();
            player_active[index] = true;
        }
    }
}
