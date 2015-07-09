using UnityEngine;
using Memoria.Battle;

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
            return((float)(int)AttackerParameters.elementAff.CheckAgainstElement(testElement) / 2);
        }
    }
}