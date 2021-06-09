using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    GameManager GameManager;
    public GameManager.GameState state;

    public GameObject[] allCharacters;
    public GameObject[] sortedCharacters;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        SpawnEnemies(); //IMPLEMENTATION REQUIRED
        SpeedSort();
    }

    // Update is called once per frame
    void Update()
    {
        if(count > sortedCharacters.Length)
        {
            count = 0;
            SpeedSort();
        }
        if (sortedCharacters[count].tag == "Player")
            state = GameManager.GameState.PlayerTurn;
        else if (sortedCharacters[count].tag == "Ally")
            state = GameManager.GameState.AllyTurn;
        else if (sortedCharacters[count].tag == "Enemy")
            state = GameManager.GameState.EnemyTurn;
    }

    public void SpeedSort()
    {
        allCharacters = GameObject.FindGameObjectsWithTag("Characters");
        int i, j;
        double key;
        for (i = 1; i < allCharacters.Length; i++)
        {
            key = allCharacters[i].GetComponent<Stats>().speed;
            j = i - 1;

            while (j >= 0 && allCharacters[j].GetComponent<Stats>().speed > key)
            {
                allCharacters[j + 1].GetComponent<Stats>().speed = allCharacters[j].GetComponent<Stats>().speed;
                j -= 1;
            }
            allCharacters[j + 1].GetComponent<Stats>().speed = key;
        }

        sortedCharacters = allCharacters;
    }

    private void SpawnEnemies() //IMPLEMENTATION REQUIRED
    {

    }
}