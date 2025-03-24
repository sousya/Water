using UnityEngine;
using System.Collections.Generic;
using WaterGame.Models;
using WaterGame.Services;
using WaterGame.Events;

namespace WaterGame.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField] private int maxMoves = 20;
        [SerializeField] private float moveTimeScale = 1f;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject bottlePrefab;
        [SerializeField] private GameObject waterPrefab;
        [SerializeField] private GameObject thunderPrefab;
        
        [Header("Materials")]
        [SerializeField] private Material bottleMaterial;
        [SerializeField] private Material waterMaterial;
        [SerializeField] private Material shineMaterial;
        
        [Header("Colors")]
        [SerializeField] private Color[] waterColors;
        
        private List<BottleController> _bottles = new List<BottleController>();
        private List<BottleState> _bottleStates = new List<BottleState>();
        private int _currentMoves;
        private bool _isPlaying;
        
        private AnimationService _animationService;
        private ResourceService _resourceService;
        
        private void Awake()
        {
            _animationService = ServiceLocator.Get<AnimationService>();
            _resourceService = ServiceLocator.Get<ResourceService>();
            
            EventManager.AddListener(GameEvents.OnBottleSelected, OnBottleSelected);
            EventManager.AddListener(GameEvents.OnLevelComplete, OnLevelComplete);
        }
        
        private void OnDestroy()
        {
            EventManager.RemoveListener(GameEvents.OnBottleSelected, OnBottleSelected);
            EventManager.RemoveListener(GameEvents.OnLevelComplete, OnLevelComplete);
        }
        
        public void InitializeLevel(LevelData levelData)
        {
            ClearLevel();
            CreateBottles(levelData);
            _currentMoves = 0;
            _isPlaying = true;
        }
        
        private void ClearLevel()
        {
            foreach (var bottle in _bottles)
            {
                Destroy(bottle.gameObject);
            }
            _bottles.Clear();
            _bottleStates.Clear();
        }
        
        private void CreateBottles(LevelData levelData)
        {
            foreach (var bottleData in levelData.Bottles)
            {
                var bottle = Instantiate(bottlePrefab, transform);
                var controller = bottle.GetComponent<BottleController>();
                var state = new BottleState
                {
                    MaxCapacity = bottleData.MaxCapacity,
                    Waters = new List<Water>(bottleData.Waters),
                    IsFrozen = bottleData.IsFrozen,
                    IsHidden = bottleData.IsHidden,
                    IsClearHidden = bottleData.IsClearHidden,
                    IsNearHidden = bottleData.IsNearHidden,
                    BottleIndex = _bottles.Count
                };
                
                controller.Initialize(state);
                _bottles.Add(controller);
                _bottleStates.Add(state);
            }
        }
        
        private void OnBottleSelected(object data)
        {
            if (!_isPlaying) return;
            
            var selectedBottle = data as BottleController;
            if (selectedBottle == null) return;
            
            // 处理瓶子选择逻辑
        }
        
        private void OnLevelComplete(object data)
        {
            _isPlaying = false;
            // 处理关卡完成逻辑
        }
        
        public void UndoMove()
        {
            if (_currentMoves <= 0) return;
            
            _currentMoves--;
            // 实现撤销移动逻辑
        }
        
        public void ResetLevel()
        {
            InitializeLevel(GetCurrentLevelData());
        }
        
        private LevelData GetCurrentLevelData()
        {
            // 获取当前关卡数据
            return new LevelData();
        }
        
        public void PauseGame()
        {
            _isPlaying = false;
            Time.timeScale = 0;
        }
        
        public void ResumeGame()
        {
            _isPlaying = true;
            Time.timeScale = moveTimeScale;
        }
    }
} 