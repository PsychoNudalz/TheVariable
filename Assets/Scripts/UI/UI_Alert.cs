using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// New UI game object to handle NPC alert state
/// </summary>
public class UI_Alert : MonoBehaviour
{
    
    [SerializeField]
    private SpriteRenderer susSprite_Base;

    [SerializeField]
    private SpriteRenderer susSprite_Fill;

    [SerializeField]
    private SpriteRenderer susSprite_Red;

    [SerializeField]
    private SpriteRenderer spottedSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void SetAlertStrength(float value)
    {
        if (value <= 0.02f)
        {
            susSprite_Base.gameObject.SetActive(false);
            susSprite_Fill.gameObject.SetActive(false);
            susSprite_Red.gameObject.SetActive(false);
            spottedSprite.gameObject.SetActive(false);
        }
        else if (value < 1f)
        {
            susSprite_Base.gameObject.SetActive(true);
            susSprite_Fill.gameObject.SetActive(true);
            susSprite_Red.gameObject.SetActive(true);
            spottedSprite.gameObject.SetActive(false);
        }
        else
        {

            susSprite_Base.gameObject.SetActive(false);
            susSprite_Fill.gameObject.SetActive(false);
            susSprite_Red.gameObject.SetActive(false);
            spottedSprite.gameObject.SetActive(true);
        }
    }
}
