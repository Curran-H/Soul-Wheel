using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject turnManager;

    public static event Action<GameState> OnGameStateChanged;

    public GameState State;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.Sample);
        UpdateGameState(GameState.Text);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Sample:
                HandleSample();
                break;
            case GameState.Text:
                HandleText();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleSample()
    {
        
    }

    private void HandleText()
    {

    }

    public enum GameState
{
    Sample,
    Text
}
}