using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Vector3_m
{
    public float x;
    public float y;
    public float z;

    public void  Assign(Vector3 vec)
    {
        x= vec.x;
        y= vec.y;
        z= vec.z;
    }

    public Vector3 AssignToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector3_m Interp(Vector3_m vm)
    {
        Vector3_m res = new Vector3_m();
        res.x = Mathf.Lerp(x, vm.x, 0.5f);
        res.y = Mathf.Lerp(y, vm.y, 0.5f);
        res.z = Mathf.Lerp(z, vm.z, 0.5f);
        return res;
    }
}
