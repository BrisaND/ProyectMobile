using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
   

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int poolSize = 20;

    private List<Bullet> pool = new List<Bullet>();

    protected virtual void Awake()
    {
      

        InicializarPool();
    }

    protected void InicializarPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false);
            pool.Add(bullet);
        }
    }

    public virtual Bullet getBullet(Vector3 position)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                pool[i].transform.position = position;
                pool[i].gameObject.SetActive(true);
                return pool[i];
            }
        }

        return null;
    }
}