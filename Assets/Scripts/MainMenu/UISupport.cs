using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISupport : MonoBehaviour {

    [SerializeField] string teaser = "The Abyss Looks Into You";
    [SerializeField] float teaserTime = 1f;
    [SerializeField] Text buttonText;

    [SerializeField] CharacterStatsObj playerStats;
    [SerializeField] CharacterStatsObj wolfStats;


    bool triggerOnce = false;

    public void StartTheGame()
    {
        if (triggerOnce) return;

        playerStats.Init();
        wolfStats.Init();

        StartCoroutine(TeaseAndStart());
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LaunchURL(string url)
    {
        Application.OpenURL(url);
    }

    IEnumerator TeaseAndStart()
    {
        buttonText.text = teaser;

        yield return new WaitForSeconds(teaserTime);

        SceneManager.LoadScene("Town");
    }
}
