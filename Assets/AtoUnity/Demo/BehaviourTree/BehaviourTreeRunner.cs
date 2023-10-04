using UnityEngine;

namespace AtoGame.TheKiwiCoder.BT.Demo
{
    public class BehaviourTreeRunner : MonoBehaviour {

        // The main behaviour tree asset
        public BehaviourTree tree;

        // Storage container object to hold game object subsystems
        BasicRoamingContext context;

        // Start is called before the first frame update
        void Start() {
            context = CreateBehaviourTreeContext();
            tree = tree.Clone();
            tree.Bind(context);
        }

        // Update is called once per frame
        void Update() {
            if (tree) {
                tree.Update();
            }
        }

        BasicRoamingContext CreateBehaviourTreeContext() {
            return BasicRoamingContext.CreateFromGameObject(gameObject);
        }

        private void OnDrawGizmosSelected() {
            if (!tree) {
                return;
            }

            BehaviourTree.Traverse(tree.rootNode, (n) => {
                if (n.drawGizmos) {
                    n.OnDrawGizmos();
                }
            });
        }
    }
}