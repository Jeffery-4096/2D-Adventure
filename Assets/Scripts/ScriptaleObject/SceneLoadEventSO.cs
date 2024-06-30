using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;

/// <summary>
/// Scene load request
/// </summary>
/// <param name="locationToLoad">Scenes to load</param>
/// <param name="posToGo">Destination position of Player</param>
/// <param name="fadeScreen">Whether to fade in and out</param>

    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(locationToLoad, posToGo, fadeScreen);
    }
}
