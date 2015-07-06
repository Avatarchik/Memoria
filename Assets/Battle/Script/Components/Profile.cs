using UnityEngine;
using System.Collections.Generic;

namespace Memoria.Battle.GameActors
{
    abstract public class Profile : MonoBehaviour
    {
        public Parameter parameter;
        public AttackType attackType;
        public string ultimateAttack;
        public string nameplate { get; set; }
        public Dictionary<string, AttackType> attackList = new Dictionary<string, AttackType>();
    }
}