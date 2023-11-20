using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// New UI game object to handle NPC alert state
/// </summary>
public class UI_Alert : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private Image susSprite_Base;

    [SerializeField]
    private Image susSprite_Fill;

    [SerializeField]
    private Image susSprite_Red;

    [SerializeField]
    private Image spottedSprite;

    public bool isActive => gameObject.activeSelf;
    private Vector2 canvasSize = new Vector2(1280, 720);


    // Start is called before the first frame update
    void Start()
    {
        SetAlertStrength(0);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetCanvasSize(Vector2 size)
    {
        canvasSize = size;
    }

    public void SetAlertStrength(float value)
    {
        if (value <= 0.02f)
        {
            susSprite_Base.gameObject.SetActive(false);
            susSprite_Fill.gameObject.SetActive(false);
            susSprite_Red.gameObject.SetActive(false);
            spottedSprite.gameObject.SetActive(false);

            susSprite_Fill.fillAmount = 0;
            susSprite_Red.fillAmount = 0;
        }
        else if (value <= .5f)
        {
            susSprite_Base.gameObject.SetActive(true);
            susSprite_Fill.gameObject.SetActive(true);
            susSprite_Red.gameObject.SetActive(false);
            spottedSprite.gameObject.SetActive(false);

            susSprite_Fill.fillAmount = value / .5f;
            susSprite_Red.fillAmount = 0;
        }
        else if (value < 1f)
        {
            susSprite_Base.gameObject.SetActive(true);
            susSprite_Fill.gameObject.SetActive(true);
            susSprite_Red.gameObject.SetActive(true);
            spottedSprite.gameObject.SetActive(false);

            susSprite_Fill.fillAmount = 1;
            susSprite_Red.fillAmount = value % .5f / .5f;
        }
        else
        {
            susSprite_Base.gameObject.SetActive(false);
            susSprite_Fill.gameObject.SetActive(false);
            susSprite_Red.gameObject.SetActive(false);
            spottedSprite.gameObject.SetActive(true);
        }
    }

    public void SetAlert(Vector2 worldPosition, float value, int flip)
    {
        if (value > 0)
        {
            if (!isActive)
            {
                gameObject.SetActive(true);
            }

            SetAlertPosition(worldPosition, flip);
            SetAlertStrength(value);
        }
        else
        {
            SetAlertStrength(0);
            gameObject.SetActive(false);
        }
    }

    private void SetAlertPosition(Vector2 p, int flip)
    {
        rectTransform.position = p;

        // if (uiCanvasPosition.x < -canvasSize.x * .5f)
        // {
        //     uiCanvasPosition.x = -canvasSize.x * .5f;
        // }
        // else if (uiCanvasPosition.x > canvasSize.x * .5f)
        // {
        //     uiCanvasPosition.x = canvasSize.x * .5f;
        // }

        Vector2 rectTransformAnchoredPosition = rectTransform.anchoredPosition;

        if (flip == 1)
        {
            rectTransformAnchoredPosition.x = canvasSize.x * .5f;
        }else if (flip == -1)
        {
            rectTransformAnchoredPosition.x = -canvasSize.x * .5f;

        }
        else
        {
            rectTransformAnchoredPosition.x =
                Mathf.Clamp(rectTransformAnchoredPosition.x, -canvasSize.x * .5f, canvasSize.x * .5f);
        }

        rectTransformAnchoredPosition.y =
            Mathf.Clamp(rectTransformAnchoredPosition.y, -canvasSize.y * .5f, canvasSize.y * .5f);


        rectTransform.anchoredPosition = rectTransformAnchoredPosition;
    }
}