using System;
using UnityEngine;

namespace Task
{
    public class TOC_Oven: TaskObjectController
    {
        [Header("Oven")]
        [Header("Interrupt Damage")]
        [SerializeField]
        private DamageZone damageZone;

        private void Start()
        {
            damageZone.gameObject.SetActive(false);
        }

        public override void OnInterruptTask()
        {
            base.OnInterruptTask();
            damageZone.gameObject.SetActive(true);

        }
    }
}