using UnityEngine;
using System.Collections;

namespace Memoria.Battle.GameActors
{
    public class Golem : EnemyAI {

        // Use this for initialization
        void Awake ()
        {
            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
            nameplate = "Namebar_Golem";
        }

        void Update()
        {
        }
    }
}