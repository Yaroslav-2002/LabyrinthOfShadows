using Assets.Scripts.Generation;
using Controls;
using Entities.Player;
using Generation;
using Generation.Algorithms;
using LevelManagement;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
{
    [SerializeField] private MapConfiguration _mapConfiguration;
    [SerializeField] private GameObject player;
    [SerializeField] private Grid grid;


    protected override void Configure(IContainerBuilder builder)
    {
        HandlePlayerDependencies(builder);

        builder.Register(context =>
        {
            return new MapGenerator(_mapConfiguration, new EllerMazeAlgorithm(_mapConfiguration.numWidthNodes), grid);
        }, Lifetime.Singleton).As<IMapGenerator>();

        builder.Register<MapGenerationManager>(Lifetime.Singleton).WithParameter(_mapConfiguration).AsImplementedInterfaces();
           
        builder.RegisterComponentInHierarchy<LevelManager>();
    }

    private void HandlePlayerDependencies(IContainerBuilder builder)
    {
        base.Configure(builder);
        builder.Register<PlayerGo>(Lifetime.Singleton).WithParameter(player).AsSelf();

        builder.Register<AnimationController>(Lifetime.Singleton).AsImplementedInterfaces();

        builder.Register<MovementController>(Lifetime.Singleton).AsImplementedInterfaces();
    }
}