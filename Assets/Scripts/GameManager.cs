using System;
using Controls;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private DynamicJoystick _dynamicJoystick;

        private InputController _inputController;
        private void Start()
        {
            GameObject playerInstance = Instantiate(player, new Vector2(-1f, -2.5f), Quaternion.Euler(0,0,0));
            PlayerController playerController = playerInstance.GetComponent<PlayerController>();
            
            _inputController =
                
#if UNITY_EDITOR
                new KeyBoardInputController();
#else 
                new JoystickInputController(_dynamicJoystick);
#endif
            playerController.Initialize(_inputController);
        }
    }
}