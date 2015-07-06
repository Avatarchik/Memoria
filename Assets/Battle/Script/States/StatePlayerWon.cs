namespace Memoria.Battle.States
{
    public class StatePlayerWon : BattleState
    {
        override public void Initialize()
        {
            FadeAttackScreen.DeFlash();
        }
        override public void Update()
        {

        }
    }
}