using UnityEngine;

// Un heroe de prueba. Por ahora los stats se cargan a mano en el asset.
// Cuando exista el equipamiento, este script va a sumar
// los bonus de los 4 slots ademas del valor base.
[CreateAssetMenu(fileName = "Heroe", menuName = "Expedicion/Heroe")]
public class HeroeData : ScriptableObject
{
    public string nombre = "Heroe";

    [Header("Stats base (los que salen del gacha)")]
    public int fuerza = 5;
    public int destreza = 5;
    public int constitucion = 5;
    public int inteligencia = 5;

    public int GetStat(StatType stat)
    {
        switch (stat)
        {
            case StatType.Fuerza:
                return fuerza;
            case StatType.Destreza:
                return destreza;
            case StatType.Constitucion:
                return constitucion;
            case StatType.Inteligencia:
                return inteligencia;
        }

        return 0;
    }
}
