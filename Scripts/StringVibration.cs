using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Manages animations triggered by the hand strumming on the guitar.
///  This includes string vibrations and speaker wave ripple effects.
/// </summary>
public class StringVibration : MonoBehaviour
{
    [Header("String")]
    [SerializeField] private GameObject stringSpriteObj;
    [SerializeField] private Material blurMat;
    [SerializeField] private float rateOfVibration = 45f;
    [SerializeField] private float vibrationDuration = 1.0f;
    [SerializeField] private float rangeOfVibration = 0.5f;

    [Header("Speakers")]
    [SerializeField] private GameObject[] speakers;
    [SerializeField] private ParticleSystem[] particleSystems;
    [SerializeField] private float speakerAnimationDuration = 0.5f;
    [SerializeField] private float speakerSizeMult = 1.33f;

    private Vector2[] initialSpeakerScales;

    private Vector3 initialPosition;
    

    private void Start()
    {
        initialPosition = stringSpriteObj.transform.position;
        InitializeSpeakerScales();
        
    }

    private void InitializeSpeakerScales()
    {
        initialSpeakerScales = new Vector2[speakers.Length];
        for (int i = 0; i < speakers.Length; i++)
        {
            initialSpeakerScales[i] = speakers[i].transform.localScale;
        }
    }

    public void OnStringTrigger()
    {
        StartCoroutine(VibrateString());
        StartCoroutine(AnimateSpeakers());

        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
    }

    private IEnumerator VibrateString()
    {
        float elapsedTime = 0f;
        while(elapsedTime < vibrationDuration)
        {
            float decayFactor = 1 - (elapsedTime / vibrationDuration);


            float offset = Mathf.Sin(elapsedTime * rateOfVibration) * rangeOfVibration; //multiply by decayFactor if needed.

            float matBlur = Mathf.Lerp(0.1f, 0f, (elapsedTime/vibrationDuration)*decayFactor);

            blurMat.SetFloat("BlurAmount", matBlur);

            stringSpriteObj.transform.position = initialPosition + new Vector3(offset, 0f, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        stringSpriteObj.transform.position = initialPosition;
    }

    private IEnumerator AnimateSpeakers()
    {
        float elapsedTime = 0f;
        
        while(elapsedTime < speakerAnimationDuration)
        {
            float scaleFactor = Mathf.Sin(elapsedTime / speakerAnimationDuration * Mathf.PI);
            scaleFactor = 1 + scaleFactor * speakerSizeMult;

            for(int i = 0; i<speakers.Length; i++)
            {
                speakers[i].transform.localScale = initialSpeakerScales[i] * scaleFactor;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        for(int i = 0; i<speakers.Length;i++)
        {
            speakers[i].transform.localScale = initialSpeakerScales[i];
        }

    }

}
