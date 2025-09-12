using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BulletObjectPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletPrefab2;
    Queue<GameObject> pool = new Queue<GameObject>();
    Queue<GameObject> pool2 = new Queue<GameObject>();

    public GameObject GetObject()
    {
        if(pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(bulletPrefab);
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    public GameObject GetObjectAutoShoot()
    {
        if(pool2.Count > 0)
        {
            GameObject obj = pool2.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(bulletPrefab2);
    }

    public void ReturnObjectAutoShoot(GameObject obj)
    {
        obj.SetActive(false);
        pool2.Enqueue(obj);
    }
}
