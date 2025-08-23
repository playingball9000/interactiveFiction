using UnityEngine;

public class StatBarManager : MonoBehaviour
{
    [SerializeField] private FoodBar foodBar;
    [SerializeField] private Transform barContainer;

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
        foodBar.ResetProgress();
    }

    public void ReloadBars()
    {
        foodBar.UpdateTotal();
    }

}
