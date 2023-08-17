using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI game object to handle NPC alert state
/// </summary>
public class UI_Alert : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer alertSprite;

    [Header("Sized")]
    [SerializeField]
    private Vector2 distanceRange = new Vector2(0,10);

    [SerializeField]
    private Vector2 scaleRange = new Vector2(.3f,10f);

    private Transform alertTransform;

    private Material alertMaterial;
    private static readonly int AlertStrength = Shader.PropertyToID("_AlertStrength");
    private Camera worldCamera;

    public bool isActive => gameObject.activeSelf;

    private void Awake()
    {
        if (alertSprite)
        {
            alertMaterial = alertSprite.material;
        }

        SetAlertStrength(0f);
        worldCamera = Camera.main;
        alertTransform = alertSprite.transform;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        alertTransform.forward = ((worldCamera.transform.position) - alertTransform.position).normalized;
        alertTransform.localScale = new Vector3(GetScale(),GetScale(),GetScale());
    }
    public void SetAlertPosition(Vector3 worldPosition)
    {
        // transform.position = world.WorldToScreenPoint(worldPosition);
        transform.position = worldPosition;
    }

    public void SetAlertStrength(float value)
    {
        alertSprite.material.SetFloat(AlertStrength, value);
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