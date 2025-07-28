using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// �ͻ��˽ű�
/// </summary>
public class Client : MonoBehaviour
{
    Queue<AP_FrameData> logic;          //�߼�֡�Ľ������
    Queue<CS_FrameData> render;         //��Ⱦ֡

    UdpClient broadcast;                    //��������㲥��˭������
    UdpClient reply;                        //�㲥���յ��Ļظ�
    UdpClient join;                         //����������reply֮�����Ӷ˿�ͬѯ�ʶ˿�
    UdpClient sender;                       //���������͵�֡��������
    UdpClient receive;                      //��������������֡����

    string localIp;                         //����ip
    string host_ip;                         //��Ҫ���ӵ�����ip
    string broadcast_ip;                    //�㲥��ַip

    int receive_port;                       //�㲥�������󣬼����ظ���Ĭ��7072
    int query_port;                         //����ѯ�ʵĹ涨�˿�
    int send_port;                          //��������֡���ݵĶ˿ں�
    int frame_data_port;                    //����֡����

    int playerId;                           //�˿ͻ��˿��Ƶ���Һ�

    int frame_mark;                         //��ǰ֡��

    List<string> host_list;                 //����ip

    float t_start;                          //���������˿�ʼ��ʱ��
    float t_clock;                          //�߼�֡���
    float t_clock_render;                   //��Ⱦ֡���
    float join_time;                        //�˿ͻ��˼����ʱ��
    float t_rem_render;                     //��¼��һ����Ⱦ֡ʱ��
    float t_rem_logic;                      //��¼��һ���߼�֡����ʱ��

    ControllerData cd;                      //��������������֡����

    FrameHandler fh;                        //֡������

    CS_FrameData res_cf;                    //��¼��Ⱦ֡��β

    bool joined;                            //�Ѿ����뷿��

    Controller input;                       //����

    public static Dictionary<int, bool> player_active;    //��Ҽ���״̬


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
    /// ����������䣬��������㲥���Է��˿�query_port
    /// </summary>
    /// <returns></returns>
    public void StartBroadcast()
    {
        Quety_data qd = new Quety_data();
        qd.port = receive_port;
        qd.ip = localIp;
        qd.type = 1;
        byte[] ds = DataTransform.Serialize(qd);
        //���л�ѯ��������
        broadcast = new UdpClient();
        broadcast.EnableBroadcast = true;
        IPAddress ipa = IPAddress.Parse(broadcast_ip);
        broadcast.SendAsync(ds, ds.Length, new IPEndPoint(ipa, query_port));
        broadcast.SendAsync(ds, ds.Length, new IPEndPoint(IPAddress.Parse(localIp), query_port));
        //�㲥ѯ��
    }
    /// <summary>
    /// ���������㲥��Ļظ����������ֿ��ܵ����ͣ����ض˿���receive_port
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
                //�ȴ�����
                byte[] buf = rs.Buffer;
                Query_res_data qrd = DataTransform.Deserialize<Query_res_data>(buf);


                if (qrd.type == 1 && qrd.isHost == true)
                {

                    host_list.Add(qrd.ip);
                    Room.ui.GetComponent<Room>().AddToUi(qrd.ip);
                    //����Ҫ֪ͨUI��ӷ����б�
                }
                else if (qrd.type == 2)
                {
                    Query_res_data2 qrd2 = DataTransform.Deserialize<Query_res_data2>(buf);
                    StartFrameSync(qrd2);
                }
                //Query_res_data2�̳���Query_res_data��ͨ������ת���ж�����
            }
        }
    }
    /// <summary>
    /// ��÷���ip����UI�ϵ�����뷿�䣬��÷����ѯ�ʶ˿ںŷ����������󣬶Է��˿���query_port
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
    /// ���������Ͳ������ݣ���ʱsenderӦ���Ѿ���ʼ����
    ///     ����ControllerData���Է��˿��� send_port
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
    /// ��������ת��������AP_FrameData�����ض˿��� frame_data_port
    /// </summary>
    /// <returns></returns>
    public async Task StartReceive()
    {
        IPEndPoint le = new IPEndPoint(IPAddress.Parse(localIp), frame_data_port);
        if (receive == null) receive = new UdpClient(le);
        while (true)
        {
            UdpReceiveResult rs = await receive.ReceiveAsync();
            //����Ϣ��ִ��


            if (joined == false) continue;



            AP_FrameData apfd = null;
            apfd = DataTransform.Deserialize<AP_FrameData>(rs.Buffer);
            //�����л�
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
    /// �����߼�֡���룬�Զ�β֡��������µĽ��֡��������Ⱦ֡
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
            //���������޽��֡��ʹ�õ�ǰ֡Ϊ��������
            render.Enqueue(now);
        }
        else
        {
            CS_FrameData now = fh.CalcNextFrame(res_cf, apfd);
            CS_FrameData inter = fh.Interpolate(res_cf, now);
            res_cf = now;
            render.Enqueue(inter);
            render.Enqueue(now);
            //��ֵ�����
        }

    }


    /// <summary>
    /// ��ʾ�Ѿ��յ�����������֡����,��Ҫ��
    ///         ͬ��֡����
    ///         ��ʼ��sender
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
    /// Ӧ�� UseCS_FrameData
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
    /// Ӧ�� UseStateData��ʵ����Ϸ����
    /// </summary>
    /// <param name="sd"></param>
    public void UseStateData(StateData sd)
    {
        var player = SM.Get_player(sd.Id).GetComponent<player>();
        player.SetStateData(sd);
    }


    /// <summary>
    /// ����Ƿ�֡���Ƿ�����ʵ��֡�ţ���Ⱦ֡��
    /// </summary>
    /// <returns></returns>
    public bool CheckFrameMark(int fm)
    {
        return fm < (int)((join_time + Time.time - t_start) / t_clock_render);
    }


    /// <summary>
    /// ��ʼ�ͻ��˷���
    /// </summary>
    public void StartClient()
    {
        List<Task> client=new List<Task>();
        client.Add(ReceiveReply());
        client.Add(StartReceive());

        Task.WhenAll(client);
    }


    /// <summary>
    /// ִ����Ⱦ֡
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
    /// ��Update���Թ̶�Ƶ��ִ����Ⱦ֡
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
    /// ��������������
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
    /// ��Ⱦ֡
    /// </summary>
    public void Render(float now,float rem)
    {
        Next_Render_Frame();

        EventHandle.ElseLogic(now, rem);

    }


    /// <summary>
    /// ����Ҫ���ӵ�ip
    /// </summary>
    /// <param name="ip"></param>
    public void Set_hostip(string ip)
    {
        host_ip = ip;
    }


    /// <summary>
    /// �������
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
