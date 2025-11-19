using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

///<summary>
/// Controls the hand movement arround the screen and palm muting.
///</summary>

public class HandController : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private float handScaleOnPalmMuteMult = .9f;
    [SerializeField] private float delayBetweenStringHits = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] palmMuteHits;
    [SerializeField] private AudioClip[] openStringHits;
    [SerializeField] private AudioClip[] cymbalHits;

    [Header("References")]
    [SerializeField] private AudioSource[] audioSource;
    [SerializeField] private StringVibration stringVibration;
    public bool palmMuting { get; private set; } = false;
    private Vector3 mousePos;
    private Vector3 initialHandScale;
    

    private Vector3 previousPosition;
    private bool stringJustHit = false;

    private void Start()
    {
        initialHandScale = transform.localScale;
        Cursor.visible = false;
    }

    //Sets this game object to mouse position
    void OnPoint(InputValue value)
    {
        Vector2 mousePositionInput = value.Get<Vector2>();

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePositionInput.x, mousePositionInput.y, 10f));
        targetPosition.z = 0f;
        
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, distance);
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider != null && hit.collider.CompareTag("String"))
            {
                if (!stringJustHit)
                {
                    HitString();
                }
            }
        }

        transform.position = targetPosition;
        previousPosition = transform.position;
    }

    //Manages palm muting
    void OnRightClick(InputValue value)
    {
        PalmMute(value);
    }

    void OnKeyPalmMute(InputValue value)
    {
        PalmMute(value);
    }

    void OnKeyStrum(InputValue value)
    {
        if (value.isPressed)
        {
            HitString();
        }
    }

    void PalmMute(InputValue value)
    {
        if (value.isPressed)
        {
            palmMuting = true;
            transform.localScale = new Vector3(initialHandScale.x * handScaleOnPalmMuteMult, initialHandScale.y * handScaleOnPalmMuteMult, initialHandScale.z * handScaleOnPalmMuteMult);
        }
        else
        {
            palmMuting = false;
            transform.localScale = initialHandScale;
        }
    }

    private void HitString()
    {
        if (palmMuting)
        {
            StartCoroutine(PlayRandomPalmMute());
        }
        else
        {
            StartCoroutine(PlayRandomOpenString());
        }

        stringVibration.OnStringTrigger();
    }

    private IEnumerator PlayRandomPalmMute()
    {
        stringJustHit = true;

        int randomIndex = Random.Range(0, palmMuteHits.Length - 1);
        audioSource[0].Stop();
        audioSource[0].PlayOneShot(palmMuteHits[randomIndex]);

        yield return new WaitForSeconds(delayBetweenStringHits);
        stringJustHit = false;
    }

    private IEnumerator PlayRandomOpenString()
    {
        stringJustHit = true;

        int randomIndex = Random.Range(0, openStringHits.Length - 1);
        audioSource[0].Stop();
        audioSource[0].PlayOneShot(openStringHits[randomIndex]);

        int randomCymb = Random.Range(0, cymbalHits.Length - 1);
        audioSource[1].PlayOneShot(cymbalHits[randomCymb]);

        yield return new WaitForSeconds(delayBetweenStringHits);
        stringJustHit = false;
    }
   
}
