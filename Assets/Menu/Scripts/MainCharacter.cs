using UnityEngine;
using System.Collections;

namespace Memoria.Menu
{
    public enum Character
    {
        RIZEL = 0,
        DIANA = 1
    }
    public class MainCharacter : MonoBehaviour
    {
        private SpriteRenderer _sr;

        [SerializeField]
        private Sprite[] _sprites;

        [SerializeField]
        private Character _currentCar;
        
        // Use this for initialization
        void Start () {
            _sr = GetComponent<SpriteRenderer>();
            _currentCar = Character.RIZEL;
            StartCoroutine(Switch(5));
        }
	
        // Update is called once per frame
        void Update () {
            switch(_currentCar)
            {
                case Character.RIZEL:
                    _sr.sprite = _sprites[(int)Character.RIZEL];
                    break;
                case Character.DIANA:
                    _sr.sprite = _sprites[(int)Character.DIANA];
                    break;
            }
        }

        public void SwitchCharacter()
        {
//            StartCoroutine(Switch((int)_currentCar));
        }

        private IEnumerator Switch(int i)
        {
            Vector3 nowPos = this.transform.position;
            Vector3 newPos = nowPos;
            float time = 0;
            float ends = 1;
            int cnt = 0;
            while(time < ends)
            {
//                newPos += Mathf.Cos(Mathf.PI * time);

                this.transform.position = newPos;

                time += Time.deltaTime;

                yield return null;
            }

        }
    }
}