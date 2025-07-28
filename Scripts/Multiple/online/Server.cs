using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// 主机房间脚本
/// </summary>
public class Server : MonoBehaviour
{
    UdpClient query;        //用于局域网询问，端口7070
    UdpClient serverD;      //接收所有玩家指令数据，端口7071
    UdpClient sender;       //广播消息，转发给所有玩家帧数据，对方端口7072
    IPAddress localIpa;     //主机本机IP，默认127.0.0.1
    string host_ip;         //ip
    string broadcast_ip;    //广播ip
    int query_port;         //记录询问端口
    int serverD_port;       //记录接收数据端口
    int broadcast_port;     //广播端口

    public FrameHandler fh; //帧处理器

    float t_start;          //游戏开始的时间
    float t_rem;            //上一次广播的时间
    float logic_time;       //帧间隔
    int frame_mark_rem;          //帧号

    [HideInInspector]
    public AP_FrameData rem_frame;  //记录上一帧的情况
    Dictionary<int, ControllerData> dic;    //记录玩家发来的数据
    public static Dictionary<int, bool> player_active;    //玩家激活状态
    int check;              //二进制1111表示四个玩家都发来了数据，0表示都没发，需要清空


    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        StartServer();

        if (!player_active.ContainsKey(1)) player_active.Add(1, true);
    }

    /// <summary>
    /// Update中计时，到固定时间广播消息
    /// </summary>
    private void Update()
    {
        Broadcast_Check();
        //Debug.Log(frame_mark_rem);
    }















    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        host_ip = "127.0.0.2";
        broadcast_ip = "127.0.0.1";
        localIpa = IPAddress.Parse(host_ip);
        query_port = 7070;
        serverD_port = 7071;
        broadcast_port = 7072;
        t_start=Time.time;
        logic_time = 1f / 30f;
        frame_mark_rem = 1;
        rem_frame = new AP_FrameData();

        dic= new Dictionary<int, ControllerData>();
        player_active = new Dictionary<int, bool>();
    }

    /// <summary>
    /// 持续监听询问，端口在7070
    ///     询问端的ip地址+端口
    ///     返回查询结果
    /// </summary>
    public async Task StartReply()
    {
        IPEndPoint le = new IPEndPoint(localIpa, query_port);
        query = new UdpClient(le);
        while (true)
        {
            UdpReceiveResult rs = await query.ReceiveAsync();
            //有消息再执行
            Quety_data qd = DataTransform.Deserialize<Quety_data>(rs.Buffer);
            //反序列化查询数据
            byte[] res = null;


            if (qd.type == 1)
            {

                Query_res_data qrd = new Query_res_data();
                qrd.ip = host_ip;
                qrd.isHost = IsHost.ishost;
                qrd.type = 1;
                res = DataTransform.Serialize(qrd);

            }
            else
            {
                Query_res_data2 ard = null;

                ard = MGM.instance.statemanager.GetComponent<SM>().GetAllState();
                ard.ip = host_ip;
                ard.type = 2;
                ard.isHost = IsHost.ishost;
                player_active.Add(ard.playerId, true);

                res = DataTransform.Serialize(ard);


            }
            //序列化查询结果


            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(qd.ip), qd.port);
            //IPEndPoint ipe2 = new IPEndPoint(IPAddress.Parse(), qd.port);
            if (sender == null) sender = new UdpClient();
            await sender.SendAsync(res, res.Length, ipe);
            //await sender.SendAsync(res, res.Length, ipe);
            //向该主机发送查询结果

        }
    }


    /// <summary>
    /// 开始持续监听端口发来的ControllerData，收到数据并处理
    /// </summary>
    /// <returns></returns>
    public async Task StartReceive()
    {
        IPEndPoint le=new IPEndPoint(localIpa, serverD_port);
        serverD = new UdpClient(le);
        while (true)
        {
            UdpReceiveResult rs = await serverD.ReceiveAsync();
            //有消息再执行
            ControllerData cd = DataTransform.Deserialize<ControllerData>(rs.Buffer);
            //反序列化

            dic[cd.Id] = cd;
            Received(cd.Id);
            //确定已经收到某个客户端的消息

            check = check | (1 << (cd.Id - 1));

            //UpdatePlayerState(cd.Id);
            //激活玩家
        }
    }


    /// <summary>
    /// 并行执行
    /// </summary>
    public void StartServer()
    {
        List<Task> tasks = new List<Task>();
        tasks.Add(StartReply());
        tasks.Add(StartReceive());
        //任务集

        Task.WhenAll(tasks);
    }

    
    public void Broadcast_Check()
    {
        float now = Time.time;
        if (now - t_rem >= logic_time)
        {
            Broadcast();
            //广播消息
            t_rem = now;
        }
    }


    /// <summary>
    /// 向局域网广播
    /// </summary>
    public void Broadcast()
    {
        AP_FrameData apfd = CDtoAPFD();

        if(sender==null)sender = new UdpClient();
        sender.EnableBroadcast = true;

        byte[] res = DataTransform.Serialize(apfd);
        sender.SendAsync(res, res.Length, new IPEndPoint(IPAddress.Parse(broadcast_ip), 7074));
        sender.SendAsync(res, res.Length, new IPEndPoint(localIpa, broadcast_port));
        frame_mark_rem++;
        check = 0;
    }


    /// <summary>
    /// 把玩家的controller数据全部转换为AP_FrameData
    /// </summary>
    public AP_FrameData CDtoAPFD()
    {
        AP_FrameData res = new AP_FrameData();
        res.frame_mark = frame_mark_rem;
        res.host_ip = host_ip;

        for (int i = 0; i < 4; i++)
        {
            if (!player_active.ContainsKey(i + 1)) continue;

            ResData rd = null;
            ControllerData cd;

            if (((1 << i) & check) > 0)
            {
                cd = dic[i + 1];
                rd = new ResData();

                JumpCheck(i + 1);
                rd.yPos = Get_Player(i + 1).transform.position.y;
                rd.isInSky = Get_Player(i + 1).GetComponent<player>().inAir;
                rd.A = cd.A;
                rd.D = cd.D;
                rd.mouse = cd.Mouse;
                rd.Mpos = cd.MousePos;
                if (player_active.ContainsKey(i + 1)) rd.active = player_active[i + 1];
                else rd.active = false;
            }
            else
            {
                rd = rem_frame.Get_resdata(i + 1);//没收到的话就要用上一帧来预测
            }


            //没收到的话就要用上一帧来预测
            rem_frame.Set(i + 1, rd);
            //记录
            res.Set(i + 1, rd);
            //赋值

        }

        return res;
    }


    /// <summary>
    /// 由于我把跳跃交给了主机，在server中完成跳跃的内容
    /// </summary>
    public void JumpCheck(int index)
    {
        var cmp = Get_Player(index).GetComponent<player>();
        if (dic[index].Space)
        {
            cmp.Jump();
        }
    }


    /// <summary>
    /// 一系列处理客户端是否发来消息的验证
    /// </summary>
    /// <param name="index"></param>
    public void Received(int index)
    {
        switch (index)
        {
            case 1:
                check = check | (1 << 0);
                break;
            case 2:
                check = check | (1 << 1);
                break;
            case 3:
                check = check | (1 << 2);
                break;
            case 4:
                check = check | (1 << 3);
                break;
        }
    }
    public bool CheckAllPlayerReceived()
    {
        if (check == (1 << 4) - 1) return true;
        else return false;
    }
    public void ResetCheck()
    {
        check = 0;
    }


    /// <summary>
    /// 傻逼设计
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject Get_Player(int index)
    {
        switch (index) {
            case 1:
                return MGM.instance.p1;
            case 2:
                return MGM.instance.p2;
            case 3:
                return MGM.instance.p3;
            case 4:
                return MGM.instance.p4;
        }
        return null;
    }
}
