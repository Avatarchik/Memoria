using UnityEngine;

namespace Memoria.Battle.GameActors
{

    abstract public class AttackType : MonoBehaviour {

        public struct SpriteData
        {
            public string skillId;
            public string descSprite;
            public string barSprite;

            public SpriteData(string id)
            {
                this.skillId = id;
                this.descSprite = "skill_info_" + skillId;
                this.barSprite = "skill_bar_" + skillId;
            }
        }
 
        public SpriteData spriteData;

        protected int animationDur;

        public ElementType elementalAff;
        public TargetType selectType;
        public DmgParameters parameters;

        public GameObject particleEffect;
        public GameObject effectObj;
        public GameObject normalEffect;

        public int phaseCost;
        public int stockCost;
        public int cutIn;
        public char targetType; // used to decide who gets selectable. e = enemy, h = self

        public bool attacked { get; set; }
        public bool ultimate;
        public string descriptionSprite;
        public int AttackTime {
            get {
                return animationDur;
            }
        }

        abstract public void Execute(Damage damage, IDamageable target);
        abstract public void PlayEffect(Entity target);
    }
}