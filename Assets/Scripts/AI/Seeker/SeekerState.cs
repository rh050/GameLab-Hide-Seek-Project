public interface SeekerState
{
    void EnterState(SeekerAI seeker);
    void UpdateState(SeekerAI seeker);
    void ExitState(SeekerAI seeker);
}
