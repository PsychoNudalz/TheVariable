using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    /// <summary>
    /// Finds the first successful child node, moved from failed nodes left to right 
    /// </summary>
    [System.Serializable]
    public class Selector : CompositeNode {
        protected int current;

        protected override void OnStart() {
            current = 0;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            for (int i = current; i < children.Count; ++i) {
                current = i;
                var child = children[current];

                switch (child.Update()) {
                    case State.Running:
                        return State.Running;
                    case State.Success:
                        return State.Success;
                    case State.Failure:
                        continue;

                }
            }

            return State.Failure;
        }
    }
}