using UnityEngine;

public class StatBarManager : MonoBehaviour
{
    [SerializeField] private StatBar healthBar;
    [SerializeField] private StatBar foodBar;
    [SerializeField] private StatBar waterBar;
    [SerializeField] private Transform statsContainer;

    private void OnEnable()
    {
        EventManager.Subscribe(GameEvent.EnterArea, ResetBars);
        EventManager.Subscribe(GameEvent.StatsChanged, ReloadBars);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvent.EnterArea, ResetBars);
        EventManager.Unsubscribe(GameEvent.StatsChanged, ReloadBars);
    }

    public void ResetBars()
    {
        healthBar.ResetProgress();
        foodBar.ResetProgress();
        waterBar.ResetProgress();
    }

    public void ReloadBars()
    {
        healthBar.UpdateTotalFromStats();
        foodBar.UpdateTotalFromStats();
        waterBar.UpdateTotalFromStats();
    }

}
