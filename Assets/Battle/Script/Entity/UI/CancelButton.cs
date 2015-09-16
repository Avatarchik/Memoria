using UnityEngine;
using UnityEngine.UI;
using Memoria.Battle.Managers;
using Memoria.Battle.States;
using Memoria.Battle.Events;
using System.Collections;
using Memoria.Managers;

namespace Memoria.Battle.GameActors
{
    public class CancelButton : MonoBehaviour
    {
        public bool Visible { get; set; }
        Image img;
        void Start ()
        {
            img = GetComponent<Image>();
            Visible = false;
        }
        void Update ()
        {
            if(Visible)
            {
                img.enabled = true;
            }
            else
            {
                img.enabled = false;
            }
        }

        public void CancelSkill()
        {
            SoundManager.instance.PlaySound(35);
            Visible = false;
            EventMgr.Instance.Raise(new CancelSkill((Hero)BattleMgr.Instance.AttackTracker.currentActor));
        }
    }
}