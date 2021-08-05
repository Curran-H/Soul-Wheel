using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;
    public TurnManager turnManager;
    public AttackManager attackManager;
    public MovementManager movementManager;
    public Database enemyDB;
    public GameObject enemyBase;
    private Tilemap map;
    public List<Vector3> tileLocations;

    void Awake()
    {
        Instance = this;
        GameObject.Instantiate(turnManager, gameObject.transform.parent);
        GameObject.Instantiate(attackManager, gameObject.transform.parent);
        GameObject.Instantiate(movementManager, gameObject.transform.parent);
        map = GameObject.FindGameObjectWithTag("Floor").GetComponent<Tilemap>();
    }

    void Start()
    {
        StoreTiles();
        SpawnEnemy("SampleEnemy", 3);
        SpawnEnemy("SampleEnemy", 5);
        SpawnEnemy("SampleEnemy", 10);
    }

    public void StoreTiles()
    {
        for(int countX = -100; countX < 100; countX++)
        {
            for(int countY = -100; countY < 100; countY++)
            {
                TileBase currentTile = map.GetTile(new Vector3Int(countX, countY, 0));
                if (currentTile != null)
                {
                    if (currentTile.name == "Bad Tileset_0")
                    {
                        tileLocations.Add(new Vector3(countX, countY, 0));
                    }
                }
            }
        }
    }

    public void SpawnEnemy(string enemyID, int level)
    {
        //First, spawn basic enemy at random location (in map, but randomly around player)
        Enemy baseEnemy = enemyDB.GetEnemyByID(enemyID);
        bool spawnValid = false;
        Vector3 randomPos = new Vector3(0, 0, 0);
        while(!spawnValid)
        {
            Vector3 currentTile = tileLocations[Random.Range(0, tileLocations.Count)];
            RaycastHit2D hit = Physics2D.BoxCast(currentTile, new Vector2(0.5f, 0.5f), 0, new Vector2(0, 0));
            if (hit.collider == null)
            {
                spawnValid = true;
                randomPos = currentTile;
            }
        }
        Object.Instantiate(enemyBase, randomPos, new Quaternion(0f, 0f, 0f, 0f));

        //Then, set enemy's stats based on their base stats from the stats sheet and level growth (actual growth algorithm TBD)
        enemyBase.name = enemyID;
        Stats enemyStats = enemyBase.GetComponent<Stats>();
        enemyStats.ID = baseEnemy.enemyID;
        enemyStats.health = baseEnemy.baseHealth + (level * 5);
        enemyStats.damage = baseEnemy.baseDamage + (level * 2);
        enemyStats.speed = baseEnemy.baseSpeed + (level * 2);
        enemyStats.defense = baseEnemy.baseDefense + (level * 2);
        enemyStats.GetComponentInChildren<SpriteRenderer>().sprite = baseEnemy.enemySprite;
        enemyStats.GetComponentInChildren<Animator>().runtimeAnimatorController = baseEnemy.animController;
    }
}