using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SOInfoDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject SONamePlate;
    [SerializeField]
    private TextMeshProUGUI SOName_Text;
[Space(5)]
[SerializeField]
// GameObject 
    private SmartObject currentSO;
    // Start is called before the first frame update
    void Start()
    {
        SetSO(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSO(SmartObject smartObject)
    {
        if (smartObject)
        {
            currentSO = smartObject;
            SONamePlate.SetActive(true);
            SOName_Text.text = currentSO.name;
        }
        else
        {
            currentSO = null;
            SONamePlate.SetActive(false);
        }
    }
}
