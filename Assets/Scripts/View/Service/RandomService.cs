using Logic;
using UnityEngine;

namespace View
{
    public sealed class RandomService : IRandomService
    {
        public int Range(int minInclude, int maxExclude)
        {
            return Random.Range(minInclude, maxExclude);
        }

        public float Range(float minInclude, float maxInclude)
        {
            return Random.Range(minInclude, maxInclude);
        }

        public bool Boolean()
        {
            return Range(0, 2) == 1;
        }
    }
}