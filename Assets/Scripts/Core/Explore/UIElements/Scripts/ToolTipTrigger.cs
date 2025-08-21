using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ITooltip tooltipInfo;

    private void Awake()
    {
        tooltipInfo = GetComponent<ITooltip>();
    }

    private void OnDisable()
    {
        TooltipManager.Instance.HideTooltip();
    }

    private void OnDestroy()
    {
        TooltipManager.Instance.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltipByID(tooltipInfo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
