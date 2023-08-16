using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "Hacks/Manager")]
public class HackManager : ScriptableObject
{
    [SerializeField]
    public static HackAbility[] AllHacks = new HackAbility[]{};
    
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

    private static void UpdateHacks()
    {
        AllHacks = Resources.LoadAll<HackAbility>("Hacks/").Cast<HackAbility>().ToArray();
    }
}
