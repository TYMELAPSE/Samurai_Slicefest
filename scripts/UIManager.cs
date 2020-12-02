using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI timeText;

    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI deathReasonText;
    public TextMeshProUGUI highScoreText;

    public GameObject gameOverScreen;
    public GameObject gameplayScreen;
    
    private Vector3 comboTextStartSize = new Vector3(0.5f, 0.5f, 1f);
    private Vector3 comboTextFinalSize = new Vector3(1.2f, 1.2f, 1f);

    private int scale1;
    private int scale2;
    private int alpha1;
    private int alpha2;

    public CountdownManager countdownManager;

    public void UpdateScoreText(int playerScore)
    {
        scoreText.text = "Score: " + playerScore;
    }

    public void UpdateComboText(int combo)
    {
        comboText.text = "x" + combo;

        if (combo > 0)
        {
            StartCoroutine(FadeInComboText());   
        }
    }

    public void UpdateTimeText(string time)
    {
        timeText.text = time;
    }

    public void UpdateFinalScoreText(int score)
    {
        finalScoreText.text = "SCORE: " + score;
        
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    private void ResetComboText()
    {
        comboText.transform.localScale = comboTextStartSize;
        comboText.GetComponent<CanvasGroup>().alpha = 0f;
        
        LeanTween.cancel(scale1);
        LeanTween.cancel(scale2);
        LeanTween.cancel(alpha1);
        LeanTween.cancel(alpha2);
    }

    public void DisplayGameOverScreen(bool show)
    {
        deathReasonText.text = countdownManager.gameOver ? "TIME'S UP" : "YOU DIED";
        
        highScoreText.text = "BEST: " + PlayerPrefs.GetInt("HighScore");
        gameOverScreen.SetActive(show);
    }

    public void DisplayGameplayScreen(bool show)
    {
        UpdateScoreText(0);
        ResetComboText();
        gameplayScreen.SetActive(show);
    }

    IEnumerator FadeInComboText()
    {
        ResetComboText();
        
        scale1 = LeanTween.scale(comboText.gameObject, new Vector3(1f,1f,1f), 0.2f).setEaseOutExpo().id;
        alpha1 = LeanTween.alphaCanvas(comboText.GetComponent<CanvasGroup>(), 1f, 0.2f).id;
        
        yield return new WaitForSeconds(0.2f);
        
        alpha2 = LeanTween.alphaCanvas(comboText.GetComponent<CanvasGroup>(), 0f, 0.5f).id;
        scale2 = LeanTween.scale(comboText.gameObject, comboTextFinalSize, 0.5f).setEaseOutQuad().id;
    }
}
