using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ClearanceLevel : MonoBehaviour
{
    [SerializeField]
    private Image[] keySprites;

    [SerializeField]
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetClearanceText(int i)
    {
        for (int index = 0; index < keySprites.Length; index++)
        {
            if (index < i)
            {
                Color color = keySprites[index].color;
                color.a = 1f;
                keySprites[index].color = color;
            }
        }

        if (animator)
        {
            animator.SetTrigger("Play");
        }
    }
}
