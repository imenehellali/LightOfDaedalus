using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AmbientAudioPlayer : MonoBehaviour
{
    public enum Soundscape
    {
        Outside,
        Dungeon,
        MainMenu,
    }

    public static AmbientAudioPlayer Instance { get; private set; }

    /// <summary> AudioClips for ambient sounds. Enter them in the same order as the enum Soundscape is (and add new enum entry if necessary) </summary>
    [SerializeField]
    private AudioClip[] m_audioClips = new AudioClip[3];
    private AudioSource m_audioSource;

    private double m_nextStartTime = AudioSettings.dspTime + 0.2;
    private Soundscape m_currentSoundscape;

    private void Awake()
    {
        Debug.Log("<color=red> Awake! </color>");
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(transform);

        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.clip = m_audioClips[(int)Soundscape.MainMenu];
        m_audioSource.PlayScheduled(m_nextStartTime);
    }

    public void ToggleSoundScape(Soundscape soundscape)
    {
        m_audioSource.Stop();
        m_audioSource.clip = m_audioClips[(int)soundscape];
        m_audioSource.Play();
    }

    public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParameter, float duration, float targetVolume)
    {
        float currentTime = 0f;
        float currentVolume;
        audioMixer.GetFloat(exposedParameter, out currentVolume);
        currentVolume = Mathf.Pow(10, currentVolume / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVolume, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParameter, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }

   
}
