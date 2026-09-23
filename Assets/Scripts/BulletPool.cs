using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{


    [SerializeField] GameObject bulletPrefa;
    [SerializeField] int poolSize = 50;

    List<GameObject> pool = new List<GameObject>();

   
    private void Awake()
    {
        for (int i = 0; i < poolSize; i++) 
        {  
            GameObject bullet = Instantiate(bulletPrefa);
            bullet.SetActive(false);
            pool.Add(bullet);        
        }
    }

    public Bullet getBullet(Vector3 position)
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (pool[i].activeInHierarchy == false)
            {
                pool[i].transform.position = position;
                pool[i].SetActive(true);
                return pool[i].GetComponent<Bullet>();
            }
        }

        return null;

    }
}
