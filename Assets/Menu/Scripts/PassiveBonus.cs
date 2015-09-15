using UnityEngine;

namespace Memoria.Menu
{
    public class PassiveBonus : MonoBehaviour
    {
        public string item;

        bool hasItem;

        void Start ()
        {
            switch(item)
            {
                case "passive_00":
                    hasItem = GameData.hasPassiveItem1;
                    break;
                case "passive_01":
                    hasItem = GameData.hasPassiveItem2;
                    break;
            }

            if(hasItem)
            {
                this.GetComponent<UnityEngine.UI.Image>().enabled = true;
            } else {
                this.GetComponent<UnityEngine.UI.Image>().enabled = false;
            }
        }
    }
}