using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public DungeonManager dungeonManager;
    public AudioManager audioManager;
    public AnimationManager animationManager;

    public static event Action<GameState> OnGameStateChanged;

    public GameState State;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        GameObject.Instantiate(audioManager, gameObject.transform.parent);
        GameObject.Instantiate(animationManager, gameObject.transform.parent);
    }

    private void Start()
    {
        UpdateGameState(GameState.Dungeon);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Dungeon:
                HandleDungeon();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleDungeon()
    {
        GameObject.Instantiate(dungeonManager, gameObject.transform.parent);
    }

    public enum GameState
    {
        Dungeon
    }
}