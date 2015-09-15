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

        Animator anim;
        // Use this for initialization
        void Start () {
            _sr = GetComponent<SpriteRenderer>();
            _currentCar = Character.RIZEL;
            anim = GetComponent<Animator>();
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

        public void Switch()
        {
            anim.SetBool("switch", true);
        }

        public void SwitchCharacter()
        {
            anim.SetTrigger("switch");
            if(_currentCar == Character.RIZEL) {
                _currentCar++;
            }
            else {
                _currentCar--;
            }
        }


    }
}