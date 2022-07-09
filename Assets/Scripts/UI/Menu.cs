using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VaudinGames.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] CanvasGroupFader darkness;
    [SerializeField] List<CanvasGroupFader> menuButtons;
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
        foreach (CanvasGroupFader button in menuButtons)
        {
            DisableCanvasGroup(button.gameObject.GetComponent<CanvasGroup>());
        }
        
    }
    private void Start()
    {
        StartCoroutine(LoadMenu());
    }

    public void OpenOptions()
    {
        StartCoroutine(OpenOptionsRoutine());
    }

    public void CloseOptions()
    {
        StartCoroutine(OpenMenuRoutine());
    }

    private IEnumerator OpenOptionsRoutine()
    {
        yield return menuFader.FadeOut(0.2f);
        DisableCanvasGroup(menu);
        yield return optionsFader.FadeIn(0.2f);
        EnableCanvasGroup(options);
    }

    private IEnumerator OpenMenuRoutine()
    {
        yield return optionsFader.FadeOut(0.2f);
        DisableCanvasGroup(options);
        yield return menuFader.FadeIn(0.2f);
        EnableCanvasGroup(menu);
    }

    private IEnumerator LoadMenu()
    {
        DisableCanvasGroup(options);
        EnableCanvasGroup(menu);
        Coroutine showtitle = darkness.FadeOut(fadeInTime);
        yield return showtitle;
        if (sceneMusic) { StartCoroutine(musicPlayer.PlayMusic()); }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        foreach (CanvasGroupFader button in menuButtons)
        {
            yield return button.FadeIn(0.5f);
        }
        foreach (CanvasGroupFader button in menuButtons)
        {
            EnableCanvasGroup(button.gameObject.GetComponent<CanvasGroup>());
        }
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
