using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static int score; // Player's score
    [SerializeField]public TextMeshProUGUI scoreText;
    [SerializeField]public TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI scoreMultiplierText;
    public int highScore;
    public int speedBoostMultiplier = 1;
    
    void Start()
    {
        score = 0;
        UpdateScoreUI();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        DisplayHighScore();
        StartCoroutine(IncrementScoreEverySecond());


    }

    IEnumerator IncrementScoreEverySecond()
    {
        while (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
            yield return new WaitForSeconds(1f);
            score += Mathf.RoundToInt(1 * speedBoostMultiplier);
            scoreMultiplierText.text = speedBoostMultiplier + "X";
            if (score > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
            UpdateScoreUI();
        }
    }
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore",score);
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
    void DisplayHighScore()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score:" + highScore;
        }
    }
    public void SetSpeedBoostMultiplier(int multiplier)
    {
        speedBoostMultiplier = multiplier;
    }

    public void ResetSpeedBoostMultiplier()
    {
        speedBoostMultiplier = 1; // Reset to default multiplier
    }
}
