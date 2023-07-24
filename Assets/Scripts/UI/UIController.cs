using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private UI_HackAbilityDisplay hackAbilityDisplay;
    // Start is called before the first frame update
    void Start()
    {
        Display_Hacks(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Display_Hacks(bool b, SmartObject so = null)
    {
        hackAbilityDisplay.gameObject.SetActive(b);
        hackAbilityDisplay.SetObjectToHack(so);
    }
}
