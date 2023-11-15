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
        StartCoroutine(VIPToExtract());
    }

    IEnumerator VIPToExtract()
    {
        animator.SetTrigger("Play");
        yield return new WaitForSeconds(.1f);
        vip_Alive.SetActive(false);
        extraction.SetActive(true);
    }
}
