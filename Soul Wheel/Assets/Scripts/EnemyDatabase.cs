using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Database", menuName = "Databases/Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    public List<Enemy> enemyTypes;
}
