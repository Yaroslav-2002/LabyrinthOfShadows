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
    [SerializeField] private Tilemap Tilemap;
    [SerializeField] private Tilemap CollisionTilemap;


    protected override void Configure(IContainerBuilder builder)
    {
        var roomsize = _mapConfiguration.roomHeight;

        HandlePlayerDependencies(builder);

        builder.Register(context =>
        {
            return new WorldGenerator(_mapConfiguration, Tilemap, CollisionTilemap, new EllerMazeAlgorithm(roomsize / 2));
        }, Lifetime.Singleton).As<IWorldGenerator>();

        builder.Register<WorldGenerationManager>(Lifetime.Singleton).WithParameter(_mapConfiguration).AsImplementedInterfaces();
           
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