using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Animator menuAnimatior;

    [SerializeField]
    private GameObject beginningButton;
    [SerializeField]
    private GameObject startGameButton;
    [Space(10)]
    [Header("Reset Timer")]
    [SerializeField]
    private bool is_ResetTimer = true;
    [SerializeField]
    private float resetTimeSeconds = 240f;

    [SerializeField]
    private float resetTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        ResetTimer_Reset();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetTrigger("Start");

    }
    
    private void FixedUpdate()
    {
        if (is_ResetTimer&&Time.time - resetTime > resetTimeSeconds)
        {
            Debug.LogError("GAME TIMED OUT");
            QuitGame();
        }
    }

    public void DisplayTutorial()
    {
        animator.SetTrigger("End");
        ResetTimer_Reset();

    }

    public void CloseBriefing()
    {
        animator.SetTrigger("Begin");
        ResetTimer_Reset();

    }

    public void StartGame()
    {
        GameManager.PlayGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void IntroAnimation_Start()
    {
        menuAnimatior.SetBool("Start",true);
        menuAnimatior.SetBool("End",false);
        SetBeginningButton();
        ResetTimer_Reset();

    }

    private void SetBeginningButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(beginningButton);
        ResetTimer_Reset();

    }

    public void IntroAnimation_End()
    {
        menuAnimatior.SetBool("Start",false);

        menuAnimatior.SetBool("End",true);
        ResetTimer_Reset();


    }

    public void UpdateSelectStartButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(startGameButton);
        ResetTimer_Reset();


    }
    
    public void ResetTimer_Reset()
    {
        resetTime = Time.time;
    }

}
