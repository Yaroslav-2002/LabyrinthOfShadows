using Cinemachine;
using Controls;
using UnityEngine;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private DynamicJoystick dynamicJoystick;
        [SerializeField] private CinemachineTargetGroup targetGroup;

        private InputController _inputController;
        
        private void Start()
        {
            GameObject playerInstance = Instantiate(player, new Vector2(10f, 10f), Quaternion.Euler(0,0,0));
            PlayerController playerController = playerInstance.GetComponent<PlayerController>();
            targetGroup.AddMember(playerInstance.transform, 1, 0);
            _inputController =
#if UNITY_EDITOR
                new KeyBoardInputController();
#else 
                new JoystickInputController(dynamicJoystick);
#endif
            playerController.Initialize(_inputController);
        }
    }
}