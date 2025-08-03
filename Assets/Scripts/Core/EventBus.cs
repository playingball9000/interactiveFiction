using System;

public static class GameEvents
{
    public static event Action OnEnterArea;
    public static event Action OnDieInArea;
    // public static event Action OnGameStart;
    // public static event Action<int> OnLevelComplete;
    // public static event Action<bool> OnTogglePause;

    public static void RaiseEnterArea() => OnEnterArea?.Invoke();
    public static void RaiseDieInArea() => OnDieInArea?.Invoke();
    // public static void GameStart() => OnGameStart?.Invoke();
    // public static void LevelComplete(int score) => OnLevelComplete?.Invoke(score);
    // public static void TogglePause(bool isPaused) => OnTogglePause?.Invoke(isPaused);
}
