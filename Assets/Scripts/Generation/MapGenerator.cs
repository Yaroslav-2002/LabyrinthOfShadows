using Assets.Scripts.Generation;
using Generation.Algorithms;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;

namespace Generation
{
    public class MapGenerator : MonoBehaviour, IMapGenerator
    {
        [Inject] private IGenerationAlgorithm _algorithm;

        private Tilemap _mazeTilemap;
        private Tilemap _collisionTilemap;

        private readonly RuleTile _wallTile;
        private readonly RuleTile _pathTile;
        private readonly Tile _collisionTile;

        private readonly int _cellSize;
        private readonly int _roomHeight;
        private readonly int _roomWidth;
        private readonly int _nodesNumHeight;
        private readonly int _nodesNumWidth;
        private readonly int _mainRoomWidth;
        private readonly int _mainRoomHeight;
        private readonly GameObject _door;
        private readonly Grid _grid;

        private int _yOffset;
        private readonly Dictionary<int, Room> _rooms;

        public MapGenerator(MapConfiguration mapConfiguration, IGenerationAlgorithm algorithm, Grid grid)
        {
            _algorithm = algorithm;

            _wallTile = mapConfiguration.wallTile;
            _pathTile = mapConfiguration.pathTile;
            _collisionTile = mapConfiguration.collisionWallTile;
            _cellSize = mapConfiguration.cellSize;
            _nodesNumHeight = mapConfiguration.numHeightNodes;
            _nodesNumWidth = mapConfiguration.numWidthNodes;
            _mainRoomWidth = mapConfiguration.mainRoomWidth;
            _mainRoomHeight = mapConfiguration.mainRoomHeight;
            _door = mapConfiguration.door;
            _grid = grid;

            _roomHeight = _nodesNumHeight * 2 + 4;
            _roomWidth = _nodesNumWidth * 2;

            _rooms = new Dictionary<int, Room>();
        }

        public void Generate()
        {
            GenerateMainRoom();
            GenerateRoom();
        }

        private void GenerateRoom()
        {
            var room = InstantiateRoom();
            SetWalls();
            _rooms.Add(Room.roomId, room);
        }

        private Room InstantiateRoom()
        {
            var room = new Room(_grid);
            room.InstantiateRoom(new Vector3 (0, _yOffset * _cellSize, 0));

            _collisionTilemap = room.CollisionTilemap;
            _mazeTilemap = room.MazeTilemap;

            return room;
        }

        public void GenerateMainRoom()
        {
            var room = InstantiateRoom();
            int xOffset = _roomWidth / 2 - _mainRoomWidth / 2 - 1;

            for (int y = 0; y < _mainRoomHeight; y++)
            {
                for (int x = 0; x < _mainRoomWidth; x++)
                {
                    bool isWall = x == 0 || x == _mainRoomWidth - 1 || y == 0 || y == _mainRoomHeight - 1;
                    GenerateCell(isWall ? _wallTile : _pathTile, isWall ? _collisionTile : null, new Vector2Int(x + xOffset, y));
                }
            }
            _yOffset += _mainRoomHeight;
        }

        private void GenerateCell(TileBase tile, TileBase collisionTile, Vector2Int cellPosition)
        {
            for (int y = 0; y < _cellSize; y++)
            {
                for (int x = 0; x < _cellSize; x++)
                {
                    var tilePos = new Vector3Int(cellPosition.x * _cellSize + x, cellPosition.y * _cellSize + y, 0);
                    _mazeTilemap.SetTile(tilePos, tile);
                    _collisionTilemap.SetTile(tilePos, collisionTile);
                }
            }
        }

        private void GenerateDoorCell(Vector2Int cellPosition)
        {
            for (int y = 0; y < _cellSize; y++)
            {
                for (int x = 0; x < _cellSize; x++)
                {
                    var tilePos = new Vector3Int(cellPosition.x * _cellSize + x, cellPosition.y * _cellSize + y, 0);
                    bool isPath = x == _cellSize / 2 || x == _cellSize / 2 - 1;

                    if (x == _cellSize / 2 && y == 0)
                    {
                        Instantiate(_door, _mazeTilemap.CellToWorld(tilePos + new Vector3Int(0, 1, 0)), Quaternion.identity);
                    }

                    _mazeTilemap.SetTile(tilePos, isPath ? _pathTile : _wallTile);
                    _collisionTilemap.SetTile(tilePos, isPath ? null : _collisionTile);
                }
            }
        }

        private void SetWalls()
        {
            GeneratePathRow(_roomHeight - 2);
            GeneratePathRow(1);

            var algorithmRows = _algorithm.GenerateRow();

            for (int y = 1; y <= _nodesNumHeight; y++)
            {
                algorithmRows.MoveNext();
                var currentRow = algorithmRows.Current;

                for (int x = 0; x < _nodesNumWidth; x++)
                {
                    Vector2Int currentNode = new Vector2Int(x * 2, y * 2);
                    GenerateCell(_pathTile, null, currentNode);

                    GenerateCell(currentRow[x, 0] ? _wallTile : _pathTile, currentRow[x, 0] ? _collisionTile : null, currentNode + Vector2Int.right);
                    GenerateCell(currentRow[x, 1] ? _wallTile : _pathTile, currentRow[x, 1] ? _collisionTile : null, currentNode + Vector2Int.up);
                    GenerateCell(_wallTile, _collisionTile, currentNode + Vector2Int.one);
                }
            }
            _collisionTilemap.RefreshAllTiles();
            GenerateOutline();

            _yOffset += _roomHeight + 1;
        }

        private void GenerateOutline()
        {
            for (int y = 0; y < _roomHeight; y++)
            {
                GenerateCell(_wallTile, _collisionTile, new Vector2Int(-1, y));
                GenerateCell(_wallTile, _collisionTile, new Vector2Int(_roomWidth - 1, y));
            }

            for (int x = -1; x < _roomWidth; x++)
            {
                if (x == _roomWidth / 2)
                {
                    GenerateDoorCell(new Vector2Int(x, _roomHeight - 1));
                }
                else
                {
                    GenerateCell(_wallTile, _collisionTile, new Vector2Int(x, _roomHeight - 1));
                }

                GenerateCell(x == _roomWidth / 2 ? _pathTile : _wallTile, x == _roomWidth / 2 ? null : _collisionTile, new Vector2Int(x, 0));
            }
        }

        private void GeneratePathRow(int y)
        {
            for (int x = 0; x < _roomWidth; x++)
            {
                GenerateCell(_pathTile, null, new Vector2Int(x, y));
            }
        }
    }
}
