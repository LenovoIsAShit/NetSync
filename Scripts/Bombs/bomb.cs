using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ×Óµ¯½Å±¾
/// </summary>
public class bomb : MonoBehaviour
{
    public int damage;
    public float speed;
    public Vector3 dir;

    public int index;
    public static int num;

    private void Awake()
    {
        num++;
        index = num;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Monster"))
        {
            var cmp = collision.gameObject.GetComponent<Monster>();
            cmp.Change_hp(damage);
        }
        Destroy(this.gameObject);
    }

    public void Set(int damage,float speed,Vector3 dir)
    {
        this.damage = damage;
        this.speed = speed;
        this.dir = dir;

        transform.right = dir;
    }

    private void Update()
    {
        if (!EventHandle.bombs.ContainsKey(index))
        {
            EventHandle.bombs.Add(index, this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        Move(Time.deltaTime);
    }

    public void Move(float dt)
    {
        transform.position += (dt * speed * dir);
    }
}
