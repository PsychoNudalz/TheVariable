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
    private GameObject startingButton;
    [SerializeField]
    private GameObject startGameButton;
    
    // Start is called before the first frame update
    void Start()
    {
        animator.SetTrigger("Start");
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayTutorial()
    {
        animator.SetTrigger("End");

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
        EventSystem.current.SetSelectedGameObject(startingButton);
    }

    public void IntroAnimation_End()
    {
        menuAnimatior.SetBool("Start",false);

        menuAnimatior.SetBool("End",true);

    }
}
