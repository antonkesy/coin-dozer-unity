using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PowerUpManager powerUpManager;

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

    public void DEBUG_WallButtonClick()
    {
        Debug.Log("Wall activated");
        gameManager.ActivateWallPowerUp();
    }

    public void DEBUG_ResetSaveGame()
    {
        Debug.Log("Reset save game");
        GameSaver.ResetData();
    }

    public void DEBUG_SpawnPowerUpWall()
    {
        powerUpManager.SpawnWallPowerUpObject();
    }
}