using UnityEngine;

namespace Memoria.Battle.GameActors
{

    public class TargetSelector : MonoBehaviour {
        GameObject handlignObj;
        public Entity target;
        public bool MouseButtonHit { get; set; }
        public bool hitBoxCollider { get; set; }

        // Use this for initialization
        void Start () {
        }

        // Update is called once per frame
        void Update ()
        {
            hitBoxCollider = (TargetSelected(false, false));
        }
        public bool TargetSelected(bool enemy, bool setTarget = true)
        {
            if (Input.GetMouseButtonDown (0)) {
                MouseButtonHit = true;
                Vector3 tapPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
                Collider2D collition2d = Physics2D.OverlapPoint (tapPoint);
                if (collition2d) {
                    RaycastHit2D hitObject = Physics2D.Raycast (tapPoint, - Vector2.up);
                    if (hitObject) {
                        if(!setTarget)
                            return true;

                        if(enemy) {
                            target = (Entity)hitObject.collider.gameObject.GetComponent<Enemy>();
                        }
                        else {
                            target = GameObject.FindObjectOfType<MainPlayer>().GetComponent<Entity>();
                        }
                        return true;
                    }
                }
            }
            MouseButtonHit = false;
            return false;
        }
    }
}