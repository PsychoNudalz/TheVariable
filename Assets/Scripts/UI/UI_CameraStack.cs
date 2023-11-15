using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CameraStack : MonoBehaviour
{
    [SerializeField]
    private Image[] spriteStack;

    [SerializeField]
    private RectTransform selectSprite;

    private Vector2 targetSelectPosition;

    [SerializeField]
    private float lerpSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(targetSelectPosition, selectSprite.position) > 1f)
        {
            selectSprite.position =
                Vector2.Lerp(selectSprite.position, targetSelectPosition, lerpSpeed * Time.deltaTime);
        }
    }

    public void UpdateStack(CameraController[] cameraControllers, int i)
    {
        for (var index = 0; index < spriteStack.Length; index++)
        {
            if (index < cameraControllers.Length)
            {
                spriteStack[index].gameObject.SetActive(true);
                spriteStack[index].sprite = UIController.current.GetSprite(cameraControllers[index].RoomLocation);
            }
            else
            {
                spriteStack[index].gameObject.SetActive(false);
            }
        }

        Invoke(nameof(DelayIndex),Time.fixedDeltaTime*2);
    }

    public void SetIndex(int i)
    {
        targetSelectPosition = spriteStack[i].rectTransform.position;
    }

    public void DelayIndex()
    {
        SetIndex(0);
    }
}