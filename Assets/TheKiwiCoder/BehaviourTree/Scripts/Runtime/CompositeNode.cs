using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TheKiwiCoder
{
    [System.Serializable]
    public abstract class CompositeNode : Node
    {
        [HideInInspector]
        [SerializeReference]
        public List<Node> children = new List<Node>();

        protected Node LeftNode => GetLeftNode();
        protected Node RightNode => GetRightNode();


        protected virtual Node GetLeftNode()
        {
            if (children.Count > 1)
            {
                return children[0];
            }
            else
            {
                throw new NullNodeException(this);
            }
        }

        protected virtual Node GetRightNode()
        {
            if (children.Count > 1)
            {
                return children[1];
            }
            else
            {
                throw new NullNodeException(this);
            }
        }
    }
}