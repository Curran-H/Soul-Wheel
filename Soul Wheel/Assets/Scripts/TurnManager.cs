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

    int direction; //Keeps track of direction of actions (0 = North, 1 = West, 2 = South, 3 = East)
    int currentAction; //Keeps track of which action is being performed (0 = None/Idle, 1 = Movement, 2 = Attack)
    int moveCount; //Keeps track of how many times entity has moved (for movement on turn)

    void Awake()
    {
        Instance = this;
        count = 0;
        direction = -1;
        currentAction = 0;
        moveCount = 0;
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
        //GameObject target = null;
        if (currentAction == 0)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (movement.IsMovementValid(sortedCharacters[count], 0))
                {
                    currentAction = 1;
                    direction = 0;
                }

            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (movement.IsMovementValid(sortedCharacters[count], 1))
                {
                    currentAction = 1;
                    direction = 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (movement.IsMovementValid(sortedCharacters[count], 2))
                {
                    currentAction = 1;
                    direction = 2;
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (movement.IsMovementValid(sortedCharacters[count], 3))
                {
                    currentAction = 1;
                    direction = 3;
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //if (attack.IsAttackValid(sortedCharacters[count], 0, ref target))
                //{
                currentAction = 2;
                direction = 0;
                attack.AttackTargetIfValid(sortedCharacters[count], 0);
                //}

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //if (attack.IsAttackValid(sortedCharacters[count], 1, ref target))
                //{
                currentAction = 2;
                direction = 1;
                attack.AttackTargetIfValid(sortedCharacters[count], 1);
                //}
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //if (attack.IsAttackValid(sortedCharacters[count], 2, ref target))
                //{
                currentAction = 2;
                direction = 2;
                attack.AttackTargetIfValid(sortedCharacters[count], 2);
                //}
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                //if (attack.IsAttackValid(sortedCharacters[count], 3, ref target))
                //{
                currentAction = 2;
                direction = 3;
                attack.AttackTargetIfValid(sortedCharacters[count], 3);
                //}
            }
        }
        else if (currentAction == 1)
        {
            movement.MoveCharacter(sortedCharacters[count], direction, 1, () => { moveCount++; });
            if (moveCount >= 600)
            {
                movement.CenterCharacter(sortedCharacters[count], direction);
                moveCount = 0;
                direction = -1;
                currentAction = 0;
                count++;
            }
        }
        else if (currentAction == 2)
            attack.IsAttackFinished(sortedCharacters[count], direction, () => { movement.CenterCharacter(sortedCharacters[count], direction); direction = -1; currentAction = 0; count++; });
    }

    private void HandleAllyTurn()
    {
        count++;
    }

    private void HandleEnemyTurn()
    {
        //Check if enemy can move in any direction (placeholder for actual AI)
        if ((movement.IsMovementValid(sortedCharacters[count], 0)
            || movement.IsMovementValid(sortedCharacters[count], 1)
            || movement.IsMovementValid(sortedCharacters[count], 2)
            || movement.IsMovementValid(sortedCharacters[count], 3))
            && direction == -1)
        {
            int currentDirection = -1;
            do
                currentDirection = UnityEngine.Random.Range(0, 4);
            while (!movement.IsMovementValid(sortedCharacters[count], currentDirection));
            direction = currentDirection;
        }

        //Move enemy to random location (placeholder for actual AI)
        else if (direction > -1)
        {
            movement.MoveCharacter(sortedCharacters[count], direction, 1, () => moveCount++);
            if (moveCount >= 600)
            {
                movement.CenterCharacter(sortedCharacters[count], direction);
                moveCount = 0;
                direction = -1;
                count++;
            }
        }

        //If enemy cannot move, do nothing (placeholder for actual AI)
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