using UnityEngine;


namespace Memoria.Menu
{
    public class TowerPartyBox : MonoBehaviour
    {
        public int hp = 4500;
        public int sp = 30;
        public int silling;

        void Awake ()
        {
            if(GameData.hasPassiveItem1)
                sp += 3;
            if(GameData.hasPassiveItem2)
                hp += 500;

            var children = GetComponentsInChildren<PartyParameter>();
            silling = GameData.silling;

            children[0].value = hp;
            children[1].value = hp;
            children[2].value = sp;
            children[3].value = silling;
        }
    }
}