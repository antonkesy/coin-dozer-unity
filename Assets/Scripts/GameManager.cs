using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private UIManger _uiManger;
    private int _score = 0;

    public void AddScore(int value)
    {
        _score += value;
        //TODO check level/save & more
        //_uiManger.UpdateScore(_score);
    }
}