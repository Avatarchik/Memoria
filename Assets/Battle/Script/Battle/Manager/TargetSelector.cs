using UnityEngine;

namespace Memoria.Battle.GameActors
{
    public class TargetSelector : MonoBehaviour {
    
        GameObject handlignObj;
        public Entity target;

        // Use this for initialization
        void Start () {
        }
    
        // Update is called once per frame
        void Update () {
        }
        public bool TargetSelected() { 
            if (Input.GetMouseButtonDown (0)) {
                Vector3 tapPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
                Collider2D collition2d = Physics2D.OverlapPoint (tapPoint);
                if (collition2d) {
                    RaycastHit2D hitObject = Physics2D.Raycast (tapPoint, - Vector2.up);
                    if (hitObject) {
                        target = (Entity)hitObject.collider.gameObject.GetComponent<Enemy>();
                        return true;
                    }
                }
            }
            return false;
        }
    }
}