using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MovementController : IMovementController
    {
        
        public Action<Vector2> OnMovementUpdated;
        private const float MoveSpeed = 1f; // Speed of the player movement

        private readonly PlayerController _playerController;

        public MovementController(PlayerController player)
        {
            _playerController = player;
        }

        public void UpdateMovement()
        {
            var movement = _playerController.movingAxis;
            if (movement.x != 0f && movement.y != 0f)
            {
                movement *= 0.7f;
            }

            movement = movement * MoveSpeed;
            MovePlayer(movement);
        }
        
        private void MovePlayer(Vector2 movement)
        {
            _playerController.rb.velocity = movement;
        }
        
    }
}