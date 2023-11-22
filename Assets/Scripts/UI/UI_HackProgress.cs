using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HackProgress : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField]
    private Image progressBar;

    [SerializeField]
    private TextMeshProUGUI timeLeftText;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Vector2 canvasSize = new Vector2(1280, 720);

    private Camera camera;

    private Coroutine delayCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetActive(bool b)
    {
        if (delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
        }

        if (b)
        {
            gameObject.SetActive(b);
            animator.Play("Active");
        }
        else
        {
            if (gameObject.activeSelf)
            {
                delayCoroutine = StartCoroutine(DelayDeactive());
                animator.Play("Deactive");
            }
        }
    }

    IEnumerator DelayDeactive()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }

    public void UpdateProgress(float timeLeft, float maxTime, Vector3 worldPosition)
    {
        transform.position = camera.WorldToScreenPoint(worldPosition);
        progressBar.fillAmount = timeLeft / maxTime;
        timeLeft = Mathf.Max(timeLeft, 0);
        timeLeftText.SetText($"{timeLeft:0.00}s");

        rectTransform.anchoredPosition =
            UIController.ClampAnchorPoint(worldPosition, rectTransform.anchoredPosition, canvasSize, camera);
    }

    public void SpeedUp()
    {
        // animator.Play("SpeedUp");
        animator.SetTrigger("SpeedUp");
    }
}