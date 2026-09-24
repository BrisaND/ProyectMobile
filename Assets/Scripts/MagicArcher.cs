using UnityEngine;

public class MagicArcher : HeroSwitcher
{
    public override BulletPool ModifyBulletPool(BulletPool basePool)
    {
        if (BulletPool.Instance != null)
        {
            return BulletPool.Instance;
        }

        return basePool;
    }
}
