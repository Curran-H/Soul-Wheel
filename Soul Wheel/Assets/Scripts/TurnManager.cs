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
    GameObject player;
    public int count;

    public int direction; //Keeps track of direction of actions (0 = North, 1 = West, 2 = South, 3 = East)
    public int currentAction; //Keeps track of which action is being performed (0 = None/Idle, 1 = Movement, 2 = Attack)
    public int moveCount; //Keeps track of how many times entity has moved (for movement on turn)
    public List<int> dirList; //Keeps track of available directions for AI to move in
    public bool turnBuffer; //Starts a timer between turns for design/debugging purposes
    //public int finishTurnCount; //The previously mentioned timer
    public bool attackFinished; //True if attack animation is complete, false if not

    void Awake()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        count = 0;
        direction = -1;
        currentAction = 0;
        moveCount = 0;
        dirList = new List<int>();
        turnBuffer = false;
        //finishTurnCount = 0;
        //attackFinished = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        sortedCharacters.Add(GameObject.FindGameObjectWithTag("Player"));
    }

    // Update is called once per frame
    void Update()
    {
        if (turnBuffer)
        {
            //Uncomment this code and those relating to it to add a timer between turns (for design/debugging purposes)
            //finishTurnCount++;
            //if(finishTurnCount > 180)
            //{
                dirList.Clear();
                direction = -1;
                currentAction = 0;
                //finishTurnCount = 0;
                turnBuffer = false;
            //}
        }
        else
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
        //Debug.Log("Sort complete!");
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
                if (movement.IsMovementValid(player, 0))
                {
                    currentAction = 1;
                    direction = 0;
                }

            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (movement.IsMovementValid(player, 1))
                {
                    currentAction = 1;
                    direction = 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (movement.IsMovementValid(player, 2))
                {
                    currentAction = 1;
                    direction = 2;
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (movement.IsMovementValid(player, 3))
                {
                    currentAction = 1;
                    direction = 3;
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //if (attack.IsAttackValid(player, 0))
                //{
                currentAction = 2;
                direction = 0;
                attackFinished = false;
                attack.AttackTargetIfValid(player, 0);
                //}

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //if (attack.IsAttackValid(player, 1))
                //{
                currentAction = 2;
                direction = 1;
                attackFinished = false;
                attack.AttackTargetIfValid(player, 1);
                //}
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //if (attack.IsAttackValid(player, 2))
                //{
                currentAction = 2;
                direction = 2;
                attackFinished = false;
                attack.AttackTargetIfValid(player, 2);
                //}
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                //if (attack.IsAttackValid(player, 3))
                //{
                currentAction = 2;
                direction = 3;
                attackFinished = false;
                attack.AttackTargetIfValid(player, 3);
                //}
            }
        }
        else if (currentAction == 1)
        {
            movement.MoveCharacter(player, direction, 1, () => { moveCount++; });
            if (moveCount >= 300)
            {
                movement.CenterCharacter(player, direction);
                moveCount = 0;
                count++;
                turnBuffer = true;
                return;
            }
        }
        else if (currentAction == 2)
        {
            if (attackFinished)
            {
                movement.CenterCharacter(player, direction);
                count++;
                turnBuffer = true;
                return;
            }
        }
    }

    private void HandleAllyTurn()
    {
        count++;
        turnBuffer = true;
        return;
    }

    private void HandleEnemyTurn()
    {
        GameObject currEnemy = sortedCharacters[count];

        //If enemy is next to player/ally, attack them and end turn
        if(currentAction == 0
            && (attack.IsAttackValid(currEnemy, 0)
            || attack.IsAttackValid(currEnemy, 1)
            || attack.IsAttackValid(currEnemy, 2)
            || attack.IsAttackValid(currEnemy, 3)))
        {
            if (attack.IsAttackValid(currEnemy, 0))
                dirList.Add(0);
            if (attack.IsAttackValid(currEnemy, 1))
                dirList.Add(1);
            if (attack.IsAttackValid(currEnemy, 2))
                dirList.Add(2);
            if (attack.IsAttackValid(currEnemy, 3))
                dirList.Add(3);

            currentAction = 2; //Current action is now attacking
            direction = dirList[UnityEngine.Random.Range(0, dirList.Count)];
            attack.AttackTarget(currEnemy, direction);
            attackFinished = false;
        }

        if (currentAction == 2)
        {
            if(attackFinished)
            {
                movement.CenterCharacter(currEnemy, direction);
                count++;
                turnBuffer = true;
                return;
            }
        }
        else
        {
            //Otherwise, move towards player if possible
            //Put directions enemy is able to move towards the player in a list (if there are any)
            if (dirList.Count == 0 && direction == -1 && currentAction == 0)
            {
                currentAction = 1; //Current action is now movement
                double xDiff = currEnemy.transform.position.x - player.transform.position.x; //Difference between enemy's x-value and player's x-value
                double yDiff = currEnemy.transform.position.y - player.transform.position.y; //Difference between enemy's y-value and player's y-value

                if (yDiff > 0.1) //Add down to list of directions if valid
                {
                    if (movement.IsMovementValid(currEnemy, 2))
                        dirList.Add(2);
                }
                else if (yDiff < -0.1) //Add up to list of directions if valid
                {
                    if (movement.IsMovementValid(currEnemy, 0))
                        dirList.Add(0);
                }
                if (xDiff > 0.1) //Add left to list of directions if valid
                {
                    if (movement.IsMovementValid(currEnemy, 1))
                        dirList.Add(1);
                }
                else if (xDiff < -0.1) //Add right to list of directions if valid
                {
                    if (movement.IsMovementValid(currEnemy, 3))
                        dirList.Add(3);
                }
            }

            if (currentAction == 1)
            {
                //If enemy can move toward player, set up to do so
                if (dirList.Count > 0 && direction == -1)
                    direction = dirList[UnityEngine.Random.Range(0, dirList.Count)];

                //If enemy cannot move toward player, set up to move in random direction away from player if possible
                else if ((movement.IsMovementValid(currEnemy, 0)
                        || movement.IsMovementValid(currEnemy, 1)
                        || movement.IsMovementValid(currEnemy, 2)
                        || movement.IsMovementValid(currEnemy, 3))
                        && direction == -1)
                {
                    //Check if enemy can move in any direction
                    int currentDirection = -1;
                    do
                        currentDirection = UnityEngine.Random.Range(0, 4);
                    while (!movement.IsMovementValid(currEnemy, currentDirection));
                    direction = currentDirection;
                }

                //Move in direction specified above, or end turn if unable to move
                if (direction > -1)
                {
                    movement.MoveCharacter(currEnemy, direction, 1, () => moveCount++);
                    if (moveCount >= 300)
                    {
                        movement.CenterCharacter(currEnemy, direction);
                        moveCount = 0;
                        count++;
                        turnBuffer = true;
                        return;
                    }
                }
                else //If enemy cannot move, do nothing
                {
                    count++;
                    turnBuffer = true;
                    return;
                }
            }
            else
            {
                count++;
                turnBuffer = true;
                return;
            }
        }
    }

    private void HandleNextLevel()
    {
        //Next level stuff here
    }

    private void HandleLose()
    {
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