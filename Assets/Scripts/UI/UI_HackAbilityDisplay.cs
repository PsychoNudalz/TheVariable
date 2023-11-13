using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_HackAbilityDisplay : MonoBehaviour
{
    [SerializeField]
    private UI_HackAbilityButton[] buttons;

    [SerializeField]
    private RectTransform selectCursor;

    [SerializeField]
    private float cursorDistanceRatio = .1f;
    private float cursorDistance = 20;

    [SerializeField]
    private float cursorSpeed;

    [SerializeField]
    private Animator animator;

    private SmartObject currentSO;

    private UI_HackAbilityButton selectedButton;

    private Vector3 targetPosition;

    // Start is called before the first frame update
    private void Start()
    {
        for (var index = 0; index < buttons.Length; index++)
        {
            var uiHackAbilityButton = buttons[index];
            uiHackAbilityButton.SetDisplay(this,index);
        }

        targetPosition = transform.position;
        cursorDistance = cursorDistanceRatio * Screen.width;
    }

    private void Update()
    {
        UpdateCursor();

    }

    private void OnEnable()
    {
        
    }

    public void SetActive(bool b)
    {
        if (b)
        {
            CancelInvoke(nameof(DelayDisable));
            gameObject.SetActive(true);
            SoundManager.PlayGlobal(SoundGlobal.HackDisplay_On);
            animator.Play("UI_HackDisplay_On");
        }
        else
        {
            SoundManager.PlayGlobal(SoundGlobal.HackDisplay_Off);
            animator.Play("UI_HackDisplay_Off");

            Invoke(nameof(DelayDisable),1f);
        }
    }

    void DelayDisable()
    {
        gameObject.SetActive(false);
    }

    public void SetObjectToHack(SmartObject so)
    {
        currentSO = so;
        if (!currentSO)
        {
            return;
        }
        for (var index = 0; index < buttons.Length; index++)
        {
            var uiHackAbilityButton = buttons[index];
            if (index < currentSO.Hacks.Length)
            {
                uiHackAbilityButton.SetActive(true,currentSO.Hacks[index]);
            }
            else
            {
                uiHackAbilityButton.SetActive(false);
            }
        }
    }
    
    public HackAbility GetHack(int i)
    {
        return currentSO.Hacks[i];
    }

    public int UpdateDir(Vector2 dir)
    {
        if (selectedButton)
        {
            selectedButton.OnHover(false);
        }
        if (dir.magnitude > 1)
        {
            dir = dir.normalized;
        }

        Vector3 cursorDisplace = (new Vector3(dir.x, dir.y)) * cursorDistance;
        cursorDisplace = Vector3.ClampMagnitude(cursorDisplace, cursorDistance);
        targetPosition = transform.position + cursorDisplace;
        if (dir.magnitude < 0.2f)
        {
            return -1;
        }

        dir = dir.normalized;
        selectedButton = buttons[0];
        float dot = Vector2.Dot(selectedButton.GetDotDir(), dir);
        float dotTemp = -1;
        int bIndex = 0;
        for (var index = 0; index < buttons.Length; index++)
        {
            var button = buttons[index];
            if (!button.IsActive)
            {
                continue;
            }
            dotTemp = Vector2.Dot(button.GetDotDir(), dir);
            // if (dotTemp<0f)
            // {
            //     break;
            // }
            if (dotTemp > dot)
            {
                dot = dotTemp;
                selectedButton = button;
                bIndex = index;
            }
        }
        //If the dir is moving in the complete opposite way 
        if (dot < 0f)
        {
            return -1;
        }
        selectedButton.OnHover(true);

        return bIndex;
    }

    void UpdateCursor()
    {

        selectCursor.position = Vector3.Lerp(selectCursor.position,targetPosition,cursorSpeed*Time.deltaTime);
    }
}