using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManger;
    private int _score;

    public void AddScore(int value)
    {
        _score += value;
        //TODO check level/save & more
        uiManger.UpdateScore(_score);
    }
}