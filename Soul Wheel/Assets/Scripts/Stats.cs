using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public string ID;

    public float health;
    public float maxHealth;
    public HealthbarBehavior Healthbar;

    public double damage;
    public double speed;
    public double defense;

    private void Start()
    {
        health = maxHealth;
        Healthbar.SetHealth(health, maxHealth);
    }

    public void TakeHit(float damage)
    {
        health -= damage;
        Healthbar.SetHealth(health, maxHealth);

        if (health <= 0 && ID != "Player")
        {
            Destroy(gameObject);
        }
    }
}
