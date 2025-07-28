using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ServerFrameCache
{
    public bool p1;
    public bool p2;
    public bool p3;
    public bool p4;

    public StateData p1_state;
    public StateData p2_state;
    public StateData p3_state;
    public StateData p4_state;

    public ServerFrameCache()
    {
        p1_state = new StateData();
        p2_state = new StateData();
        p3_state = new StateData();
        p4_state = new StateData();
    }

    public void Setplayer(int i,bool isActive,StateData sd)
    {
        switch (i)
        {
            case 1:
                p1 = isActive;
                p1_state = sd;
                break;
            case 2:
                p2= isActive;
                p2_state = sd;
                break;
            case 3:
                p3 = isActive;
                p3_state = sd;
                break;
            case 4:
                p4 = isActive;
                p4_state = sd;
                break;
        }
    }

    public StateData Get_state(int i)
    {
        switch (i)
        {
            case 1:
                return p1_state;
            case 2:
                return p2_state;
            case 3:
                return p3_state;
            case 4:
                return p4_state;
        }
        return null;
    }

    public bool Get_p(int i)
    {
        switch(i){
            case 1:
                return p1;
            case 2:
                return p2;
            case 3:
                return p3;
            case 4:
                return p4;
        }
        return false;
    }
}
