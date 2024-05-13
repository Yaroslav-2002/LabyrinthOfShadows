using Assets.Scripts.Generation;
using Generation.Algorithms;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;

namespace Generation
{
    public class WorldGenerator : IWorldGenerator, ITickable
    {
        [Inject] private IGenerationAlgorithm _algorithm;

        private readonly Tilemap MazeTilemap;
        private readonly Tilemap CollisionTilemap;

        private readonly RuleTile WallTile;
        private readonly RuleTile PathTile;

        private readonly Tile CollisionTile;

        private readonly int CellSize;

        private readonly int RoomHeight;
        private readonly int RoomWidth;
        private readonly int NodesNumHeight;
        private readonly int NodesNumWidth;

        private readonly int MainRoomWidth;
        private readonly int MainRoomHeight;

        private readonly int CorridorWidth;
        private readonly int CorridorHeight;

        private readonly GameObject Door;

        private bool[,,] _mazeNodes;
        private int Yoffeset;

        private Dictionary<int, Room> rooms;

        public WorldGenerator(MapConfiguration mapConfiguration, Tilemap mazeTilemap, Tilemap collisionTileMap, IGenerationAlgorithm algorithm)
        {
            _algorithm = algorithm;
            CollisionTilemap = collisionTileMap;
            MazeTilemap = mazeTilemap;

            WallTile = mapConfiguration.wallTile;
            PathTile = mapConfiguration.pathTile;
            CollisionTile = mapConfiguration.collisionWallTile;
            CellSize = mapConfiguration.cellSize;
            RoomHeight = mapConfiguration.roomHeight;
            RoomWidth = mapConfiguration.roomWidth;
            MainRoomWidth = mapConfiguration.mainRoomWidth;
            MainRoomHeight = mapConfiguration.mainRoomHeight;
            CorridorHeight = mapConfiguration.corridorHeight;
            CorridorWidth = mapConfiguration.corridorWidth;
            Door = mapConfiguration.door;

            NodesNumHeight = RoomHeight / 2;
            NodesNumWidth = RoomWidth / 2;
        }

        public void Generate()
        {
            _algorithm.Init();
            GenerateMainRoom();
            GenerateCorridor();
            GenerateRoom();
        }

        public void Tick()
        {
           
        }

        int roomId;
        private void GenerateRoom()
        {
            _mazeNodes = new bool[NodesNumWidth, NodesNumHeight, 2];

            for (int y = 0; y < NodesNumHeight; y++)
            {
                for (int x = 0; x < NodesNumWidth; x++)
                {
                    _mazeNodes[x, y, 0] = true;
                    _mazeNodes[x, y, 1] = true;
                }
            }

            _algorithm.Generate(ref _mazeNodes);

            Room room = new();
            SetWalls(ref room);

            rooms.Add(roomId, room);

            roomId++;
        }

        private void GenerateCorridor()
        {
            for (int y = 0; y < CorridorHeight; y++)
            {
                for (int x = 0; x < CorridorWidth; x++)
                {
                    if (y == 0 && x == CorridorWidth / 2)
                    {
                        GenerateDoorCell(x, y);
                    }
                    else if(x == 0 || x == CorridorWidth - 1 || y == 0)
                    {
                        GenerateCell(true, new Vector2(x + RoomWidth / 2 - CorridorWidth / 2, y));
                        continue;
                    }
                    GenerateCell(false, new Vector2(x + RoomWidth / 2 - CorridorWidth / 2, y));
                }
            }
            Yoffeset += CorridorHeight;
        }

        private void GenerateDoorCell(int x, int y)
        {
            GenerateCell(false, new Vector2(x + RoomWidth / 2 - CorridorWidth / 2, y));
            GameObject.Instantiate(Door, MazeTilemap.CellToWorld(new Vector3Int((x + RoomWidth / 2 - CorridorWidth / 2) * CellSize, Yoffeset * CellSize + 1, 0)), Quaternion.identity);
        }

        public void GenerateMainRoom()
        {
            for (int y = 0; y < MainRoomHeight; y++)
            {
                for (int x = 0; x < MainRoomWidth; x++)
                {
                    if (x == 0 || x == MainRoomWidth - 1)
                    {
                        GenerateCell(true, new Vector2(x + RoomWidth / 2 - MainRoomWidth / 2, y));
                        continue;
                    }
                    GenerateCell(false, new Vector2(x + RoomWidth / 2 - MainRoomWidth / 2, y));
                }
            }
            Yoffeset += MainRoomHeight;
        }

        private void GenerateCell(bool isWall, Vector2 cellPosition)
        {
            int yStartCoordinate = ((int)cellPosition.y + Yoffeset) * CellSize;
            int xStartCoordinate = (int)cellPosition.x * CellSize;

            for (int y = yStartCoordinate; y < yStartCoordinate + CellSize; y++)
            {
                for (int x = xStartCoordinate; x < xStartCoordinate + CellSize; x++)
                {
                    MazeTilemap.SetTile(new Vector3Int(x, y, 0), isWall ? WallTile : PathTile);
                    CollisionTilemap.SetTile(new Vector3Int(x, y, 0), isWall ? CollisionTile : null);
                }
            }
        }

        private void SetWalls(ref Room room)
        {
            for (int y = 0; y < NodesNumHeight; y++)
            {
                for (int x = 0; x < NodesNumWidth; x++)
                {
                    Vector2 node = new Vector2(x * 2, y * 2);
                    Vector2 nodeRightPos = new Vector2(x * 2 + 1, y * 2);
                    Vector2 nodeBottomPos = new Vector2(x * 2, y * 2 + 1);
                    Vector2 nodeWallPos = new Vector2(x * 2 + 1, y * 2 + 1);

                    GenerateCell(true, new Vector2(- 1, y * 2 ));
                    GenerateCell(true, new Vector2(-1, y * 2 + 1));

                    GenerateCell(false, node);
                    GenerateCell(_mazeNodes[x, y, 0], nodeRightPos);
                    GenerateCell(_mazeNodes[x, y, 1], nodeBottomPos);
                    GenerateCell(true, nodeWallPos);
                }
            }
            Yoffeset += RoomHeight;
            CollisionTilemap.RefreshAllTiles();
        }
    }
}