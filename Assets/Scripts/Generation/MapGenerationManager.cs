using UnityEngine.Tilemaps;
using UnityEngine;
using Assets.Scripts.Generation;
using VContainer;

namespace Generation
{
    public class MapGenerationManager : IMapGenerationManager
    {
        [Inject] private readonly IMapGenerator _worldGenerator;

        private MapConfiguration _mapConfiguration;

        public MapGenerationManager(MapConfiguration mapConfiguration)
        {
            _mapConfiguration = mapConfiguration;
        }

        public void InitWorld()
        {
            _worldGenerator.Generate();
        }
    }
}