public static class GameEvents
{
    // Puerta
    public const string DoorHit = "DoorHit";                // int: daño recibido a distancia
    public const string DoorDamaged = "DoorDamaged";        // int: HP restante
    public const string DoorDestroyed = "DoorDestroyed";    // sin parametros

    // Enemigos
    public const string EnemySpawned = "EnemySpawned";      // sin parametros
    public const string EnemyDamaged = "EnemyDamaged";      // sin parametros
    public const string EnemyDied = "EnemyDied";            // GameObject: el que murio

    // Oleada
    public const string WaveFinished = "WaveFinished";      // sin parametros

    // Torres
    public const string TurretShot = "TurretShot";          // sin parametros
    public const string TurretDisabled = "TurretDisabled";  // sin parametros

    // Configuracion remota
    public const string RemoteConfigReady = "RemoteConfigReady"; // sin parametros
}