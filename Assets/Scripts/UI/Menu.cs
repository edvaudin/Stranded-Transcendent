using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VaudinGames.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] CanvasGroupFader darkness;
    [SerializeField] CanvasGroupFader startButton;
    [SerializeField] CanvasGroupFader menuFader;
    [SerializeField] CanvasGroupFader optionsFader;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float levelTransitionTime = 2f;
    [SerializeField] AudioClip sceneMusic;
    private MusicPlayer musicPlayer;
    private CanvasGroup options;
    private CanvasGroup menu;

    private void Awake()
    {
        darkness.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        options = optionsFader.GetComponent<CanvasGroup>();
        menu = menuFader.GetComponent<CanvasGroup>();
        musicPlayer = FindObjectOfType<MusicPlayer>(); 
        startButton.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }
    private void Start()
    {
        StartCoroutine(LoadMenu());
    }

    public void OpenOptions()
    {
        StartCoroutine(OpenOptionsRoutine());
    }

    private IEnumerator OpenOptionsRoutine()
    {
        yield return menuFader.FadeOut(0.2f);
        DisableCanvasGroup(menu);
        yield return optionsFader.FadeIn(0.2f);
        EnableCanvasGroup(options);
    }

    private IEnumerator LoadMenu()
    {
        DisableCanvasGroup(options);
        EnableCanvasGroup(menu);
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

    private void EnableCanvasGroup(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    private void DisableCanvasGroup(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}
