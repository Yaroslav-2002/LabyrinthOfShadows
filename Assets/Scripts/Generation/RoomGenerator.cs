using System.Collections.Generic;
using Rooms;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Generation
{
    public class RoomGenerator : MonoBehaviour
    {
        public Tilemap tilemap; // Assign in inspector
        public Tile wallTile; // Assign your wall tile in inspector

        private const int Rows = 100;
        private const int Cols = 100;

        private void Start()
        {
            GenerateRoom();
        }
        
        private void GenerateRoom()
        {
        }
    }
    
}
