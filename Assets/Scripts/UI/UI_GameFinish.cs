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
    private TextMeshProUGUI fastestText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private Image data_Bar;
    [SerializeField]
    float dataIncreaseAmount = 10f;
    [SerializeField]
    private TextMeshProUGUI highScoreText;
    
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
    
    public void GameOver(float currentTime, float fastestTime,float currentScore,float highScore)
    {
        gameFinishScreen.SetActive(true);
        gameOverTitle.SetActive(true);
        gameWinTitle.SetActive(false);
        DisplayResults(currentTime, fastestTime, currentScore, highScore);
    }

    public void GameWin(float currentTime, float fastestTime,float currentScore,float highScore)
    {
        gameFinishScreen.SetActive(true);
        gameOverTitle.SetActive(false);
        gameWinTitle.SetActive(true);
        DisplayResults(currentTime, fastestTime, currentScore, highScore);
    }

    private void DisplayResults(float currentTime, float fastestTime, float currentScore, float highScore)
    {
        playTimeText.text = UIController.SecondsToString(currentTime);
        fastestText.text = UIController.SecondsToString(fastestTime);
        // scoreText.text = currentScore.ToString("0");
        highScoreText.text = highScore.ToString("0") + "GB";

        targetData = currentScore;
        maxData = GameManager.GM.MaxGb;
        currentData = UIController.UpdateDelayValueUI(0, targetData, maxData, dataIncreaseAmount, scoreText, data_Bar);
    }
}
