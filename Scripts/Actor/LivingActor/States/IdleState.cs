namespace LivingActors
{
    public class IdleState : IState
    {
        public IState DoState(LivingActor actor)
        {
            // state values
            actor.state = "Idle";
            actor.SetAnimState(0);

            return actor.DoIdleState();
        }
    }
}
