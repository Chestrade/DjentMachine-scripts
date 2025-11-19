using UnityEngine;
using System.Collections;

/// <summary>
///  Manages cursor transperency when hovering over tempo options.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CursorRegion : MonoBehaviour
{

    [SerializeField] private float transparencyAnimDuration = 1f;
    [SerializeField] private SpriteRenderer handSprite;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pick"))
        {
            Cursor.visible = true;

            StartCoroutine(SetHandTransparency(1f, 0.1f));
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Pick"))
        {
            Cursor.visible = false;
            StartCoroutine(SetHandTransparency(0.1f, 1f));
        }
    }

    private IEnumerator SetHandTransparency(float startValue,  float endValue)
    {
        float elapsedTime = 0f;

        while (elapsedTime < transparencyAnimDuration)
        {
            float t = elapsedTime / transparencyAnimDuration;
            float currentTransparency = Mathf.Lerp(startValue, endValue, t);
            handSprite.color = new Color(handSprite.color.r, handSprite.color.g, handSprite.color.b, currentTransparency);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
