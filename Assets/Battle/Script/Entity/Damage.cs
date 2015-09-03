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

        // TODO: Finalize calcunations
        public int Calculate()
        {
            var totalDmg = (float)AttackerParameters.attack;
            Debug.Log("1: "+ totalDmg);
            totalDmg *= (float)DamageParameters.attackPower;
            Debug.Log("2: "+ totalDmg);
            if(AttackerParameters.blockBonus) totalDmg *= 2;
            Debug.Log("3: "+ totalDmg);
            totalDmg *= GetElementalBonus(TargetParameters.elementAff);
            Debug.Log("4: "+ totalDmg);
            totalDmg *= TryCritical(AttackerParameters.criticalHit);
            Debug.Log("5: "+ totalDmg);
            totalDmg -= TargetParameters.defense;
            Debug.Log("6: "+ totalDmg);
            totalDamage = Mathf.CeilToInt(totalDmg / 3);
            Debug.Log("7: "+ totalDmg);
            if(totalDamage < 0) { totalDamage = 0; }
            return Mathf.CeilToInt(totalDamage);
        }

        public float GetElementalBonus(ElementType testElement)
        {
            return((float)(int)AttackerParameters.elementAff.CheckAgainstElement(testElement) / 2);
        }

        public float TryCritical(float critChance)
        {
            var r = new System.Random();
            for(float i = 0; i < critChance; i += 0.01f)
            {
                if((r.Next(0, 100).Equals(100)))
                    return 2.0f;
            }
            return 1.0f;
        }

        public void Appear(Vector3 pos)
        {
            ActorSpawner spawner = GameObject.FindObjectOfType<ActorSpawner>();
            var dmg = totalDamage.ToArray();
            for(int i = 0; i < dmg.Length; i++)
            {
                var number = (spawner.Spawn<DamageNumber>("Numbers/damageNumber")).GetComponent<DamageNumber>();
                number.spriteResource = "Numbers/bt_" + dmg[i];
                number.ParentToUI();
                number.Init();
                number.transform.position = new Vector3(pos.x + (i * 0.4f), pos.y, 1);
                DestroyObject(number.gameObject, 1.0f);
            }
        }
    }
}