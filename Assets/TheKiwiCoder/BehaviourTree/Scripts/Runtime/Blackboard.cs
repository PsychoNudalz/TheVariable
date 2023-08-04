using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheKiwiCoder {

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard
    {
        public NPC_AlertState alertState = NPC_AlertState.Peace;
        public float health;
        public float healthThreshold = 0;

        public Vector3 moveToPosition;
        public ItemName missingItem = ItemName.None;
        public ItemObject locatedItem;
        public ItemObject pickedUpItem;
        [Space(5)]
        public bool flag_wait = false;
        public float wait_startTime = 0;
        public float wait_duration = 0;

        // public ItemName MissingItem => missingItems[0];
        // protected bool foundAllItems =>
    }
}