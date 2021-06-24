using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public TurnState State;
    public static event Action<TurnState> OnTurnStateChanged;
    public MovementManager movement;
    public AttackManager attack;

    public List<GameObject> allCharacters;
    public List<GameObject> sortedCharacters;
    public int count;

    void Awake()
    {
        Instance = this;
        count = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        sortedCharacters.Add(GameObject.FindGameObjectWithTag("Player"));
    }

    // Update is called once per frame
    void Update()
    {
        if (count >= sortedCharacters.Count)
            SpeedSort();
        else if (sortedCharacters[count].tag == "Player")
            UpdateTurnState(TurnState.PlayerTurn);
        else if (sortedCharacters[count].tag == "Ally")
            UpdateTurnState(TurnState.AllyTurn);
        else if (sortedCharacters[count].tag == "Enemy")
            UpdateTurnState(TurnState.EnemyTurn);
    }

    //Sort all characters on map by speed value for proper turn order
    public void SpeedSort()
    {
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

            while (j >= 0 && allCharacters[j].GetComponent<Stats>().speed < key.GetComponent<Stats>().speed)
            {
                allCharacters[j + 1] = allCharacters[j];
                j -= 1;
            }
            allCharacters[j + 1] = key;
        }

        sortedCharacters = allCharacters;
        Debug.Log("Sort complete!");
    }
    
    public void UpdateTurnState(TurnState newState)
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
        GameObject target = null;
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (movement.IsMovementValid(sortedCharacters[count], 0))
            {
                movement.MoveCharacter(sortedCharacters[count], 0, 1, () => count++);
            }

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (movement.IsMovementValid(sortedCharacters[count], 1))
            {
                movement.MoveCharacter(sortedCharacters[count], 1, 1, () => count++);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (movement.IsMovementValid(sortedCharacters[count], 2))
            {
                movement.MoveCharacter(sortedCharacters[count], 2, 1, () => count++);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (movement.IsMovementValid(sortedCharacters[count], 3))
            {
                movement.MoveCharacter(sortedCharacters[count], 3, 1, () => count++);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            if (attack.IsAttackValid(sortedCharacters[count], 0, ref target))
            {
                attack.AttackTarget(sortedCharacters[count], target, 0, () => count++);
            }

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (attack.IsAttackValid(sortedCharacters[count], 1, ref target))
            {
                attack.AttackTarget(sortedCharacters[count], target, 1, () => count++);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (attack.IsAttackValid(sortedCharacters[count], 2, ref target))
            {
                attack.AttackTarget(sortedCharacters[count], target, 2, () => count++);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (attack.IsAttackValid(sortedCharacters[count], 3, ref target))
            {
                attack.AttackTarget(sortedCharacters[count], target, 3, () => count++);
            }
        }
    }

    private void HandleAllyTurn()
    {
        count++;
    }

    private void HandleEnemyTurn()
    {
        //Move enemy to random location (placeholder for actual AI)
        if (movement.IsMovementValid(sortedCharacters[count], 0)
            || movement.IsMovementValid(sortedCharacters[count], 1)
            || movement.IsMovementValid(sortedCharacters[count], 2)
            || movement.IsMovementValid(sortedCharacters[count], 3))
        {
            int direction = -1;
            do
                direction = UnityEngine.Random.Range(0, 4);
            while (!movement.IsMovementValid(sortedCharacters[count], direction));
            movement.MoveCharacter(sortedCharacters[count], direction, 1, () => count++);
        }
        else
            count++;
    }

    private void HandleNextLevel()
    {
        //Next level stuff here
    }

    private void HandleLose()
    {
        //Lose stuff here
    }

    private void HandleBusy()
    {

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