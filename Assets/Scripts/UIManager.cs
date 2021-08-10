using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text levelText;

    public void UpdateScore(long score, long scorePerLevel)
    {
        scoreText.text = $"{score}/{scorePerLevel}";
    }

    public void UpdateLevel(long level)
    {
        levelText.text = $"{level}";
    }

    public void ShakeButtonClick()
    {
        gameManager.Shake();
    }
}