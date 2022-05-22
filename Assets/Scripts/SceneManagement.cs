using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VaudinGames.UI;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] CanvasGroupFader cgf;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float levelTransitionTime = 2f;
    [SerializeField] float deathFadeTime = 1f;

    private void Start()
    {
        cgf.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        cgf.FadeOut(fadeInTime);
    }
    public IEnumerator RestartLevel()
    {
        cgf.FadeIn(deathFadeTime);
        yield return new WaitForSeconds(deathFadeTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator LoadNextLevel(string sceneName)
    {
        cgf.FadeIn(levelTransitionTime);
        yield return new WaitForSeconds(levelTransitionTime);
        SceneManager.LoadScene(sceneName);

    }
}
