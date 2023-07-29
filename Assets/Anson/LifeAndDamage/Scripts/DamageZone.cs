using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField]
    private float baseDamage;

    [SerializeField]
    private bool friendlyFire = false;

    [SerializeField]
    private LayerMask damageLayerMask;

    [SerializeField]
    private float duration = 1f;

    [SerializeField]
    private float range = 1f;

    
    [SerializeField]
    [Tooltip("1: Close.  0: Far")]
    private AnimationCurve damageCurve;

    [SerializeField]
    private Collider damageTrigger;

    [SerializeField]
    private Transform zoneDisplay;

    private List<LifeSystem> inTriggerUnit = new List<LifeSystem>();

    private void Awake()
    {
        if (damageTrigger)
        {
            if (damageTrigger is SphereCollider sc)
            {
                sc.radius = range;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(.5f, .5f, .5f, .5f);
        Gizmos.DrawSphere(transform.position, range);
    }

    private void FixedUpdate()
    {
        if (inTriggerUnit.Count > 0)
        {
            float damageThisFrame = baseDamage* Time.fixedDeltaTime;
            foreach (LifeSystem lifeSystem in inTriggerUnit)
            {
                DamageSystem.DealDamage(lifeSystem,
                    new DamageData(damageThisFrame*SampleRangeMultiplier(lifeSystem.Position), transform.position,
                        range), friendlyFire);
            }
        }
    }

    float SampleRangeMultiplier(Vector3 target)
    {
        return damageCurve.Evaluate(1f - (Vector3.Distance(target, transform.position) / range));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out LifeSystem ls))
        {
            if (!inTriggerUnit.Contains(ls))
            {
                inTriggerUnit.Add(ls);
            }
        }

        LifeSystem[] temp = inTriggerUnit.ToArray();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] == null)
            {
                inTriggerUnit.Remove(temp[i]);
            }
        }

        inTriggerUnit = new List<LifeSystem>(temp);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out LifeSystem ls))
        {
            if (inTriggerUnit.Contains(ls))
            {
                inTriggerUnit.Remove(ls);
            }
        }
    }
}