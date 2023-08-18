using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// UI game object to handle NPC alert state
/// </summary>
public class UI_Alert : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer susSprite;

    [SerializeField]
    private SpriteRenderer spottedSprite;

    [SerializeField]
    private Transform alertTransform;
    [Header("Sized")]
    [SerializeField]
    private Vector2 distanceRange = new Vector2(0,10);

    [SerializeField]
    private Vector2 scaleRange = new Vector2(.3f,10f);


    private Material alertMaterial;
    private static readonly int AlertStrength = Shader.PropertyToID("_AlertStrength");
    private Camera worldCamera;

    public bool isActive => gameObject.activeSelf;

    private void Awake()
    {
        if (susSprite)
        {
            alertMaterial = susSprite.material;
        }

        SetAlertStrength(0f);
        worldCamera = Camera.main;
        spottedSprite.gameObject.SetActive(false);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 transformForward = ((-worldCamera.transform.position)+alertTransform.position).normalized;
        alertTransform.forward = transformForward;
        
        Vector3 transformLocalScale = new Vector3(GetScale(),GetScale(),GetScale());
        alertTransform.localScale = transformLocalScale;
    }
    public void SetAlertPosition(Vector3 worldPosition)
    {
        // transform.position = world.WorldToScreenPoint(worldPosition);
        transform.position = worldPosition;
    }

    public void SetAlertStrength(float value)
    {
        if (value < 1f)
        {
            susSprite.gameObject.SetActive(true);
            spottedSprite.gameObject.SetActive(false);
            susSprite.material.SetFloat(AlertStrength, value);
        }
        else
        {
            susSprite.gameObject.SetActive(false);
            spottedSprite.gameObject.SetActive(true);
            susSprite.material.SetFloat(AlertStrength, 1f);

        }
    }


    public void SetAlert(Vector3 worldPosition, float value)
    {
        if (value > 0)
        {
            if (!isActive)
            {
                gameObject.SetActive(true);
            }

            SetAlertPosition(worldPosition);
            SetAlertStrength(value);
        }
        else
        {
            gameObject.SetActive(false);
            SetAlertStrength(0);
        }
    }

    float GetScale()
    {
        float distance = Vector3.Distance(worldCamera.transform.position, alertTransform.position);
        float factor = Math.Clamp(distance / distanceRange.y, 0f, 1f);
        return (scaleRange.y - scaleRange.x) * factor + scaleRange.x;
    }

}