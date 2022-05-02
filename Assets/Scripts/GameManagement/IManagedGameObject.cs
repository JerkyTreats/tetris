namespace GameManagement
{
    public interface IManagedGameObject
    {
        void Activate();
        void Deactivate();
        void Terminate();
        // void Interrupt();
    }
}