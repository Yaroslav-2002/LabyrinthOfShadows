using Assets.Scripts.Generation;
using Entities.Player;
using Generation;
using LevelManagement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
{
    //TO DO: split into data classes
    [SerializeField] private MapConfiguration _mapConfiguration;
    [SerializeField] private GameObject player;
        
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        
        builder.Register<PlayerGo>(Lifetime.Singleton).WithParameter(player).AsSelf();

        builder.RegisterInstance(_mapConfiguration);

        builder.Register<WorldGenerationManager>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.RegisterComponentInHierarchy<LevelManager>();
    }
}