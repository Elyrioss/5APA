using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private bool loadScene = false;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject loadingMenu;
    [SerializeField]
    private Slider loadingSlider;

    [SerializeField]
    private int scene;
    [SerializeField]
    private TextMeshProUGUI loadingText;

    private bool playClicked = false;

    void Update()
    {
        if (playClicked && !loadScene)
        {
            mainMenu.SetActive(false);

            loadingMenu.SetActive(true);

            loadScene = true;
            
            loadingText.text = "LOADING...";
            
            StartCoroutine(LoadNewScene());
        }
        
        if (loadScene == true)
        {
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
        }

    }
    
    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(3);
        
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        
        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);

            loadingSlider.value = progress;

            yield return null;
        }
    }

    public void PlayButtonClicked()
    {
        playClicked = true;
    }

    public void QuitButtonClicked()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
