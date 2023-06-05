namespace LivingActors
{
    public class WanderingState : IState
    {
        public IState DoState(LivingActor actor)
        {
            // state values
            actor.state = "Wandering";
            actor.SetAnimState(6);

            return actor.DoWanderingState();
        }
    }
}
