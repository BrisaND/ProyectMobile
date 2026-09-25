using System;
using System.Collections.Generic;

public static class EventManager
{
    private static readonly Dictionary<string, Action> eventTable = new Dictionary<string, Action>();
    private static readonly Dictionary<string, Delegate> eventTableWithParam = new Dictionary<string, Delegate>();

    #region Eventos sin Parámetros
    public static void Subscribe(string eventName, Action listener)
    {
        if (!eventTable.ContainsKey(eventName)) eventTable[eventName] = null;
        eventTable[eventName] += listener;
    }

    public static void Unsubscribe(string eventName, Action listener)
    {
        if (eventTable.ContainsKey(eventName))
        {
            eventTable[eventName] -= listener;
            if (eventTable[eventName] == null) eventTable.Remove(eventName);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        if (eventTable.TryGetValue(eventName, out Action thisEvent)) thisEvent?.Invoke();
    }
    #endregion

    #region Eventos Genéricos (con Datos)
    public static void Subscribe<T>(string eventName, Action<T> listener)
    {
        if (!eventTableWithParam.ContainsKey(eventName)) eventTableWithParam[eventName] = null;
        eventTableWithParam[eventName] = (Action<T>)eventTableWithParam[eventName] + listener;
    }

    public static void Unsubscribe<T>(string eventName, Action<T> listener)
    {
        if (eventTableWithParam.ContainsKey(eventName))
        {
            eventTableWithParam[eventName] = (Action<T>)eventTableWithParam[eventName] - listener;
            if (eventTableWithParam[eventName] == null) eventTableWithParam.Remove(eventName);
        }
    }

    public static void TriggerEvent<T>(string eventName, T parameter)
    {
        if (eventTableWithParam.TryGetValue(eventName, out Delegate thisEvent))
        {
            (thisEvent as Action<T>)?.Invoke(parameter);
        }
    }
    #endregion

    public static void ClearAllEvents()
    {
        eventTable.Clear();
        eventTableWithParam.Clear();
    }
}