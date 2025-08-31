using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RpgTextBoxHandler : MonoBehaviour
{
    public Canvas rpgTextBoxCanvas;
    public TextMeshProUGUI rpgTextBox;

    private Queue<string> textQueue = new();

    private bool isTyping = false;
    WaitForSeconds textQueueWait = new(0.7f);

    // Delegates for other functions to use to invoke this. Probably not the right way but oh well.
    public delegate void UpdateRpgTextBoxDelegate(string text);
    public static UpdateRpgTextBoxDelegate invokeUpdateRpgTexBox;

    void OnEnable()
    {
        invokeUpdateRpgTexBox += UpdateRpgTextBox;
        rpgTextBox.color = Color.white;
    }

    void OnDisable()
    {
        invokeUpdateRpgTexBox -= UpdateRpgTextBox;
    }

    public void UpdateRpgTextBox(string text)
    {

        // Using a queue here in case multiple updates come through at once so each one is handled in order
        textQueue.Enqueue(text);
        if (!isTyping)
        {
            StartCoroutine(ProcessTextQueue(rpgTextBox));
        }

    }

    private IEnumerator ProcessTextQueue(TextMeshProUGUI tmpBox)
    {
        tmpBox.text = "";

        isTyping = true;
        while (textQueue.Count > 0)
        {
            var nextText = textQueue.Dequeue();

            yield return StartCoroutine(UiUtilMb.Instance.TypewriterAppend(nextText, tmpBox));
            yield return textQueueWait;
        }
        isTyping = false;
    }
}
