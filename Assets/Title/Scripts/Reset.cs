using UnityEngine;
using System.Collections;

namespace Memoria.Title
{
    public class Reset : MonoBehaviour
    {
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