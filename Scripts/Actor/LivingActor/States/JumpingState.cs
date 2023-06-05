namespace LivingActors
{
    public class JumpingState : IState
    {
        public IState DoState(LivingActor actor)
        {
            // state values
            actor.state = "Jumping";
            actor.SetAnimState(2);

            return actor.DoJumpingState();
        }
    }
}
