using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] float fadeDuration = 2f;
    [SerializeField] float musicVolumeScale = 0.1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;
    }

    public IEnumerator PlayMusic()
    {
        if (audioSource.isPlaying)
        {
            yield return StartCoroutine(FadeOutMusic(2f));
            audioSource.Stop();
        }
        audioSource.volume = 0;
        audioSource.Play();

        yield return StartCoroutine(FadeInMusic(fadeDuration));
    }

    public IEnumerator PlayOneShotMusic(AudioClip music, float volumeScale)
    {
        if (audioSource.isPlaying) 
        {
            yield return StartCoroutine(FadeOutMusic(2f));
            audioSource.Stop();
        }
        audioSource.PlayOneShot(music, volumeScale);

        yield return StartCoroutine(FadeInMusic(fadeDuration));
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOutMusic(fadeDuration));
    }

    public IEnumerator FadeOutMusic(float fadeDuration)
    {
        
        while (audioSource.volume > Mathf.Epsilon)
        {
            audioSource.volume -= Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    private IEnumerator FadeInMusic(float fadeDuration)
    {
        
        while (audioSource.volume < musicVolumeScale)
        {
            audioSource.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    public IEnumerator WaitForMusicToFinish(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
}
