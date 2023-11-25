using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameFinish : MonoBehaviour
{
    
    
    [Header("Game Finish")]
    [SerializeField]
    private GameObject gameFinishScreen;

    [SerializeField]
    private GameObject gameOverTitle;

    [SerializeField]
    private GameObject gameWinTitle;

    [SerializeField]
    private TextMeshProUGUI playTimeText;


    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private Image data_Bar;
    [SerializeField]
    float dataIncreaseAmount = 10f;
    
    [Header("High Scores")]
    [SerializeField]
    private TextMeshProUGUI highScoreText;
    [SerializeField]
    private TextMeshProUGUI highScoreTimeText;
    [Header("Fastest Scores")]
    [SerializeField]
    private TextMeshProUGUI fastestScoreText;
    [SerializeField]
    private TextMeshProUGUI fastestTimeText;
    
    private float currentData = 0;
    private float targetData = 0;
    private int maxData = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentData = UIController.UpdateDelayValueUI(currentData, targetData, maxData, dataIncreaseAmount,scoreText, data_Bar);

    }

    public void SetActive(bool b)
    {
        gameObject.SetActive(b);
    }
    
    public void GameOver(float currentScore, float currentTime, float highScore_Score, float highScore_Time , float fastestTime_Score, float fastestTime_Time)
    {
        gameFinishScreen.SetActive(true);
        gameOverTitle.SetActive(true);
        gameWinTitle.SetActive(false);
        DisplayResults(currentScore, currentTime, highScore_Score, highScore_Time,fastestTime_Score,fastestTime_Time);
    }

    public void GameWin(float currentScore, float currentTime, float highScore_Score, float highScore_Time , float fastestTime_Score, float fastestTime_Time)
    {
        gameFinishScreen.SetActive(true);
        gameOverTitle.SetActive(false);
        gameWinTitle.SetActive(true);
        DisplayResults(currentScore, currentTime, highScore_Score, highScore_Time,fastestTime_Score,fastestTime_Time);
    }

    private void DisplayResults(float currentScore, float currentTime, float highScore_Score, float highScore_Time , float fastestTime_Score, float fastestTime_Time)
    {
        playTimeText.text = UIController.SecondsToString(currentTime);
        highScoreTimeText.text = UIController.SecondsToString(highScore_Time);
        // scoreText.text = currentScore.ToString("0");
        highScoreText.text = highScore_Score.ToString("0") + "GB";

        fastestTimeText.text = UIController.SecondsToString(fastestTime_Time);
        fastestScoreText.text = fastestTime_Score.ToString("0") + "GB";

        targetData = currentScore;
        maxData = GameManager.GM.MaxGb;
        currentData = UIController.UpdateDelayValueUI(0, targetData, maxData, dataIncreaseAmount, scoreText, data_Bar);
    }
}
