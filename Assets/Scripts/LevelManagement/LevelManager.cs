using Unity.Cinemachine;
using Entities.Player;
using Generation;
using UnityEngine;
using VContainer;

namespace LevelManagement
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup targetGroup;
        [Inject] private IWorldGenerationManager _worldGenerationManager;
        [Inject] private PlayerGo _playerGo;
        
        public void Start()
        {
            SetPlayer();
            _worldGenerationManager.InitWorld();
        }

        private void SetPlayer()
        {
            _playerGo.Init();
            targetGroup.AddMember(_playerGo.View.transform, 2, 2);
            //get main room coordinates
            _playerGo.View.transform.position = new Vector3(20f, 10f, 0f);
        }
    }
}