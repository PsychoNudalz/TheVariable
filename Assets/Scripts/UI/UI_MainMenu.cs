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
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetTrigger("Start");

    }

    public void DisplayTutorial()
    {
        animator.SetTrigger("End");

    }

    public void CloseBriefing()
    {
        animator.SetTrigger("Begin");

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
    }

    private void SetBeginningButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(beginningButton);
    }

    public void IntroAnimation_End()
    {
        menuAnimatior.SetBool("Start",false);

        menuAnimatior.SetBool("End",true);

    }

    public void UpdateSelectStartButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(startGameButton);

    }
}
