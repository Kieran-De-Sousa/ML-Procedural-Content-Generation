// Root namespace for all Machine Learning-related utilities.
namespace ML
{
    /// <summary>
    /// Singleton that overviews all ML systems present in each room simulation.
    /// </summary>
    public class MLSystem : Singleton<MLSystem>
    {
        public MLAgent mlAgent;
    }
}