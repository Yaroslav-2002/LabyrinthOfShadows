using System;
using UnityEngine;

namespace Assets.Scripts.SavingSystem
{
    public static class SaveSystem
    {
        // Method to save the entire game state
        public static void Serialze(PlayerData playerData, RoomData roomData)
        {
            try
            {
                PlayerSerializer.Serialize(playerData);
                Debug.Log("Player data saved successfully.");

                RoomSerializer.Serialize(roomData);
                Debug.Log("Room data saved successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save game data: {ex.Message}");
            }
        }

        public static void Deserialize(ref PlayerData playerData, ref RoomData roomData)
        {
            try
            {
                // Deserialize the player's data
                playerData = PlayerSerializer.Deserialize();

                // Deserialize the current room data
                roomData = RoomSerializer.Deserialize();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to deserialize game data: {ex.Message}");
            }
        }
    }
}