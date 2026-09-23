using UnityEngine;

public class HeroSwitcher : MonoBehaviour
{
    public virtual BulletPool ModifyBulletPool(BulletPool basePool)
    {
        return basePool;
    }
    public virtual int ModifyDamage(int baseDamage)
    {
        return baseDamage;
    }

    public virtual float ModifyFireRate(float baseFireRate)
    {
        return baseFireRate;
    }

}
