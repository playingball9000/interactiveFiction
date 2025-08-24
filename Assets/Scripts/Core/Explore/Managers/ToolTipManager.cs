using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [SerializeField] private GameObject tooltipCanvas;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private RectTransform tooltipBg;

    ITooltip currentProvider;

    private void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    private void Update()
    {
        if (tooltipCanvas.activeSelf && currentProvider != null)
        {
            tooltipText.text = currentProvider.GetTooltipText();
            var position = Input.mousePosition;
            var normalizedPosition = new Vector2(position.x / Screen.width, position.y / Screen.height);
            var pivot = CalculatePivot(normalizedPosition);

            tooltipBg.pivot = pivot;

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent as RectTransform,
                Input.mousePosition,
                null,
                out pos
            );
            tooltipBg.localPosition = pos + new Vector2(10f, -10f);
        }
    }
    private Vector2 CalculatePivot(Vector2 normalizedPosition)
    {
        Vector2 pivot = new Vector2(-0.05f, 1.05f);

        if (normalizedPosition.x > 0.5f)
            pivot.x = 1.05f;

        if (normalizedPosition.y < 0.5f)
            pivot.y = -0.05f;

        return pivot;
    }

    public void ShowTooltipByID(ITooltip tip)
    {
        currentProvider = tip;
        tooltipText.text = tip.GetTooltipText();
        tooltipCanvas.SetActive(true);
    }

    public void HideTooltip()
    {
        // onDisable calls this so it causes errors when the game shuts down
        if (!tooltipCanvas) return;
        currentProvider = null;
        tooltipCanvas.SetActive(false);
    }
}
