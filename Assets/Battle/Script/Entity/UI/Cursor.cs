using UnityEngine;

namespace Memoria.Battle.GameActors
{
    public class BattleCursor : UIElement
    {
        Animator anim;
        override public void Init()
        {
            anim = GetComponent<Animator>();
        }

        public void SelectAnimation()
        {
            anim.SetBool("select", true);
        }
    }
}