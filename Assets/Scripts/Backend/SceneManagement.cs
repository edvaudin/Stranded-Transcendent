using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VaudinGames.UI;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] CanvasGroupFader darkness;
    [SerializeField] CanvasGroupFader deathScreen;
    [SerializeField] CanvasGroupFader deathButton;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float levelTransitionTime = 2f;
    [SerializeField] float deathFadeTime = 1f;
    [SerializeField] AudioClip deathMusic;
    [SerializeField] Button sceneButton;
    protected MusicPlayer musicPlayer;
    [SerializeField] AudioClip sceneMusic;
    private PlayerInput playerInput;
    Coroutine waitForKeyboardRoutine = null;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    protected virtual void Start()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        if (sceneMusic) {
            StartCoroutine(musicPlayer.PlayMusic());
        }
        darkness.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        darkness.FadeOut(fadeInTime);
        deathScreen.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        deathButton.gameObject.GetComponent<CanvasGroup>().alpha = 0;   
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
        musicPlayer.StopMusic();
        yield return new WaitForSeconds(levelTransitionTime);
        GameManager.Instance.UpdateGameState(GameState.Playing);
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
        waitForKeyboardRoutine = StartCoroutine(WaitForKeyboard());
        Coroutine showDeathCanvas = deathScreen.FadeIn(4f);
        
        Coroutine playDeathMusic = StartCoroutine(musicPlayer.PlayOneShotMusic(deathMusic, 3f));
        Coroutine waitForDeathMusic = StartCoroutine(musicPlayer.WaitForMusicToFinish(deathMusic.length));

        yield return showDeathCanvas;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        sceneButton.interactable = true;
        yield return playDeathMusic;
        Coroutine showDeathButton = deathButton.FadeIn(2f);
        yield return waitForDeathMusic;
    }

    private IEnumerator WaitForKeyboard()
    {
        while (!playerInput.actions["Continue"].IsPressed())
        {
            yield return null;
        }
        OnDeathScreenButtonPress();
    }
    public void OnDeathScreenButtonPress()
    {
        if (waitForKeyboardRoutine != null) { StopCoroutine(waitForKeyboardRoutine); }
        if (SceneManager.GetActiveScene().name.Contains("Underworld"))
        {
            Destroy(FindObjectOfType<PlayerStatManager>().gameObject);
            StartCoroutine(LoadNextLevel("Menu"));
        }
        else
        {
            StartCoroutine(LoadNextLevel("Underworld 1"));
        }
    }

    protected void HideCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    protected void EnableCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    protected virtual void OnDisable()
    {
        PlayerCombat.playerDied -= OnPlayerDeath;
    }
}
