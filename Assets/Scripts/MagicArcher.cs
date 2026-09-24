using UnityEngine;

public class MagicArcher : HeroSwitcher
{
    [SerializeField] private BulletPool piercingPool;

    public override BulletPool ModifyBulletPool(BulletPool basePool)
    {
        if (piercingPool != null)
        {
            return piercingPool;
        }

        return basePool;
    }
}