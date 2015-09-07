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
            var critBonus = TryCritical(AttackerParameters.criticalHit);

            Debug.Log("Attackers attack power: "+ totalDmg);


            Debug.Log("Skill bonus: "+ totalDmg +" * "+
                      (float)DamageParameters.attackPower +" = "+
                      (totalDmg *= (float)DamageParameters.attackPower));
            //totalDmg *= (float)DamageParameters.attackPower;


            if(AttackerParameters.blockBonus) {
                Debug.Log("Block bonus: "+ totalDmg +" * 2 = "+ (totalDmg *= 2));
                //totalDmg *= 2;
            }


            Debug.Log("Elemental Bonus: "+ totalDmg +" * "+
                      GetElementalBonus(TargetParameters.elementAff) +" = "+
                      (totalDmg *= GetElementalBonus(TargetParameters.elementAff)));
            //totalDmg *= GetElementalBonus(TargetParameters.elementAff);


            Debug.Log("Critical Hit Bonus: "+ totalDmg +" * "+
                      TryCritical(AttackerParameters.criticalHit) +" = "+
                      (totalDmg *= critBonus));
            //totalDmg *= critBonus;


            Debug.Log("Defense Check: "+ totalDmg +" - "+
                      TargetParameters.defense +" = "+
                      (totalDmg -= TargetParameters.defense));

            //totalDmg -= TargetParameters.defense;


            Debug.Log("Divide by 3: "+ totalDmg +" / 3 = "+
                      (totalDmg /= 3));
            //totalDmg /= 3;

            totalDamage = Mathf.CeilToInt(totalDmg);


            if(totalDamage < 0) { totalDamage = 0; }
            return totalDamage;
        }

        public float GetElementalBonus(ElementType testElement)
        {
            return((float)(int)AttackerParameters.elementAff.CheckAgainstElement(testElement) / 2);
        }

        public float TryCritical(float critChance)
        {
            var r = new System.Random();
            for(float i = 0; i < 100; i += 0.01f)
            {
                if((r.Next(0, 100).Equals(100)))
                    return 2.0f;
            }
            return 1.0f;
        }

        public void Appear(Vector3 pos, bool heal = false)
        {
            ActorSpawner spawner = GameObject.FindObjectOfType<ActorSpawner>();

            if(heal)
                totalDamage *= (-1);

            var dmg = totalDamage.ToArray();

            for(int i = 0; i < dmg.Length; i++)
            {
                var number = (spawner.Spawn<DamageNumber>("Numbers/damageNumber")).GetComponent<DamageNumber>();
                number.spriteResource = "Numbers/bt_" + dmg[i];
                number.ParentToUI();
                number.Init();
                if(heal)
                {
                    number.SetColor(Color.green);
                }
                number.transform.position = new Vector3(pos.x, pos.y, pos.z);
                var localPos = number.Position;
                number.transform.localPosition = new Vector3(localPos.x + (i * 60) - 60, localPos.y + (i * 30), 1);

                number.FallDown(localPos.y);
                DestroyObject(number.gameObject, 1.5f);

//                number.transform.localPosition = new Vector3(number.Position.x, number.Position.y, 1);
//                number.FoldOut((pos.x + (i * 60) - 60));
            }
        }
    }
}