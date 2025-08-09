using UnityEngine;

public class StatBarManager : MonoBehaviour
{
    [SerializeField] private GameObject healthbar;
    [SerializeField] private Transform barContainer;

    private void OnEnable()
    {
        EventManager.Subscribe(GameEvent.OnEnterArea, ReloadBars);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvent.OnEnterArea, ReloadBars);
    }

    public void ReloadBars()
    {
    }

}
