namespace LivingActors
{
    public class DyingState : IState
    {
        public IState DoState(LivingActor actor)
        {
            // state values
            actor.state = "Dying";
            actor.SetAnimState(5);

            return actor.DoDyingState();
        }
    }
}

