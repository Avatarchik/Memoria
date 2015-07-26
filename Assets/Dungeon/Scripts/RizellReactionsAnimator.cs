using UnityEngine;
using System.Collections;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon
{
    public class RizellReactionsAnimator : MonoBehaviour
    {
        public void ShowMessage(int visible)
        {
            EventManager.instance.ShowMessageBox(visible != 0);
        }
    }
}