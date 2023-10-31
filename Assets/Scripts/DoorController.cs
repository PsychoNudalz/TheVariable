using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private List<string> tagList = new List<string>();

    [SerializeField]
    private List<Collider> enteredEntities;

    private void Awake()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tagList.Contains(other.tag))
        {
            animator.SetBool("Open", true);
            enteredEntities.Add(other);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (tagList.Contains(other.tag))
        {
            enteredEntities.Remove(other);
            if (enteredEntities.Count == 0)
            {
                animator.SetBool("Open", false);

            }
        }
    }
}
