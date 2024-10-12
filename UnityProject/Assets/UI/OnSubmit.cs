using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TMP_InputField))]
public class OnSubmit : MonoBehaviour
{
    private TMP_InputField inpField;

    // Start is called before the first frame update
    void Start()
    {
        inpField = this.GetComponent<TMP_InputField>();
        inpField.onSubmit.AddListener(onSubmit);
    }

    public UnityEvent OnCorrect;
    public UnityEvent OnIncorrect;
    public List<string> acceptedAnswers;

    private void onSubmit(string msg)
    {
        if (inpField.wasCanceled) return;

        var msgLowered = msg.ToLower().Trim();
        foreach (var answer in acceptedAnswers)
        {
            if (msgLowered.Contains(answer.ToLower()))
            {
                OnCorrect?.Invoke();
                return;
            }
        }
        OnIncorrect?.Invoke();
    }
}
