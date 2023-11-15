using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Objective : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [Header("VIP")]
    [SerializeField]
    private GameObject vip_Alive;
    [SerializeField]
    private GameObject extraction;
    [SerializeField]
    private Image vip_Sprite;

    [SerializeField]
    private TextMeshProUGUI vip_Text_Alive;


    [Header("Data")]
    [SerializeField]
    private Image data_Bar;

    [SerializeField]
    private TextMeshProUGUI data_Text;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KilledVIP()
    {
        
    }
}
