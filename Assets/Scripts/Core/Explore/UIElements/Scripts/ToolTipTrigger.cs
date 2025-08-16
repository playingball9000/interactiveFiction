using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public IToolTip toolTipInfo;

    private void Awake()
    {
        toolTipInfo = GetComponent<IToolTip>();
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
        TooltipManager.Instance.ShowTooltipByID(toolTipInfo.GetToolTipText());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
