namespace LivingActors
{
    public interface IState
    {
        virtual IState DoState(LivingActor actor) { return null; }
    }
}
