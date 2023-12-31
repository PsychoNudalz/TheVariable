using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum LifeState
{
    Alive,
    Dead
}

public class LifeSystem : MonoBehaviour
{
    [SerializeField]
    private LifeState lifeState = LifeState.Alive;

    [SerializeField]
    private  float health_max = 100f;

    [SerializeField]
    private float health;

    [SerializeField]
    private AnimationCurve healthToDisplay;

    [Header("Events")]
    [SerializeField]
    private UnityEvent onDamageEvent;

    [SerializeField]
    protected UnityEvent onDeathEvent;


    [Header("Components")]
    [SerializeField]
    private Renderer healthSpriteRenderer;

    private Material healthMaterial;

    private UnitFaction faction;

    public Vector3 Position => transform.position;

    public float Health => health;


    public bool IsFriendly(UnitFaction other)
    {
        return other.Equals(faction);
    }

    public bool IsHostile(UnitFaction other)
    {
        if (other == UnitFaction.None)
        {
            return true;
        }
        return !other.Equals(faction);
    }
    private void Start()
    {
        if (healthSpriteRenderer)
        {
            healthMaterial = healthSpriteRenderer.material;
        }

        health = health_max;
        UpdateHealthShader(health);
    }

    public virtual void SwitchState(LifeState ls)
    {
        LifeState previousState = lifeState;
        lifeState = ls;

        if (ls == LifeState.Dead && previousState != ls)
        {
            OnDeath();
        }

        // print("Player: " + this + " " + previousState + " --> " + lifeState);
    }


    protected virtual void OnDeath()
    {
        onDeathEvent.Invoke();
    }

    public virtual bool TakeDamage(float damage, LifeSystem source = null)
    {
        health -= damage;
        onDamageEvent.Invoke();
        if (health <= 0)
        {
            SwitchState(LifeState.Dead);
            health = 0;
        }

        UpdateHealthShader(health);

        return IsDead();
    }

    public virtual bool TakeDamage(DamageData damageData, LifeSystem source = null)
    {
        health -= damageData.Damage;
        onDamageEvent.Invoke();
        if (health <= 0)
        {
            SwitchState(LifeState.Dead);
            health = 0;
        }

        UpdateHealthShader(health);

        return IsDead();
    }

    public virtual bool IsDead()
    {
        return lifeState == LifeState.Dead;
    }    public virtual bool IsAlive()
    {
        return lifeState == LifeState.Alive;
    }


    [ContextMenu("UpdateHealth")]
    public virtual void UpdateHealthShader(float h)
    {
        if (healthMaterial)
        {
            healthMaterial.SetFloat("_CircleStrength", healthToDisplay.Evaluate(h / health_max));
        }
    }
}