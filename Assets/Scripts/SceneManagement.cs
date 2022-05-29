using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VaudinGames.UI;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] CanvasGroupFader darkness;
    [SerializeField] CanvasGroupFader deathScreen;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float levelTransitionTime = 2f;
    [SerializeField] float deathFadeTime = 1f;
    [SerializeField] AudioClip deathMusic;
    [SerializeField] Button sceneButton;
    private MusicPlayer musicPlayer;

    private void Start()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        darkness.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        darkness.FadeOut(fadeInTime);
        deathScreen.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        PlayerCombat.playerDied += OnPlayerDeath;
        sceneButton.interactable = false;
    }
    public IEnumerator RestartLevel()
    {
        darkness.FadeIn(deathFadeTime);
        yield return new WaitForSeconds(deathFadeTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator LoadNextLevel(string sceneName)
    {
        darkness.FadeIn(levelTransitionTime);
        yield return new WaitForSeconds(levelTransitionTime);
        SceneManager.LoadScene(sceneName);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnPlayerDeath()
    {
        StartCoroutine(PlayerDeath());
    }

    private IEnumerator PlayerDeath()
    {
        Coroutine showDeathCanvas = deathScreen.FadeIn(4f);
        Coroutine playDeathMusic = StartCoroutine(musicPlayer.PlayMusic(deathMusic));
        Coroutine waitForDeathMusic = StartCoroutine(musicPlayer.WaitForMusicToFinish(deathMusic.length));

        yield return showDeathCanvas;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        sceneButton.interactable = true;
        yield return playDeathMusic;
        yield return waitForDeathMusic;

    }

    public void OnDeathScreenButtonPress()
    {
        if (SceneManager.GetActiveScene().name.Contains("Underworld"))
        {
            StartCoroutine(LoadNextLevel("Overworld 1"));
        }
        else
        {
            StartCoroutine(LoadNextLevel("Underworld 1"));
        }
    }

    private void OnDisable()
    {
        PlayerCombat.playerDied -= OnPlayerDeath;
    }
}
