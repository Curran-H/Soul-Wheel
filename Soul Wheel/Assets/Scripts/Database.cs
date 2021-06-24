using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Database : MonoBehaviour
{
    public EnemyDatabase enemies;

    private static Database instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Enemy GetEnemyByID(string ID)
    {
        return enemies.enemyTypes.FirstOrDefault(i => i.enemyID == ID);
    }
}
