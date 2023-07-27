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
        public Vector3 moveToPosition;
        public int health;
        public ItemName missingItem = ItemName.None;
        public ItemObject locatedItem;
        public ItemObject pickedUpItem;
        
        
        // public ItemName MissingItem => missingItems[0];
        // protected bool foundAllItems =>
    }
}