namespace Memoria.Battle.GameActors
{
    public class Dhiel : Profile
    {
        //現ディエル
        void Awake ()
        {
            nameplate = "Namebar_Dhiel";
            nameplateId = 1;
            ultimateAttack = "Diel_SP";
            parameter.attack = 688;
            parameter.defense = 139;
            parameter.speed = 303;
            parameter.hp = 1058;
            parameter.criticalHit = 0.12f;
            parameter.elementAff = new ElementWind(Element.WIND);

            attackList.Add("Attack_Normal", gameObject.AddComponent<DielNormal>());
            attackList.Add("Attack_Special", gameObject.AddComponent<DielElement>());
            attackList.Add("Diel_SP", gameObject.AddComponent<DielUltimate>());
        }
    }
}