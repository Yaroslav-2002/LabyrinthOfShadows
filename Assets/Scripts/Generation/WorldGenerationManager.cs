using UnityEngine.Tilemaps;
using UnityEngine;
using Assets.Scripts.Generation;
using VContainer;

namespace Generation
{
    public class WorldGenerationManager : IWorldGenerationManager
    {
        [Inject] private readonly IWorldGenerator _worldGenerator;

        private MapConfiguration _mapConfiguration;

        public WorldGenerationManager(MapConfiguration mapConfiguration)
        {
            _mapConfiguration = mapConfiguration;
        }

        public void InitWorld()
        {
            _worldGenerator.Generate();
        }
    }
}