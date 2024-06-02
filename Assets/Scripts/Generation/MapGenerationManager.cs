using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Generation;
using Generation.Algorithms;
using UnityEngine.AI;
using UnityEngine.UI;
using NavMeshPlus.Components;
using System;
using UnityEditor;

namespace Generation
{
    public class MapGenerationManager : MonoBehaviour, IMapGenerationManager, IDataPersistence
    {
        [SerializeField] float trapSpawnProbability;

        [SerializeField] private MapConfiguration mapConfiguration;
        [SerializeField] private NavMeshSurface navmeshSurface;
        [SerializeField] private Grid grid;
        [SerializeField] private Tilemap wallTilemap;
        [SerializeField] private Tilemap pathTilemap;
        [SerializeField] private Tilemap collisionTilemap;
        [SerializeField] private Tilemap decorationTilemap;
        [SerializeField] private GameObject chest;

        [SerializeField] private Transform DoorsTransform;
        [SerializeField] private Transform ChestsTransform;
        [SerializeField] private TrapManager trapManager;
        [SerializeField] private DoorManager doorManager;

        public Action<Dictionary<int, Room>> OnMapGenerated;
        private IGenerationAlgorithm algorithm;
        public Dictionary<int, Room> rooms { get; private set; }

        #region Configuration Fields
        private RuleTile wallTile;
        private RuleTile pathTile;
        private Tile collisionTile;
        private int cellSize;
        private int roomHeight;
        private int roomWidth;
        private int nodesNumHeight;
        private int nodesNumWidth;
        private int mainRoomHeight;
        private int mainRoomWidth;
        private static int yOffset;
        private int roomId;
        #endregion

        private void Update()
        {
            if (navmeshSurface.navMeshData == null)
            {
                return;
            }
            navmeshSurface.UpdateNavMesh(navmeshSurface.navMeshData);
        }

        #region Map Generation
        public void InitMap()
        {
            rooms = new Dictionary<int, Room>();
            InitializeConfiguration();

            algorithm = new EllerMazeAlgorithm(nodesNumWidth);

            int roomId = 1;
            GenerateMainRoom();
            for (int i = 0; i < 8; i++)
            {
                rooms.Add(roomId, GenerateRoom());
                roomId++;
            }

            navmeshSurface.BuildNavMeshAsync();

            OnMapGenerated.Invoke(rooms);
        }

        private void InitializeConfiguration()
        {
            cellSize = mapConfiguration.cellSize;
            nodesNumHeight = mapConfiguration.numHeightNodes;
            nodesNumWidth = mapConfiguration.numWidthNodes;
            collisionTile = mapConfiguration.collisionWallTile;
            wallTile = mapConfiguration.wallTile;
            pathTile = mapConfiguration.pathTile;
            mainRoomHeight = mapConfiguration.mainRoomHeight;
            mainRoomWidth = mapConfiguration.mainRoomWidth;
            roomHeight = nodesNumHeight * 2 + 2;
            roomWidth = nodesNumWidth * 2 - 1;
        }

        private void GenerateDoorCell(Vector2Int cellPosition, bool spawnDoor)
        {
            for (int y = 0; y < cellSize; y++)
            {
                for (int x = 0; x < cellSize; x++)
                {
                    var tilePos = new Vector3Int(cellPosition.x * cellSize + x, (cellPosition.y + yOffset) * cellSize + y, 0);
                    bool isPath = x == cellSize / 2 || x == cellSize / 2 - 1;

                    if (x == cellSize / 2 && y == 0 && spawnDoor)
                    {
                        doorManager.SpawnDoor(tilePos + Vector3Int.up);
                    }

                    SetTile(tilePos, isPath);
                }
            }
        }

        private void SetTile(Vector3Int tilePos, bool isPath)
        {
            if (isPath)
            {
                pathTilemap.SetTile(tilePos, pathTile);
                wallTilemap.SetTile(tilePos, null);
                collisionTilemap.SetTile(tilePos, null);
            }
            else
            {
                collisionTilemap.SetTile(tilePos, collisionTile);
                wallTilemap.SetTile(tilePos, wallTile);
            }
        }

        private void GenerateCell(bool isWall, Vector2Int cellPosition)
        {
            for (int y = 0; y < cellSize; y++)
            {
                for (int x = 0; x < cellSize; x++)
                {
                    var tilePos = new Vector3Int(cellPosition.x * cellSize + x, (cellPosition.y + yOffset) * cellSize + y, 0);
                    if (isWall)
                    {
                        collisionTilemap.SetTile(tilePos, collisionTile);
                        wallTilemap.SetTile(tilePos, wallTile);
                    }
                    else
                    {
                        bool isBorderTile = x != 0 && y != 0 && x != cellSize - 1;
                        if (UnityEngine.Random.value < trapSpawnProbability)
                        {
                            trapManager.SpawnTrap(tilePos + new Vector3(0.5f, 0.5f, 0));
                        }

                        pathTilemap.SetTile(tilePos, pathTile);
                    }
                }
            }
        }

