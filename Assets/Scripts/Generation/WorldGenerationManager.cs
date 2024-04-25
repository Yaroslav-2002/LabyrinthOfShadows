using UnityEngine.Tilemaps;
using UnityEngine;
using Assets.Scripts.Generation;

namespace Generation
{
    public class WorldGenerationManager : IWorldGenerationManager
    {
        private readonly WorldGeneratorBase _worldGenerator;
        public WorldGenerationManager(MapConfiguration mapConfiguration, WorldGeneratorBase worldGenerator)
        {
            _worldGenerator = worldGenerator;
        }

        public void InitWorld()
        {
            _worldGenerator.Generate();
        }
    }

    public interface IWorldGenerationManager
    {
        void InitWorld();
    }
}