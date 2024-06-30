// Procedural Content Generation
namespace PCG.Tilemaps
{
    /// <summary>
    /// Interface for collidable tiles.
    /// </summary>
    public interface ICollidable
    {
        /// <summary>
        /// Is the tile collidable.
        /// </summary>
        public bool IsCollidable { get; }
    }

    /// <summary>
    /// Interface for interactable tiles.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Is the tile interactable.
        /// </summary>
        public bool IsInteractable { get; }
        /// <summary>
        /// Has the tile been interacted with.
        /// </summary>
        public bool IsInteracted { get; }

        /// <summary>
        /// Abstract overridable method implemented by tiles that have "Triggers".
        /// </summary>
        public abstract void Interact();
    }
}