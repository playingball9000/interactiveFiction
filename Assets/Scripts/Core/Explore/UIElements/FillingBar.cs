using UnityEngine;
using UnityEngine.UI;

public class FillingBar : MonoBehaviour
{
    [SerializeField] private Image progressImage;

    private float runTime;
    private float currentTime;
    private bool isRunning = false;

    protected virtual void OnEnable()
    {
        Initialize(3f);
        StartFilling();
    }

    public void Initialize(float runTime)
    {
        this.runTime = runTime;
        currentTime = 0f;
        isRunning = false;
        progressImage.fillAmount = 0f;
    }

    protected void Update()
    {
        if (!isRunning) return;

        currentTime += Time.deltaTime;
        float progress = Mathf.Clamp01(currentTime / runTime);
        progressImage.fillAmount = progress;

        if (progress >= 1f)
        {
            isRunning = false;
            // Optionally: Trigger callback when full
        }
    }

    public void StartFilling()
    {
        isRunning = true;
    }

    public void StopFilling()
    {
        isRunning = false;
    }
}
