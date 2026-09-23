using UnityEngine;

public class MagicArcher : HeroSwitcher
{
    [SerializeField] private BulletPool piercingPool;

    public override BulletPool ModifyBulletPool(BulletPool basePool)
    {
        return piercingPool;
    }
}
