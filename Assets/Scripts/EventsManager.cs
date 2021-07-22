using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using Facebook.Unity;
using TMPro;
using UnityEngine.SceneManagement;

public class EventsManager : MonoBehaviour
{
    private static IEventsHandler eventsHandler;

    void Awake()
    {
        DontDestroyOnLoad(this);
        eventsHandler = new DebugEventsHandler();
        if (eventsHandler != null)
            eventsHandler.Initialize();
    }

    public static void LogLevelStartedEvent(int levelIndex, EventParams param)
    {
        if (eventsHandler != null)
            eventsHandler.LogLevelStartedEvent(levelIndex, param);
    }

    public static void LogLevelFailedEvent(int levelIndex, EventParams param)
    {
        if (eventsHandler != null)
            eventsHandler.LogLevelFailedEvent(levelIndex, param);
    }

    public static void LogLevelCompleteEvent(int levelIndex, EventParams param)
    {
        if (eventsHandler != null)
            eventsHandler.LogLevelCompleteEvent(levelIndex, param);
    }

    public static void LogLevelRestartEvent(int levelIndex, EventParams param)
    {
        if (eventsHandler != null)
            eventsHandler.LogLevelRestartEvent(levelIndex, param);
    }
}

public interface IEventsHandler
{
    bool bIsInitialized { get; set; }
    void Initialize();
    void LogLevelCompleteEvent(int levelIndex, EventParams param);
    void LogLevelFailedEvent(int levelIndex, EventParams param);
    void LogLevelRestartEvent(int levelIndex, EventParams param);
    void LogLevelStartedEvent(int levelIndex, EventParams param);
}

public class EventParams
{
    public float score;
}

public class DebugEventsHandler : IEventsHandler
{
    public bool bIsInitialized { get; set; }
    private bool bIsDebugging = false;
    public void Initialize()
    {
        bIsInitialized = true;
        if (bIsDebugging)
            Debug.Log("EditorDebugEventsHandler is initialized");
    }

    public void LogLevelCompleteEvent(int levelIndex, EventParams param)
    {
        if (bIsDebugging)
            Debug.Log($"EditorDebugEventsHandler:LogLevelCompleteEvent for level: {levelIndex}");
    }

    public void LogLevelFailedEvent(int levelIndex, EventParams param)
    {
        if (bIsDebugging)
            Debug.Log($"EditorDebugEventsHandler:LogLevelFailedEvent for level: {levelIndex}");
    }

    public void LogLevelRestartEvent(int levelIndex, EventParams param)
    {
        if (bIsDebugging)
            Debug.Log($"EditorDebugEventsHandler:LogLevelRestartEvent for level: {levelIndex}");
    }

    public void LogLevelStartedEvent(int levelIndex, EventParams param)
    {
        if (bIsDebugging)
            Debug.Log($"EditorDebugEventsHandler:LogLevelStartedEvent for level: {levelIndex}");
    }
}

/*
public class TinySauceEventsHandler : IEventsHandler
{
    public bool bIsInitialized { get; set; }

    public void Initialize()
    {

    }

    public void LogLevelCompleteEvent(int levelIndex, EventParams param)
    {
        TinySauce.OnGameFinished(true, param.score, levelIndex.ToString());
    }

    public void LogLevelFailedEvent(int levelIndex, EventParams param)
    {
        TinySauce.OnGameFinished(false, param.score, levelIndex.ToString());
    }

    public void LogLevelRestartEvent(int levelIndex, EventParams param)
    {
        TinySauce.OnGameFinished(false, param.score, levelIndex.ToString());
    }

    public void LogLevelStartedEvent(int levelIndex, EventParams param)
    {
        TinySauce.OnGameStarted(levelIndex.ToString());
    }
}
*/
/*
public class FacebookEventsHandler : IEventsHandler
{
    public bool bIsInitialized { get { return FB.IsInitialized; } set { } }

    public void Initialize()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                {
                    FB.ActivateApp();

                }

            });
        }
    }
    public void LogLevelStartedEvent(int levelIndex, EventParams param)
    {
        if (!bIsInitialized)
        {
            return;
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        string levelName = SceneManager.GetSceneAt(levelIndex) != null ? SceneManager.GetSceneAt(levelIndex).name : $"LevelIndex:{levelIndex}";
        parameters[AppEventParameterName.Level] = levelName;
        //parameters[AppEventParameterName.Description] = "";

        FB.LogAppEvent(
            "LevelStarted",
            null,
            parameters
        );
    }
    public void LogLevelCompleteEvent(int levelIndex, EventParams param)
    {
        if (!bIsInitialized)
        {
            return;
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        string levelName = SceneManager.GetSceneAt(levelIndex) != null ? SceneManager.GetSceneAt(levelIndex).name : $"LevelIndex:{levelIndex}";
        parameters[AppEventParameterName.Level] = levelName;

        FB.LogAppEvent(
            "LevelComplete",
            null,
            parameters
        );
    }

    public void LogLevelRestartEvent(int levelIndex, EventParams param)
    {
        if (!bIsInitialized)
        {
            return;
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        string levelName = SceneManager.GetSceneAt(levelIndex) != null ? SceneManager.GetSceneAt(levelIndex).name : $"LevelIndex:{levelIndex}";
        parameters[AppEventParameterName.Level] = levelName;

        FB.LogAppEvent(
            "LevelRestart",
            null,
            parameters
        );
    }

    public void LogLevelFailedEvent(int levelIndex, EventParams param)
    {
        if (!bIsInitialized)
        {
            return;
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        string levelName = SceneManager.GetSceneAt(levelIndex) != null ? SceneManager.GetSceneAt(levelIndex).name : $"LevelIndex:{levelIndex}";
        parameters[AppEventParameterName.Level] = levelName;

        FB.LogAppEvent(
            "LevelFailed",
            null,
            parameters
        );
    }
}
*/
