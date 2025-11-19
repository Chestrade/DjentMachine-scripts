using UnityEngine;

/// <summary>
/// Changes the background color over time indefinitely.
/// </summary>
public class BGAnimation : MonoBehaviour
{
    [SerializeField] float colorChangeSpeed = 1f;

    [SerializeField] float satValue = 0.39f;
    [SerializeField] float vValue = 1.00f;

    private SpriteRenderer rend;
    private float hueValue = 0f;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        SetNewBGColor();
    }
    
    void SetNewBGColor()
    {
        hueValue += Time.deltaTime * colorChangeSpeed;
        if(hueValue > 1f)
        {
            hueValue = 0f;
        }

        Color newColor = Color.HSVToRGB(hueValue, satValue, vValue);

        rend.color = newColor;
    }
}
