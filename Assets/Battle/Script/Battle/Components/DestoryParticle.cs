using UnityEngine;

namespace Memoria.Battle.Utility
{
    public class DestoryParticle : MonoBehaviour {
        ParticleSystem ps;

        void Start () {
            ps = GetComponent<ParticleSystem> ();
        }
    
        void Update () {
            if(!ps.IsAlive(true)) {
                Destroy(gameObject);
            }
        }
    }
}