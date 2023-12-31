using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Saves list of all hacks in resources
/// Used to add hack based on hack type and or hack name
/// </summary>
public class HackManager : ScriptableObject
{
    [SerializeField]
    public static HackAbility[] AllHacks = new HackAbility[]{};
    
    const string hacksPath = "Hacks/";

    
    public static  HackAbility GetHack<T>()
    {
        if (AllHacks.Length == 0)
        {
            UpdateHacks();
        }


        // if (AllHacks.Length == 0)
        // {
        //     return null;
        // }

        for (var i = 0; i < AllHacks.Length; i++)
        {
            var hackAbility = AllHacks[i];
            if (typeof(T) == hackAbility.GetType())
            {
                return  AllHacks[i];
            }
        }

        Debug.LogError($"HackManager can't find hacks");

        throw new NullReferenceException();

    }
    public static HackAbility GetHack(string name)
    {
        if (AllHacks.Length == 0)
        {
            UpdateHacks();
        }

        foreach (HackAbility hackAbility in AllHacks)
        {
            if (hackAbility.HackName.Equals(name))
            {
                var temp = CreateInstance(hackAbility.GetType());
                return temp as HackAbility;
                
            }
        }

        return null;
    }

    private static void UpdateHacks()
    {
        AllHacks = Resources.LoadAll<HackAbility>(hacksPath).Cast<HackAbility>().ToArray();
    }


}
