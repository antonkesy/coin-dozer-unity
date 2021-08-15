using UnityEngine;

namespace antonkesy
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        public long levelScore;
        public long level = 1;

        [SerializeField] private long scorePerLevel = 20;

        private void Start()
        {
            gameManager.UpdateScoreUI(levelScore, scorePerLevel);
            gameManager.UpdateLevelUI(level);
        }

        public void AddScore(int value)
        {
            levelScore += value;
            CheckNewLevel();
            gameManager.UpdateScoreUI(levelScore, scorePerLevel);
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
            gameManager.UpdateScoreUI(levelScore, scorePerLevel);
            gameManager.UpdateLevelUI(level);
        }
    }
}