using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public class BottomParticles : MonoBehaviour {

        private bool _initialized;

        void Init ()
        {

            var type = BattleMgr.Instance.elementalAffinity;
            switch(type)
            {
                case Element.FIRE:
                    SetColor(Color.red);
                    break;
                case Element.THUNDER:
                    SetColor(Color.yellow);
                    break;
                case Element.WATER:
                    SetColor(Color.blue);
                    break;
                case Element.WIND:
                    SetColor(Color.green);
                    break;
            }
            _initialized = true;
        }

        void Update()
        {
            if(!_initialized)
            {
                Init();
            }
        }

        private void SetColor(Color color)
        {
            ParticleSystem[] _particles;
            _particles = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < _particles.Length; i++)
            {
                _particles[i].startColor = color;
            }
        }
    }
}