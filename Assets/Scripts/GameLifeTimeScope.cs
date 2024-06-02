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

    protected override void Configure(IContainerBuilder builder)
    {
        HandlePlayerDependencies(builder);
           
    }

    private void HandlePlayerDependencies(IContainerBuilder builder)
    {
        base.Configure(builder);
        builder.Register<PlayerGo>(Lifetime.Singleton).WithParameter(player).AsSelf();

        builder.Register<AnimationController>(Lifetime.Singleton).AsImplementedInterfaces();

        builder.Register<MovementController>(Lifetime.Singleton).AsImplementedInterfaces();
    }
}