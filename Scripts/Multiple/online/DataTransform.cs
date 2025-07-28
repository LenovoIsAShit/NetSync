using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// �ṩ���л��ͷ����л�������������
/// </summary>
public class DataTransform 
{
    public static T Deserialize<T>(byte[] by)
    {
        if (by == null) return default(T);

        BinaryFormatter f = new BinaryFormatter();
        MemoryStream m = new MemoryStream(by);
        return (T)f.Deserialize(m);
    }


    public static byte[] Serialize(object obj)
    {
        if(obj==null) return null;

        BinaryFormatter f = new BinaryFormatter();
        MemoryStream m = new MemoryStream();
        f.Serialize(m, obj);
        return m.ToArray();
    }
}
