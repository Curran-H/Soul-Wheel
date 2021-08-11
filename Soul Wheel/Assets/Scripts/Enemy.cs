using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Characters/Enemies")]   
public class Enemy : ScriptableObject
{
    public string enemyID;
    public string enemyName;
    public double baseHealth;
    public double baseDamage;
    public double baseSpeed;
    public double baseDefense;
    public Sprite enemySprite;
}