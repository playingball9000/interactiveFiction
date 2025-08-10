using UnityEngine;

public class StatBarManager : MonoBehaviour
{
    [SerializeField] private FoodBar foodBar;
    [SerializeField] private Transform barContainer;

    private void OnEnable()
    {
        EventManager.Subscribe(GameEvent.EnterArea, ReloadBars);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvent.EnterArea, ReloadBars);
    }

    public void ReloadBars()
    {
        Debug.Log("stat bar manager - relaodbars");

    }

}
