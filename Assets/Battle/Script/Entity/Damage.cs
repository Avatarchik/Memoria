using UnityEngine;
using System.Collections;
using Memoria.Battle;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public class Damage : ScriptableObject {

        public Parameter AttackerParameters { get; set; }
        public Parameter TargetParameters { get; set; }
        public DmgParameters DamageParameters { get; set; }

        public int Calculate()
        {
            var totalDmg = 11.0f;
            totalDmg *= GetElementalBonus(TargetParameters.elementAff);

            return Mathf.CeilToInt(totalDmg);
        }

        public float GetElementalBonus(ElementType testElement)
        {
            Debug.Log(AttackerParameters.elementAff.CheckElements(testElement));
            return((float)(int)AttackerParameters.elementAff.CheckElements(testElement) / 2);
        }

        public void DamageEffect(IDamageable target)
        {
        }
    }
}