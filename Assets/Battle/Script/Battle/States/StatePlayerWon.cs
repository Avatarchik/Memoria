namespace Memoria.Battle.States
{
    public class StatePlayerWon : BattleState
    {
        override public void Initialize()
        {
            //        Debug.Log("Player won");
            FadeAttackScreen.DeFlash();
        }
        override public void Update()
        {

        }
    }
}