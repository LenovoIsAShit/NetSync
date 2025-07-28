using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// ��������ű�
/// </summary>
public class Server : MonoBehaviour
{
    UdpClient query;        //���ھ�����ѯ�ʣ��˿�7070
    UdpClient serverD;      //�����������ָ�����ݣ��˿�7071
    UdpClient sender;       //�㲥��Ϣ��ת�����������֡���ݣ��Է��˿�7072
    IPAddress localIpa;     //��������IP��Ĭ��127.0.0.1
    string host_ip;         //ip
    string broadcast_ip;    //�㲥ip
    int query_port;         //��¼ѯ�ʶ˿�
    int serverD_port;       //��¼�������ݶ˿�
    int broadcast_port;     //�㲥�˿�

    public FrameHandler fh; //֡������

    float t_start;          //��Ϸ��ʼ��ʱ��
    float t_rem;            //��һ�ι㲥��ʱ��
    float logic_time;       //֡���
    int frame_mark_rem;          //֡��

    [HideInInspector]
    public AP_FrameData rem_frame;  //��¼��һ֡�����
    Dictionary<int, ControllerData> dic;    //��¼��ҷ���������
    public static Dictionary<int, bool> player_active;    //��Ҽ���״̬
    int check;              //������1111��ʾ�ĸ���Ҷ����������ݣ�0��ʾ��û������Ҫ���


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
    /// Update�м�ʱ�����̶�ʱ��㲥��Ϣ
    /// </summary>
    private void Update()
    {
        Broadcast_Check();
        //Debug.Log(frame_mark_rem);
    }















    /// <summary>
    /// ��ʼ��
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
    /// ��������ѯ�ʣ��˿���7070
    ///     ѯ�ʶ˵�ip��ַ+�˿�
    ///     ���ز�ѯ���
    /// </summary>
    public async Task StartReply()
    {
        IPEndPoint le = new IPEndPoint(localIpa, query_port);
        query = new UdpClient(le);
        while (true)
        {
            UdpReceiveResult rs = await query.ReceiveAsync();
            //����Ϣ��ִ��
            Quety_data qd = DataTransform.Deserialize<Quety_data>(rs.Buffer);
            //�����л���ѯ����
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
            //���л���ѯ���


            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(qd.ip), qd.port);
            //IPEndPoint ipe2 = new IPEndPoint(IPAddress.Parse(), qd.port);
            if (sender == null) sender = new UdpClient();
            await sender.SendAsync(res, res.Length, ipe);
            //await sender.SendAsync(res, res.Length, ipe);
            //����������Ͳ�ѯ���

        }
    }


    /// <summary>
    /// ��ʼ���������˿ڷ�����ControllerData���յ����ݲ�����
    /// </summary>
    /// <returns></returns>
    public async Task StartReceive()
    {
        IPEndPoint le=new IPEndPoint(localIpa, serverD_port);
        serverD = new UdpClient(le);
        while (true)
        {
            UdpReceiveResult rs = await serverD.ReceiveAsync();
            //����Ϣ��ִ��
            ControllerData cd = DataTransform.Deserialize<ControllerData>(rs.Buffer);
            //�����л�

            dic[cd.Id] = cd;
            Received(cd.Id);
            //ȷ���Ѿ��յ�ĳ���ͻ��˵���Ϣ

            check = check | (1 << (cd.Id - 1));

            //UpdatePlayerState(cd.Id);
            //�������
        }
    }


    /// <summary>
    /// ����ִ��
    /// </summary>
    public void StartServer()
    {
        List<Task> tasks = new List<Task>();
        tasks.Add(StartReply());
        tasks.Add(StartReceive());
        //����

        Task.WhenAll(tasks);
    }

    
    public void Broadcast_Check()
    {
        float now = Time.time;
        if (now - t_rem >= logic_time)
        {
            Broadcast();
            //�㲥��Ϣ
            t_rem = now;
        }
    }


    /// <summary>
    /// ��������㲥
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
    /// ����ҵ�controller����ȫ��ת��ΪAP_FrameData
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
                rd = rem_frame.Get_resdata(i + 1);//û�յ��Ļ���Ҫ����һ֡��Ԥ��
            }


            //û�յ��Ļ���Ҫ����һ֡��Ԥ��
            rem_frame.Set(i + 1, rd);
            //��¼
            res.Set(i + 1, rd);
            //��ֵ

        }

        return res;
    }


    /// <summary>
    /// �����Ұ���Ծ��������������server�������Ծ������
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
    /// һϵ�д���ͻ����Ƿ�����Ϣ����֤
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
    /// ɵ�����
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
