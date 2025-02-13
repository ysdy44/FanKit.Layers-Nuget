namespace FanKit.Layers.Sample
{
    public interface IUndoRedo
    {
        bool CanUndo();
        bool CanRedo();
        InvalidateModes TryUndo();
        InvalidateModes TryRedo();
        void Invalidate(InvalidateModes modes);
    }
}