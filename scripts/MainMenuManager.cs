using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;
using Toggle = UnityEngine.UI.Toggle;

public class MainMenuManager : MonoBehaviour
{
    public GameObject howToPlayScreen;
    public GameObject transitionScreen;

    public TextMeshProUGUI basicTutorialText;
    public TextMeshProUGUI strategiesTutorialText;
    public TextMeshProUGUI tutorialText;

    public TextMeshProUGUI basicTutorialTextTemplate;
    public TextMeshProUGUI strategyTutorialTextTemplate;

    public Toggle toggleBlood;

    public VideoPlayer videoPlayer;
    public VideoClip strategiesTutorialVideo;
    public VideoClip basicsTutorialVideo;

    private float _largeFontSize = 30.3f;
    private float _smallFontSize = 19.3f;

    private float _movementTime = 0.8f;
    private float _hideTransitionScreen = -1116f;

    void Start()
    {
        toggleBlood.isOn = GlobalGameSettings.hideBlood;
        LeanTween.moveLocalX(transitionScreen, _hideTransitionScreen, _movementTime);
    }
    public void DisplayHowToPlayScreen()
    {
        DisplayBasicsTutorial();
        LeanTween.moveLocalX(howToPlayScreen, 0f, _movementTime).setEaseOutExpo();
    }

    public void HideHowToPlayScreen()
    {
        videoPlayer.Stop();
        LeanTween.moveLocalX(howToPlayScreen, -836f, _movementTime).setEaseOutExpo();
    }

    public void StartGame()
    {
        //TODO implement logic for randomly selecting level
        //TODO figure out why 5000f is required and _hideTransitionScreen snaps to lower value
        transitionScreen.transform.position = new Vector3(5000f, transitionScreen.transform.position.y, transitionScreen.transform.position.z);
        LeanTween.moveLocalX(transitionScreen, 0f, 0.5f);
        StartCoroutine(LoadNewMap());
    }

    public void ToggleBlood()
    {
        GlobalGameSettings.hideBlood = !GlobalGameSettings.hideBlood;
        Debug.Log(GlobalGameSettings.hideBlood);
    }

    public void DisplayBasicsTutorial()
    {
        basicTutorialText.color = Color.white;
        basicTutorialText.fontSize = _largeFontSize;
        strategiesTutorialText.color = Color.black;
        strategiesTutorialText.fontSize = _smallFontSize;

        tutorialText.text = basicTutorialTextTemplate.text;
        videoPlayer.clip = basicsTutorialVideo;
        videoPlayer.Play();
    }

    public void DisplayStrategiesTutorial()
    {
        basicTutorialText.color = Color.black;
        basicTutorialText.fontSize = _smallFontSize;
        strategiesTutorialText.color = Color.white;
        strategiesTutorialText.fontSize = _largeFontSize;

        tutorialText.text = strategyTutorialTextTemplate.text;
        videoPlayer.clip = strategiesTutorialVideo;
        videoPlayer.Play();
    }

    private IEnumerator LoadNewMap()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("SampleScene");
    }
}
