using System.Collections;
using TMPro;
using UnityEngine;

///<summary>
/// Manages the drum sequencer.
///</summary>
public class DrumSequencer : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip hiHat;
    [SerializeField] private AudioClip snare;

    [Header("Sequencer Settings")]
    [SerializeField] private float tempo = 120f;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI tempoText;

    private float beatInterval;
    private int currentBeat = 0;
    private AudioSource audioSource;

   
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        CalculateBeatInterval();
        StartCoroutine(PlayBeat());
        SetTempoText(tempo);

    }

    void CalculateBeatInterval()
    {
        beatInterval = 60f / tempo;
    }

    private void PlaySample(AudioClip clip)
    {
        if(clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private IEnumerator PlayBeat()
    {
        while(true)
        {
            PlaySample(hiHat);
            
            if(currentBeat == 2)
            {
                PlaySample(snare);
            }

            currentBeat = (currentBeat + 1) % 4;

            yield return new WaitForSeconds(beatInterval);
        }
    }

    private float ClampTempo(float newTempo)
    {
        return Mathf.Clamp(newTempo, 60f, 300f);
    }

    public void ChangeTempo(float tempoToAdd)
    {
        tempo += tempoToAdd;
        tempo = ClampTempo(tempo);
        CalculateBeatInterval();
        SetTempoText(tempo);
    }

    private void SetTempoText(float newTempo)
    {
        tempoText.text = newTempo.ToString() + " BPM";
    }
}
