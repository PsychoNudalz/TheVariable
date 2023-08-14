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

    private Material alertMaterial;
    private static readonly int AlertStrength = Shader.PropertyToID("_AlertStrength");
    private Camera world;

    public bool isActive => gameObject.activeSelf;

    private void Awake()
    {
        if (alertSprite)
        {
            alertMaterial = alertSprite.material;
        }

        SetAlertStrength(0f);
        world = Camera.main;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.forward = -(world.transform.forward);
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


}