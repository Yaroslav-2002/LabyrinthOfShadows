using System;
using Entities;
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
    //TO DO: split into data classes
    [SerializeField] private GenerationAlgorithmType genAlgType;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Tilemap collisionTileMap;
    [SerializeField] private Tile wallTile;
    [SerializeField] private Tile pathTile;
    [SerializeField] private Tile collisionWallTile; 
    [SerializeField] private int mazeSize;
    [SerializeField] private int cellSize;
    [SerializeField] private int mainRoomHeight;
    [SerializeField] private GameObject player;
        
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        
        builder.Register<PlayerGo>(Lifetime.Singleton).WithParameter(player).AsSelf();
        
         switch (genAlgType)
         {
             case GenerationAlgorithmType.Eller:
                 builder.Register(container => new ProceduralWorldGenerator(container.Resolve<PlayerGo>(), new EllerMazeAlgorithm(mazeSize),
                     tileMap, collisionTileMap, wallTile, pathTile, 
                     collisionWallTile, mazeSize, cellSize, mainRoomHeight), Lifetime.Singleton).As<ITickable, WorldGeneratorBase>();
                 break;
             case GenerationAlgorithmType.DepthSearch:
                 builder.Register(_ => new StaticWorldGenerator(new DepthSearchAlgorithm(mazeSize, mazeSize),
                     tileMap, collisionTileMap, wallTile, pathTile, 
                     collisionWallTile, mazeSize, cellSize, mainRoomHeight), Lifetime.Singleton).As<WorldGeneratorBase>();;
                 break;
             default:
                 throw new AggregateException("Unsupported generation algorithm type.");
         }
        
        builder.Register<WorldGenerationManager>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.RegisterComponentInHierarchy<LevelManager>();
    }
}

public enum GenerationAlgorithmType
{
    Eller,
    DepthSearch
}