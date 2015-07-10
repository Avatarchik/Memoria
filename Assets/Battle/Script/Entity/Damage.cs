using System;
using UnityEngine;
using Memoria.Battle;

namespace Memoria.Battle.GameActors
{
    public class Damage : ScriptableObject {

        public Parameter AttackerParameters { get; set; }
        public Parameter TargetParameters { get; set; }
        public DmgParameters DamageParameters { get; set; }
        public int totalDamage;

        public int Calculate()
        {
            var totalDmg = 12.0f;
            totalDmg *= GetElementalBonus(TargetParameters.elementAff);
            totalDamage = Mathf.CeilToInt(totalDmg);
            return Mathf.CeilToInt(totalDmg);
        }

        public float GetElementalBonus(ElementType testElement)
        {
            return((float)(int)AttackerParameters.elementAff.CheckAgainstElement(testElement) / 2);
        }

        public void Appear()
        {
            ActorSpawner spawner = GameObject.FindObjectOfType<ActorSpawner>();
            var dmg = totalDamage.ToArray();
            for(int i = 0; i < dmg.Length; i++)
            {
                //var spriteNr = Resources.Load<Sprite>("bt_"+ dmg[i]);

                var number = (spawner.Spawn<DamageNumber>("Numbers/damageNumber")).GetComponent<DamageNumber>();
                number.spriteResource = "Numbers/bt_" + dmg[i];
                number.ParentToUI();
                number.Init();
                number.transform.position = new Vector3(0 + (i * 0.3f), 0, 1);
                DestroyObject(number.gameObject, 0.4f);
            }
        }
 
    }
}