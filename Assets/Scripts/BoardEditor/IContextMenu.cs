namespace BoardEditor
{
    public interface IContextMenu<in T>
    {
        void Enable(T eventController);
    }
}