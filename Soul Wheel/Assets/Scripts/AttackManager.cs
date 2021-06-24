using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackManager : MonoBehaviour
{
    //Checks if attack is valid for given entity at given direction (0 = up, 1 = left, 2 = down, 3 = right)
    public bool IsAttackValid(GameObject entity, int direction, ref GameObject target)
    {
        Vector3 offset = new Vector3(0f, 0f, 0f);
        switch (direction)
        {
            case 0:
                offset = new Vector3(0f, 1f, 0f);
                break;
            case 1:
                offset = new Vector3(-1f, 0f, 0f);
                break;
            case 2:
                offset = new Vector3(0f, -1f, 0f);
                break;
            case 3:
                offset = new Vector3(1f, 0f, 0f);
                break;
        }

        Rigidbody2D rb2d = entity.GetComponent<Rigidbody2D>();
        RaycastHit2D hit = Physics2D.BoxCast(entity.transform.position + offset, new Vector2(0.5f, 0.5f), 0, new Vector2(0, 0));
        if (hit.collider != null)
        {
            if (((entity.tag == "Player" || entity.tag == "Ally") && hit.collider.tag == "Enemy") || (entity.tag == "Enemy" && (hit.collider.tag == "Player" || hit.collider.tag == "Ally")))
            {
                Debug.Log("Attack valid!");
                target = hit.collider.gameObject;
                return true;
            }
        }
        Debug.Log("Attack invalid!");
        return false;
    }

    //Makes given entity basic attack in given direction (0 = up, 1 = left, 2 = down, 3 = right)
    public void AttackTarget(GameObject entity, GameObject target, int direction, Action onAttackComplete)
    {
        target.GetComponent<Stats>().health -= entity.GetComponent<Stats>().damage;
        Debug.Log("Target was damaged for " + entity.GetComponent<Stats>().damage + " hp and is now at " + target.GetComponent<Stats>().health + " hp!");
        onAttackComplete();
    }
}
