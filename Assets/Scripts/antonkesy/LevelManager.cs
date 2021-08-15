using System;
using UnityEngine;

namespace antonkesy
{
    public class LevelManager : MonoBehaviour
    {
        private GameManager _gameManager;
        public long levelScore;
        public long level = 1;

        [SerializeField] private long scorePerLevel = 20;

        private void Awake()
        {
            _gameManager = GetComponent<GameManager>();
        }

        private void Start()
        {
            _gameManager.UpdateScoreUI(levelScore, scorePerLevel);
            _gameManager.UpdateLevelUI(level);
        }

        public void AddScore(int value)
        {
            levelScore += value;
            CheckNewLevel();
            _gameManager.UpdateScoreUI(levelScore, scorePerLevel);
        }

        private void CheckNewLevel()
        {
            if (levelScore >= scorePerLevel)
            {
                StartNextLevel();
            }
        }

        private void StartNextLevel()
        {
            levelScore -= scorePerLevel;
            ++level;
            scorePerLevel *= 2;
            CheckNewLevel();
            _gameManager.UpdateScoreUI(levelScore, scorePerLevel);
            _gameManager.UpdateLevelUI(level);
        }
    }
}