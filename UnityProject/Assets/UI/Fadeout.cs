using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fadeout : MonoBehaviour
{
    public List<Image> images;
    public List<TextMeshProUGUI> texts;
    public float duration;
    private float maxduration;

    private List<KeyValuePair<Image, float>> imageInitialAlphas;
    private List<KeyValuePair<TextMeshProUGUI, float>> textInitialAlphas;

    private void Awake()
    {
        maxduration = duration;

        imageInitialAlphas = new List<KeyValuePair<Image, float>>();
        textInitialAlphas = new List<KeyValuePair<TextMeshProUGUI, float>>();

        foreach (var image in images)
        {
            if (image != null)
            {
                imageInitialAlphas.Add(new KeyValuePair<Image, float>(image, image.color.a));
            }
        }

        foreach (var text in texts)
        {
            if (text != null)
            {
                textInitialAlphas.Add(new KeyValuePair<TextMeshProUGUI, float>(text, text.color.a));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (duration > 0)
        {
            float fadeFactor = duration / maxduration;

            foreach (var pair in textInitialAlphas)
            {
                TextMeshProUGUI text = pair.Key;
                float initialAlpha = pair.Value;

                if (text != null)
                {
                    Color color = text.color;
                    color.a = initialAlpha * fadeFactor;
                    text.color = color;
                }
            }

            foreach (var pair in imageInitialAlphas)
            {
                Image image = pair.Key;
                float initialAlpha = pair.Value;

                if (image != null)
                {
                    Color color = image.color;
                    color.a = initialAlpha * fadeFactor;
                    image.color = color;
                }
            }

            duration -= Time.deltaTime;
        }
        else //Reset and disable
        {
            this.enabled = false;

            duration = maxduration;

            foreach (var pair in textInitialAlphas)
            {
                TextMeshProUGUI text = pair.Key;
                float initialAlpha = pair.Value;

                if (text != null)
                {
                    Color color = text.color;
                    color.a = initialAlpha;
                    text.color = color;
                    text.gameObject.SetActive(false);
                }
            }

            foreach (var pair in imageInitialAlphas)
            {
                Image image = pair.Key;
                float initialAlpha = pair.Value;

                if (image != null)
                {
                    Color color = image.color;
                    color.a = initialAlpha;
                    image.color = color;
                    image.gameObject.SetActive(false);
                }
            }
        }
    }
}
