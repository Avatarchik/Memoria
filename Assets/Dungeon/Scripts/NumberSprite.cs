using UnityEngine;

namespace Memoria.Dungeon
{
    public class NumberSprite : MonoBehaviour
    {
        [SerializeField]
        private int _value = 0;

        public int value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
                int num = value;

                for (int i = 0; i < digits.Length; i++)
                {
                    digits[i].SetFloat("value", num % 10);
                    num /= 10;
                }
            }
        }

        [SerializeField]
        private Animator[] digits;

        //  // Use this for initialization
        //  void Start()
        //  {

        //  }

        //  // Update is called once per frame
        //  void Update()
        //  {

        //  }
    }
}