using Entities.Player;
using Generation.Algorithms;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;

namespace Generation
 {
     public class ProceduralWorldGenerator : WorldGeneratorBase, ITickable
     {
         private readonly PlayerGo _player;
         public ProceduralWorldGenerator(PlayerGo player, IGenerationAlgorithm algorithm, Tilemap tileMap, Tilemap collisionTileMap, Tile wallTile, Tile pathTile, Tile collisionWallTile, int mazeSize, int cellSize, int mainRoomHeight) : base(algorithm, tileMap, collisionTileMap, wallTile, pathTile, collisionWallTile, mazeSize, cellSize, mainRoomHeight)
         {
             _player = player;
         }
         
         public override void Generate()
         {
             base.Generate();
             GenerateRow();
         }
         
         public void Tick()
         {
             if (_player.View.transform.position.y - mazeHeight < 10)
             {
                 GenerateRow();
             }
         }
         
         private void GenerateRow()
         {
             // obtain walls for 2 rows
             walls = new bool[2, mazeSize];
             
             for (int i = 0; i < 2; i++)
             {
                 for (int j = 0; j < mazeSize; j++)
                 {
                     walls[i, j] = true;
                 }
             }
             
             algorithm.Generate(ref walls);

             //update maze for 2 rows
             SetWallTiles();
         }
         
         private void SetWallTiles()
         {
             //set left side walls
             // P.S. right walls are set because walls[0, lastCell] is always true
             GenerateCell(true, -1 * cellSize, mazeHeight * cellSize);
             GenerateCell(true, -1 * cellSize, (mazeHeight - 1) * cellSize);
             
             for (int x = 0; x < mazeSize; x++)
             {
                 //cellSize = offset
                 var cellStartX  = x * 2 * cellSize;
                 var wallStartX = (x * 2 + 1) * cellSize;
                 var cellStartY = mazeHeight * cellSize;
                 var cellStartYBottom = (mazeHeight - 1) * cellSize;
                 
                 // Set the right walls
                 GenerateCell(false, cellStartX, cellStartY); //Cell is always a pass
                 GenerateCell(walls[0, x], wallStartX, cellStartY);
                 
                 // Set the bottom walls
                 GenerateCell(walls[1, x], cellStartX, cellStartYBottom);
                 GenerateCell(true, wallStartX, cellStartYBottom);
             }

             mazeHeight -= 2;
             collisionTileMap.RefreshAllTiles();
         }
     }
 }