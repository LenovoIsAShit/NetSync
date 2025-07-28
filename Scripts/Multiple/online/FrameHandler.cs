using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ֡���ݴ�����
/// </summary>
public class FrameHandler 
{
    float t_clock = 1f / 30f;   //�߼�֡���



    public CS_FrameData Interpolate(CS_FrameData cfl,CS_FrameData cfr)
    {
        CS_FrameData cfres = new CS_FrameData();
        cfres.frame_mark = cfr.frame_mark - 1;
        for(int i = 1; i <= 4; i++)
        {
            if (cfl.dic.ContainsKey(i) && cfr.dic.ContainsKey(i)){
                cfres.dic[i] = new StateData();
                
                cfres.dic[i].Id = i;
                //���
                cfres.dic[i].Pos.Assign( new Vector3(
                    Mathf.Lerp(cfl.dic[i].Pos.x, cfr.dic[i].Pos.x, 0.5f),
                    Mathf.Lerp(cfl.dic[i].Pos.y, cfr.dic[i].Pos.y, 0.5f),
                    Mathf.Lerp(cfl.dic[i].Pos.z, cfr.dic[i].Pos.z, 0.5f)
                    )
                );
                //λ��
                cfres.dic[i].MousePos.Assign(new Vector3(
                    Mathf.Lerp(cfl.dic[i].MousePos.x, cfr.dic[i].MousePos.x, 0.5f),
                    Mathf.Lerp(cfl.dic[i].MousePos.y, cfr.dic[i].MousePos.y, 0.5f),
                    Mathf.Lerp(cfl.dic[i].MousePos.z, cfr.dic[i].MousePos.z, 0.5f)
                    )
                );
                //���λ��
                if (cfl.dic[i].Mouse || cfr.dic[i].Mouse) cfres.dic[i].Mouse = true;
                if (cfl.dic[i].Space || cfr.dic[i].Space) cfres.dic[i].Space = true;
                //����
                cfres.dic[i].hp = (int)Mathf.Lerp(cfl.dic[i].hp, cfr.dic[i].hp, 0.5f);
                //Ѫ��
                cfres.dic[i].wp=cfr.dic[i].wp;
                //����
            }
            else
            {
                if (cfl.dic.ContainsKey(i))cfres.dic[i] = cfl.dic[i];
                if (cfr.dic.ContainsKey(i))cfres.dic[i] = cfr.dic[i];
            }
        }

        return cfres;
    }

    public void UseFrame(CS_FrameData cf)
    {
        for(int i = 1; i <= 4; i++)
        {
            if (cf.dic[i] == null) continue;
            GameObject p = GetPlayer(i);

            p.transform.position = cf.dic[i].Pos.AssignToVector3();
            //λ��
            var cmp = p.GetComponent<player>();
            cmp.Change_dir(cf.dic[i].MousePos.AssignToVector3());
            //����
            cmp.Set_HP(cf.dic[i].hp);
            //Ѫ��
            cmp.Change_weapon(cf.dic[i].wp);
            //����
            cmp.die=cf.dic[i].die;
            //�Ƿ�����
            if (cf.dic[i].Mouse)cmp.Shoot();
            //�Ƿ����
        }
    }

    GameObject GetPlayer(int i)
    {
        switch (i)
        {
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


    public CS_FrameData GetNowFrame()
    {
        CS_FrameData cf = new CS_FrameData();
        for (int i = 1; i <= SM.player_num; i++)
        {
            cf.Set(i, SM.Get_player_sd(i));
        }
        return cf;
    }


    public CS_FrameData CalcNextFrame(CS_FrameData cf,AP_FrameData ap)
    {
        CS_FrameData next = new CS_FrameData();
        next.frame_mark = ap.frame_mark;
        for(int i = 1; i <= 4; i++)
        {
            if (cf.Get(i) == null) continue;
            StateData sd = cf.Get(i);
            ResData rd = ap.Get_resdata(i);

            StateData res = Calc(sd, rd);
            next.Set(i, res);


            
        }
        return next;
    }

    public StateData Calc(StateData sd,ResData rd)
    {
        StateData res = new StateData();

        float t = t_clock;
        //t��һ���߼�֡�ļ��

        res.Id = sd.Id;
        res.Pos = sd.Pos;
        res.MousePos = sd.MousePos;
        res.Mouse=sd.Mouse;
        res.Space = rd.isInSky;
        res.hp = sd.hp;
        res.wp = sd.wp;
        res.die = sd.die;
        res.yPos = rd.yPos;
        if (Client.player_active.ContainsKey(sd.Id)) res.active = Client.player_active[sd.Id];
        else res.active = false;
        //��ֵ

        Vector3 p = res.Pos.AssignToVector3();
        if (rd.D) p += (t * 5f * SM.Get_player(res.Id).transform.right);
        if (rd.A) p -= (t * 5f * SM.Get_player(res.Id).transform.right);
        if (rd.isInSky) p.Set(p.x, rd.yPos, p.z);
        res.Pos.Assign(p);
        //λ��

        res.MousePos = rd.Mpos.Interp(res.MousePos);
        //���λ��

        res.Mouse = rd.mouse;
        //��갴��

        return res;
    }
}
