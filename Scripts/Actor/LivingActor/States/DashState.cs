namespace LivingActors
{
    public class DashingState : IState
    {
        public IState DoState(LivingActor actor)
        {
            // state values
            actor.state = "Dashing";
            actor.SetAnimState(4);

            return actor.DoDashingState();
        }
    }
}
