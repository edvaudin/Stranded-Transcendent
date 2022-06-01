using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VaudinGames.UI;
using UnityEngine.UI;

public class UnderworldManagement : SceneManagement
{
    [SerializeField] CanvasGroupFader victoryScreen;
    [SerializeField] CanvasGroupFader victoryButtonFader;
    [SerializeField] AudioClip victoryMusic;
    [SerializeField] Button victoryButton;
    private CanvasGroup victoryCg;

    protected override void Start()
    {
        base.Start();
        BossSpawner.allBossesKilled += TriggerVictory;
        victoryButton.interactable = false;
        victoryCg = victoryScreen.GetComponent<CanvasGroup>();
        HideCanvasGroup(victoryCg);
    }

    private void TriggerVictory()
    {
        StartCoroutine(Victory());
    }

    private IEnumerator Victory()
    {
        EnableCanvasGroup(victoryCg);
        Coroutine showVictoryCanvas = victoryScreen.FadeIn(4f);

        Coroutine playVictoryMusic = StartCoroutine(musicPlayer.PlayOneShotMusic(victoryMusic, 3));
        Coroutine waitForVictoryMusic = StartCoroutine(musicPlayer.WaitForMusicToFinish(victoryMusic.length));

        yield return showVictoryCanvas;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        victoryButton.interactable = true;
        yield return playVictoryMusic;
        Coroutine showVictoryButton = victoryButtonFader.FadeIn(2f);
        yield return waitForVictoryMusic;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        BossSpawner.allBossesKilled -= TriggerVictory;
    }
}
