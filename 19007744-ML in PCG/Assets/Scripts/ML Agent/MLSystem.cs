// Root namespace for all Machine Learning-related utilities.
namespace ML
{
    /// <summary>
    /// Singleton that overviews all ML systems present in each room simulation.
    /// </summary>
    public class MLSystem : ManagerSystem
    {
        public MLAgent mlAgent;

        /// <summary>
        /// Reset the system by ending the agents current episode.
        /// </summary>
        public override void ResetSystem()
        {
            mlAgent.EndEpisode();
        }
    }
}