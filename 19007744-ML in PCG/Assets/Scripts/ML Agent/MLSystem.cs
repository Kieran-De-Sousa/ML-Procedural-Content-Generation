// Base
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;

// Root namespace for all Machine Learning-related utilities.
namespace ML
{
    public class MLSystem : Singleton<MLSystem>
    {
        public MLAgent mlAgent;
    }
}