        private Room GenerateRoom()
        {
            Room currentRoom = new Room(roomId);  // roomId should be a class-wide variable tracking the current room number
            GeneratePathRow(roomHeight);
            GeneratePathRow(1);

            var algorithmRows = algorithm.GenerateRow();

            for (int y = 1; y <= nodesNumHeight; y++)
            {
                algorithmRows.MoveNext();
                var currentRow = algorithmRows.Current;

                for (int x = 0; x < nodesNumWidth; x++)
                {
                    Vector2Int currentNode = new Vector2Int(x * 2, y * 2);
                    GenerateCell(false, currentNode);

                    // Check conditions for spawning, example:
                    if (!currentRow[x, 0] && !currentRow[x, 1]) // Example condition
                    {
                        currentRoom.MobSpawnPositions.Add(new Vector3Int(currentNode.x * cellSize, (currentNode.y + yOffset) * cellSize, 0));
                    }

                    GenerateCell(currentRow[x, 0], currentNode + Vector2Int.right);
                    GenerateCell(currentRow[x, 1], currentNode + Vector2Int.up);
                    GenerateCell(true, currentNode + Vector2Int.one);
                }
            }
            collisionTilemap.RefreshAllTiles();
            GenerateOutline();

            yOffset += roomHeight + 1;

            return currentRoom;
        }

        private void GenerateOutline()
        {
            int wallMidpoint = roomWidth / 2;

            for (int y = 0; y <= roomHeight; y++)
            {
                GenerateCell(true, new Vector2Int(-1, y));
                GenerateCell(true, new Vector2Int(roomWidth, y));
            }

            for (int x = -1; x <= roomWidth; x++)
            {
                GenerateCell(true, new Vector2Int(x, roomHeight + 1));
                if (x != wallMidpoint)
                {
                    GenerateCell(true, new Vector2Int(x, 0));
                }
            }

            GenerateDoorCell(new Vector2Int(wallMidpoint, roomHeight + 1), true);
        }

        private void GeneratePathRow(int y)
        {
            for (int x = 0; x < roomWidth; x++)
            {
                GenerateCell(false, new Vector2Int(x, y));
            }
        }

        private void GenerateMainRoom()
        {
            int xOffset = roomWidth / 2 - mainRoomWidth / 2;

            for (int y = 0; y < mainRoomHeight; y++)
            {
                for (int x = 0; x < mainRoomWidth; x++)
                {
                    bool isWall = (x == 0 || x == mainRoomWidth - 1 || y == 0 || y == mainRoomHeight - 1);
                    GenerateCell(isWall, new Vector2Int(x + xOffset, y));
                }
            }
            GenerateDoorCell(new Vector2Int(mainRoomWidth / 2 + xOffset, mainRoomHeight - 1), true);

            yOffset += mainRoomHeight - 1;
        }
        #endregion

        #region Save/Load
        public void SaveData(GameData data)
        {
            data.collisionTilemapData = SerializeTilemap(collisionTilemap);
            data.wallTilemapData = SerializeTilemap(wallTilemap);
            data.pathTilemapData = SerializeTilemap(pathTilemap);
        }

        public void LoadData(GameData gameData)
        {
            if (gameData.collisionTilemapData == null || gameData.wallTilemapData == null || gameData.pathTilemapData == null)
            {
                InitMap();
            }
            else
            {
                ApplyTilemapData(collisionTilemap, gameData.collisionTilemapData, false);
                navmeshSurface.BuildNavMeshAsync();
                ApplyTilemapData(pathTilemap, gameData.pathTilemapData, true);
                ApplyTilemapData(wallTilemap, gameData.wallTilemapData, true);
            }
        }
        private TilemapData SerializeTilemap(Tilemap tilemap)
        {
            TilemapData tilemapData = new TilemapData { tiles = new List<TileData>() };
            foreach (var position in tilemap.cellBounds.allPositionsWithin)
            {
                TileBase tile = tilemap.GetTile(position);
                if (tile != null)
                {
                    tilemapData.tiles.Add(new TileData { posX = position.x, posY = position.y, tileBaseName = tile.name });
                }
            }
            return tilemapData;
        }

        private void ApplyTilemapData(Tilemap tilemap, TilemapData tilemapData, bool isRuleTile)
        {
            tilemap.ClearAllTiles();
            foreach (TileData tileData in tilemapData.tiles)
            {
                Vector3Int position = new Vector3Int(tileData.posX, tileData.posY, 0);
                TileBase tile = isRuleTile
                    ? Resources.Load<AdvancedRuleTile>(tileData.tileBaseName)
                    : Resources.Load<Tile>(tileData.tileBaseName);

                if (tile != null)
                {
                    tilemap.SetTile(position, tile);
                }
                else
                {
                    Debug.LogError($"Failed to load tile: {tileData.tileBaseName}");
                }
            }
        }
        #endregion

        public Vector3 GetPLayerSpawnPosition()
        {
            return new Vector3(roomWidth / 2 * cellSize, mainRoomHeight / 2 * cellSize, 0);
        }
    }
}