using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VaudinGames.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] CanvasGroupFader darkness;
    [SerializeField] CanvasGroupFader startButton;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float levelTransitionTime = 2f;
    [SerializeField] AudioClip sceneMusic;
    private MusicPlayer musicPlayer;

    private void Awake()
    {
        darkness.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        musicPlayer = FindObjectOfType<MusicPlayer>(); 
        startButton.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }
    private void Start()
    {
        StartCoroutine(LoadMenu());
    }

    private IEnumerator LoadMenu()
    {
        Coroutine showtitle = darkness.FadeOut(fadeInTime);
        yield return showtitle;
        if (sceneMusic) { StartCoroutine(musicPlayer.PlayMusic()); }
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Coroutine showStartButton = startButton.FadeIn(2f);
    }

    public void StartGame()
    {
        StartCoroutine(LoadNextLevel("Overworld 1"));
    }

    public IEnumerator LoadNextLevel(string sceneName)
    {
        darkness.FadeIn(levelTransitionTime);
        musicPlayer.StopMusic();
        yield return new WaitForSeconds(levelTransitionTime);
        SceneManager.LoadScene(sceneName);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
