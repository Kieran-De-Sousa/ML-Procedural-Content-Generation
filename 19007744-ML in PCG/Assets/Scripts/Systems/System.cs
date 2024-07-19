// Unity
using UnityEngine;

/// <summary>
/// Abstract base class for managing the three major systems of the simulation. Provides override method so all systems
/// can be reset through polymorphism.
/// </summary>
public abstract class ManagerSystem : MonoBehaviour
{
    /// <summary>
    /// Reset the current system.
    /// </summary>
    public abstract void ResetSystem();
}