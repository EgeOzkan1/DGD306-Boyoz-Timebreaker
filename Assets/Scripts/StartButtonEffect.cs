using UnityEngine;
using UnityEngine.UI;

public class StartButtonEffect: MonoBehaviour
{
    public Image image;
    public float blinkInterval = 0.5f; 

    private bool isVisible = true;
    private float timer;

    void Start()
    {
        if (image == null)
            image = GetComponent<Image>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= blinkInterval)
        {
            isVisible = !isVisible; 
            Color color = image.color;
            color.a = isVisible ? 1f : 0f; 
            image.color = color;

            timer = 0f;
        }
    }
}