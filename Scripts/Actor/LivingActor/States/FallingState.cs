namespace LivingActors
{
    public class FallingState : IState
    {
        public IState DoState(LivingActor actor)
        {
            // state values
            actor.state = "Falling";
            actor.SetAnimState(3);

            return actor.DoFallingState();
        }
    }
}
