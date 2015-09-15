using UnityEngine;
using System.Collections;

namespace Memoria.Title
{
    public class Reset : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
        
        public void ResetData()
        {
            GameData.Reset();
        }
    }
}