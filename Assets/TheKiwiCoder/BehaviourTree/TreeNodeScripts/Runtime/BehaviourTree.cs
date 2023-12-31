using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace TheKiwiCoder {
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject {

        [SerializeReference]
        public RootNode rootNode;

        [SerializeReference]
        public List<Node> nodes = new List<Node>();

        public Node.State treeState = Node.State.Running;

        public Blackboard blackboard = new Blackboard();

#region  EditorProperties 
        public Vector3 viewPosition = new Vector3(600, 300);
        public Vector3 viewScale = Vector3.one;
#endregion

        public BehaviourTree() {
            rootNode = new RootNode();
            nodes.Add(rootNode);
        }

        public Node.State Update() {
            if (rootNode.state == Node.State.Running) {
                treeState = rootNode.Update();
            }
            return treeState;
        }

        public static List<Node> GetChildren(Node parent) {
            List<Node> children = new List<Node>();

            if (parent is DecoratorNode decorator && decorator.child != null) {
                children.Add(decorator.child);
            }

            if (parent is RootNode rootNode && rootNode.child != null) {
                children.Add(rootNode.child);
            }

            if (parent is CompositeNode composite) {
                return composite.children;
            }

            return children;
        }

        public static void Traverse(Node node, System.Action<Node> visiter) {
            if (node != null) {
                visiter.Invoke(node);
                var children = GetChildren(node);
                children.ForEach((n) => Traverse(n, visiter));
            }
        }

        public BehaviourTree Clone() {
            BehaviourTree tree = Instantiate(this);
            return tree;
        }

        public void Bind(Context context) {
            Traverse(rootNode, node => {
                node.context = context;
                blackboard.npcController = context.NpcController;
                node.blackboard = blackboard;
            });
        }

        
        public void CleanTree()
        {
            int errorCount = 0;
            List<Node> allNodesList = new List<Node>(nodes);
            
            //TODO:Need to do tree traversal to find all connecting nodes
            //
            // foreach (Node node in nodes)
            // {
            //     foreach (Node n in GetChildren(node))
            //     {
            //         if (allNodesList.Contains(n))
            //         {
            //             allNodesList.Remove(node);
            //         }
            //     }
            //
            // }

            errorCount = allNodesList.Count;
            Debug.LogError($"Tree Clean Results: {errorCount} nodes without parents");
        }
    }
}