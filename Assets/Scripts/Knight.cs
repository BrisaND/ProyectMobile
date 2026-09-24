using UnityEngine;

public class Knight : HeroSwitcher
{
    [SerializeField] int damageBonus = 90;
    [SerializeField] float fireRateMultiplier = 1.5f;


    public override int ModifyDamage(int baseDamage)
    {
        return baseDamage + damageBonus;
    }


    public override float ModifyFireRate(float baseFireRate)
    {
        return baseFireRate * fireRateMultiplier;
    }
}
