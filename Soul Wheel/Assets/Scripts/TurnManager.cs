using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    GameManager GameManager;
    public TurnState State;
    public static event Action<TurnState> OnTurnStateChanged;
    public MovementManager movement;

    public List<GameObject> allCharacters;
    public List<GameObject> sortedCharacters;
    int count;
    bool onTurn;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        SpawnEnemies(); //IMPLEMENTATION REQUIRED
        SpeedSort();
        onTurn = true;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (count >= sortedCharacters.Count)
        {
            SpeedSort();
        }
        if (onTurn)
        {
            if (sortedCharacters[count].tag == "Player")
                UpdateGameState(TurnState.PlayerTurn);
            else if (sortedCharacters[count].tag == "Ally")
                UpdateGameState(TurnState.AllyTurn);
            else if (sortedCharacters[count].tag == "Enemy")
                UpdateGameState(TurnState.EnemyTurn);
        }
    }

    //Sort all characters on map by speed value for proper turn order
    public void SpeedSort()
    {
        onTurn = false;
        count = 0;
        allCharacters = new List<GameObject> (GameObject.FindGameObjectsWithTag("Player"));
        allCharacters.AddRange(GameObject.FindGameObjectsWithTag("Ally"));
        allCharacters.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        int i, j;
        GameObject key;
        for (i = 1; i < allCharacters.Count; i++)
        {
            key = allCharacters[i];
            j = i - 1;

            while (j >= 0 && allCharacters[j].GetComponent<Stats>().GetSpeed() < key.GetComponent<Stats>().GetSpeed())
            {
                allCharacters[j + 1] = allCharacters[j];
                j -= 1;
            }
            allCharacters[j + 1] = key;
        }

        sortedCharacters = allCharacters;
        onTurn = true;
        Debug.Log("Sort complete!");
    }

    private void SpawnEnemies() //IMPLEMENTATION REQUIRED
    {

    }

    public void UpdateGameState(TurnState newState)
    {
        State = newState;

        switch (newState)
        {
            case TurnState.PlayerTurn:
                HandlePlayerTurn();
                break;
            case TurnState.AllyTurn:
                HandleAllyTurn();
                break;
            case TurnState.EnemyTurn:
                HandleEnemyTurn();
                break;
            case TurnState.Lose:
                HandleNextLevel();
                break;
            case TurnState.NextLevel:
                HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnTurnStateChanged?.Invoke(newState);
    }

    private void HandlePlayerTurn()
    {
        onTurn = true;
        bool hasMoved = false;
        if (State != TurnState.PlayerTurn)
            return;
        if(Input.GetKeyDown(KeyCode.W))
        {
            if (movement.IsMovementValid(sortedCharacters[count], 0))
            {
                movement.MoveCharacter(sortedCharacters[count], 0, 1);
                hasMoved = true;
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (movement.IsMovementValid(sortedCharacters[count], 1))
            {
                movement.MoveCharacter(sortedCharacters[count], 1, 1);
                hasMoved = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (movement.IsMovementValid(sortedCharacters[count], 2))
            {
                movement.MoveCharacter(sortedCharacters[count], 2, 1);
                hasMoved = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (movement.IsMovementValid(sortedCharacters[count], 3))
            {
                movement.MoveCharacter(sortedCharacters[count], 3, 1);
                hasMoved = true;
            }
        }

        if (hasMoved)
            count++;
    }

    private void HandleAllyTurn()
    {
        if (State != TurnState.AllyTurn)
            return;
    }

    private void HandleEnemyTurn()
    {
        if (State != TurnState.EnemyTurn)
            return;
        //Enemy turn stuff here
        count++;
    }

    private void HandleNextLevel()
    {
        if (State != TurnState.NextLevel)
            return;
        //Next level stuff here
    }

    private void HandleLose()
    {
        if (State != TurnState.Lose)
            return;
        //Lose stuff here
    }

    public enum TurnState
    {
        PlayerTurn,
        AllyTurn,
        EnemyTurn,
        NextLevel,
        Lose
    }
}