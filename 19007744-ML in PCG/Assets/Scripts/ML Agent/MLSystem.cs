// Root namespace for all Machine Learning-related utilities.

using UnityEngine;

namespace ML
{
    /// <summary>
    /// Singleton that overviews all ML systems present in each room simulation.
    /// </summary>
    public class MLSystem : ManagerSystem
    {
        public MLAgent mlAgent;

        public override void ResetSystem()
        {
            mlAgent.EndEpisode();
        }
    }
}