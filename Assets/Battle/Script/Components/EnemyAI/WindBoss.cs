
namespace Memoria.Battle.GameActors
{
    public class WindBoss : EnemyAI
    {
        //風ボス
        void Awake ()
        {
            nameplate = "Namebar_WindBoss";
            nameplateId = 6;
            parameter.attack = 1044;
            parameter.defense = 169;
            parameter.speed = 298;
            parameter.hp = 5000;

            parameter.elementAff = new ElementWind(Element.WIND); 

            attackList.Add ("Enemy_Normal", gameObject.AddComponent<EnemyNormal>());
            attackList.Add ("Enemy_Skill", gameObject.AddComponent<BossSkill>());
            attackList.Add ("Enemy_Ultimate", gameObject.AddComponent<BossWind>());
        }
    }
}