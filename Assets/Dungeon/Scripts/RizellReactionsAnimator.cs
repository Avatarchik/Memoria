using UnityEngine;
using System.Collections;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon
{
    public class RizellReactionsAnimator : MonoBehaviour
    {
        //  
        //          // Use this for initialization
        //          void Start()
        //          {
        //  
        //          }
        //  
        //          // Update is called once per frame
        //          void Update()
        //          {
        //  
        //          }

        public void ShowMessage(int visible)
        {
            EventManager.instance.ShowMessageBox(visible != 0);
        }
    }
}