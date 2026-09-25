using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

// Trae los valores del dashboard de Unity Cloud y los deja disponibles
// para todo el juego. Sobrevive a los cambios de escena.
//
// Si no hay internet, el proyecto no esta linkeado, o todavia no termino
// de descargar, los getters devuelven el valor por defecto que le pasa
// cada script. O sea: el juego SIEMPRE funciona, con o sin conexion.
public class RemoteConfigManager : MonoBehaviour
{
    public static RemoteConfigManager Instance;
    public static bool Listo { get; private set; }

    // Remote Config exige estas dos structs aunque esten vacias.
    // Sirven para segmentar usuarios; nosotros no las usamos.
    public struct userAttributes { }
    public struct appAttributes { }

    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        await Inicializar();
    }

    private async Task Inicializar()
    {
        try
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            RemoteConfigService.Instance.FetchCompleted += OnFetchCompleted;
            await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Remote Config no disponible. Se usan los valores del inspector. " + e.Message);
            Listo = false;
            EventManager.TriggerEvent(GameEvents.RemoteConfigReady);
        }
    }

    private void OnFetchCompleted(ConfigResponse response)
    {
        Listo = response.requestOrigin != ConfigOrigin.Default;

        if (Listo)
        {
            Debug.Log("Remote Config cargado desde " + response.requestOrigin);
        }
        else
        {
            Debug.LogWarning("Remote Config devolvio valores por defecto.");
        }

        // Los scripts que ya arrancaron se enteran y se reconfiguran.
        EventManager.TriggerEvent(GameEvents.RemoteConfigReady);
    }

    private void OnDestroy()
    {
        if (Instance == this && Listo)
        {
            RemoteConfigService.Instance.FetchCompleted -= OnFetchCompleted;
        }
    }

    // ---- Getters. Cada uno recibe el valor por defecto del propio script,
    //      asi nunca hay un numero magico escondido aca adentro.

    public static int GetInt(string clave, int porDefecto)
    {
        if (!Listo) return porDefecto;
        return RemoteConfigService.Instance.appConfig.GetInt(clave, porDefecto);
    }

    public static float GetFloat(string clave, float porDefecto)
    {
        if (!Listo) return porDefecto;
        return RemoteConfigService.Instance.appConfig.GetFloat(clave, porDefecto);
    }

    public static bool GetBool(string clave, bool porDefecto)
    {
        if (!Listo) return porDefecto;
        return RemoteConfigService.Instance.appConfig.GetBool(clave, porDefecto);
    }
}
