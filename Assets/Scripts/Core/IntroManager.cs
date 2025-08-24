using System.Collections;
using UnityEngine;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] paragraphs; // Fill in from Inspector
    public TextMeshProUGUI introText;

    private int currentIndex = 0;
    private bool waitingForInput = false;

    void Start()
    {
        if (paragraphs.Length > 0)
        {
            StartCoroutine(PlayIntro());
        }
    }

    IEnumerator PlayIntro()
    {
        while (currentIndex < paragraphs.Length)
        {
            introText.text = paragraphs[currentIndex];
            waitingForInput = true;

            // Wait until any key is pressed
            yield return new WaitUntil(() => Input.anyKeyDown);

            waitingForInput = false;
            currentIndex++;
        }

        EndIntro();
    }

    void EndIntro()
    {
        // Do whatever comes next (e.g., load menu, fade out, etc.)
        Debug.Log("Intro finished!");
    }
}
