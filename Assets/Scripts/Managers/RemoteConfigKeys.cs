// Las 9 claves de Remote Config, en un solo lugar.
// OJO: las claves solo admiten letras, digitos, puntos, guiones bajos y guiones.
// Nada de eñes ni acentos, por eso "danio" y no "daño".
public static class RemoteConfigKeys
{
    public const string PuertaVidaMaxima = "puerta_vida_maxima";                 // Int
    public const string PuertaDanioPorEnemigo = "puerta_danio_por_enemigo";      // Int
    public const string TorreDanioBase = "torre_danio_base";                     // Int
    public const string TorreAlcance = "torre_alcance";                          // Float
    public const string EnemigoVelocidad = "enemigo_velocidad";                  // Float
    public const string OleadaCantidadCaminantes = "oleada_cantidad_caminantes"; // Int
    public const string OleadaIntervaloSpawn = "oleada_intervalo_spawn";         // Float
    public const string ExpedicionEventosMax = "expedicion_eventos_max";         // Int
    public const string MostrarPorcentajeChecks = "mostrar_porcentaje_checks";   // Bool
}
