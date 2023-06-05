namespace LivingActors 
{
    public class WalkingState : IState
    {
        public IState DoState(LivingActor actor)
        {
            // state values
            actor.state = "Walking";
            actor.SetAnimState(1);

            return actor.DoWalkingState();
        }
    }
}
