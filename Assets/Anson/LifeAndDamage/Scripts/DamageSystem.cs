using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum UnitFaction
{
    None,
    Faction_1,
    Faction_2
}

[AllowsNull]
public struct DamageData
{
    private Vector3 point;
    private float range;
    private float damage;

    public Vector3 Point => point;
    public float Damage => damage;

    public float Range => range;


    public DamageData(float damage, Vector3 point, float range)
    {
        this.point = point;
        this.range = range;
        this.damage = damage;
    }
}

/// <summary>
/// to control damaging player
/// </summary>
public class DamageSystem : MonoBehaviour
{
    [SerializeField]
    protected float damagePerSecond;

    [SerializeField]
    protected string[] damageTags;

    [SerializeField]
    protected LayerMask damageLayers;

    [SerializeField]
    protected bool needLineOfSight = false;

    [SerializeField]
    private LayerMask allLayers;

    [SerializeField]
    protected float LOSRange = 0;


    public bool isLOS(LifeSystem target)
    {
        // Vector3 dir = (target.transform.position - transform.position).normalized;
        // RaycastHit hit = Physics.Raycast(transform.position, dir, LOSRange, allLayers);
        // if (hit)
        // {
        //     if (hit.collider.TryGetComponent(out LifeSystem ls) && ls.Equals(target))
        //     {
        //         return true;
        //     }
        //
        //     return false;
        // }
        // else
        // {
        //     return true;
        // }
        return true;
    }

    public static bool isLOS(Vector3 target, Vector3 self, float LOSRange, LayerMask layerMask)
    {
        Vector3 dir = (target - self).normalized;
        Debug.DrawRay(self, dir*LOSRange, Color.blue, .02f);
        RaycastHit hit;
        if (Physics.Raycast(self, dir, out hit,LOSRange, layerMask))
        {
            if (hit.collider.TryGetComponent(out LifeSystem ls))
            {
                return true;
            }
        }

        return false;
    }    public static bool isLOS(LifeSystem target, Vector3 self, float LOSRange, LayerMask layerMask)
    {
        Vector3 dir = (target.Position - self).normalized;
        Debug.DrawRay(self, dir*LOSRange, Color.blue, .02f);
        RaycastHit hit;
        if (Physics.Raycast(self, dir, out hit,LOSRange, layerMask))
        {
            if (hit.collider.TryGetComponent(out LifeSystem ls)&&ls.Equals(target))
            {
                return true;
            }
        }

        return false;
    }

    public static void DealDamage(LifeSystem targetLs, float damage, bool friendlyFire, LifeSystem self = null)
    {
        targetLs.TakeDamage(damage, self);
    }

    public static void DealDamage(LifeSystem targetLs, DamageData damageData, bool friendlyFire,
        UnitFaction faction = UnitFaction.None, LifeSystem self = null)
    {
        if (!friendlyFire)
        {
            if (targetLs.IsHostile(faction))
            {
                targetLs.TakeDamage(damageData, self);
            }
        }
        else
        {
            // Debug.Log("Friendly state ignored");
            targetLs.TakeDamage(damageData, self);
        }

    }

    public static void SphereCastDamage(Vector3 position, float damage, float range, LayerMask layerMask,
        bool friendlyFire, bool singleDamage = false,
        UnitFaction faction = UnitFaction.None, LifeSystem self = null)
    {
        Collider[] colliders = Physics.OverlapSphere(position, range, layerMask);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out LifeSystem lifeSystem))
            {
                DealDamage(lifeSystem, new DamageData(damage, position, range), friendlyFire, faction, self);
                if (singleDamage)
                {
                    return;
                }
            }
        }
    }

    public static void SphereCastDamage(Vector3 position, float damage, float range, LayerMask layerMask,
        bool friendlyFire,
        AnimationCurve damageCurve, bool singleDamage = false, UnitFaction faction = UnitFaction.None,
        LifeSystem self = null)
    {
        Collider[] colliders = Physics.OverlapSphere(position, range, layerMask);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out LifeSystem lifeSystem))
            {
                float rangeScale = damageCurve.Evaluate((collider.transform.position - position).magnitude / range);
                DealDamage(lifeSystem, new DamageData(damage * rangeScale, position, range), friendlyFire, faction,
                    self);
                if (singleDamage)
                {
                    return;
                }
            }
        }
    }
}