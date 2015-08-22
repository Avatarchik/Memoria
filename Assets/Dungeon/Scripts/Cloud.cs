using UnityEngine;
using System.Collections;

namespace Memoria.Dungeon
{
    public class Cloud : MonoBehaviour
    {        
        private enum CloudPosition
        {
            Top,
            Bottom,
        }
        
        [SerializeField]
        private CloudPosition cloudPosition;
        
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}