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
        Instantiate(turnManager);
        UpdateGameState(GameState.PlayerTurn);
        UpdateGameState(GameState.AllyTurn);
        UpdateGameState(GameState.EnemyTurn);
        UpdateGameState(GameState.Lose);
        UpdateGameState(GameState.NextLevel);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.PlayerTurn:
                HandlePlayerTurn();
                break;
            case GameState.AllyTurn:
                HandleAllyTurn();
                break;
            case GameState.EnemyTurn:
                HandleEnemyTurn();
                break;
            case GameState.Lose:
                HandleLose();
                break;
            case GameState.NextLevel:
                HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged(newState);
    }

    private void HandlePlayerTurn()
    {
        
    }

    private void HandleAllyTurn()
    {

    }

    private void HandleEnemyTurn()
    {

    }

    private void HandleLose()
    {

    }

    public enum GameState
{
    PlayerTurn,
    AllyTurn,
    EnemyTurn,
    NextLevel,
    Lose
}
}