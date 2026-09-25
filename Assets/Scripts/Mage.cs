using UnityEngine;

// MAGO
// Segun el GDD: el proyectil se vuelve elemental y pega en area chica,
// a cambio de menos daño a objetivo unico.
public class Mage : HeroSwitcher
{
    [SerializeField] private BulletPool explosivePool;

    [Tooltip("Menos de 1 = pega mas flojo a un solo objetivo, que es el precio del area.")]
    [SerializeField] private float damageMultiplier = 0.6f;

    [Tooltip("Menos de 1 = dispara mas lento, porque cada tiro vale mas.")]
    [SerializeField] private float fireRateMultiplier = 0.8f;

    public override BulletPool ModifyBulletPool(BulletPool basePool)
    {
        if (explosivePool == null)
        {
            return basePool;
        }

        return explosivePool;
    }

    public override int ModifyDamage(int baseDamage)
    {
        return Mathf.Max(1, Mathf.RoundToInt(baseDamage * damageMultiplier));
    }

    public override float ModifyFireRate(float baseFireRate)
    {
        return baseFireRate * fireRateMultiplier;
    }
}
