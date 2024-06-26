// Procedural Content Generation
namespace PCG.Tilemaps
{
    public interface ICollidable
    {
        public bool IsCollidable { get; }
    }

    public interface IInteractable
    {
        public bool IsInteractable { get; }
        public bool IsInteracted { get; }

        /// <summary>
        /// Virtual overridable method inherited by tiles that have "Triggers".
        /// </summary>
        public abstract void Interact();
    }
}