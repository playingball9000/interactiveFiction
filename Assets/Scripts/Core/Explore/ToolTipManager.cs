using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [SerializeField] private GameObject toolTipCanvas;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private RectTransform toolTipBg;

    // All tooltips stored here
    private Dictionary<string, string> tooltipData = new Dictionary<string, string>()
    {
        { "playButton", "Start the game!" },
        { "settingsButton", "Adjust your game settings." },
        { "quitButton", "Exit the game." }
    };

    private void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    private void Update()

    {
        var position = Input.mousePosition;
        var normalizedPosition = new Vector2(position.x / Screen.width, position.y / Screen.height);
        var pivot = CalculatePivot(normalizedPosition);

        toolTipBg.pivot = pivot;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            Input.mousePosition,
            null,
            out pos
        );
        toolTipBg.localPosition = pos + new Vector2(10f, -10f);
    }

    private Vector2 CalculatePivot(Vector2 normalizedPosition)
    {
        var pivotTopLeft = new Vector2(-0.05f, 1.05f);
        var pivotTopRight = new Vector2(1.05f, 1.05f);
        var pivotBottomLeft = new Vector2(-0.05f, -0.05f);
        var pivotBottomRight = new Vector2(1.05f, -0.05f);

        // Probably have it change pivots closer to the edges
        if (normalizedPosition.x < 0.5f && normalizedPosition.y >= 0.5f)
        {
            return pivotTopLeft;
        }
        else if (normalizedPosition.x > 0.5f && normalizedPosition.y >= 0.5f)
        {
            return pivotTopRight;
        }
        else if (normalizedPosition.x <= 0.5f && normalizedPosition.y < 0.5f)
        {
            return pivotBottomLeft;
        }
        else
        {
            return pivotBottomRight;
        }
    }

    public void ShowTooltipByID(string id)
    {
        tooltipText.text = id;
        toolTipCanvas.SetActive(true);
    }

    public void HideTooltip()
    {
        // onDisable calls this so it causes errors when the game shuts down
        if (!toolTipCanvas) return;
        toolTipCanvas.SetActive(false);
    }
}
