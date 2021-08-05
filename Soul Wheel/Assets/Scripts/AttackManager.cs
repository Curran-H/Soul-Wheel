using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackManager : MonoBehaviour
{
    public AnimationManager animationManager;

    private void Awake()
    {
        animationManager = FindObjectOfType<AnimationManager>();
    }

    //Checks if attack is valid for given entity at given direction (0 = North, 1 = West, 2 = South, 3 = East)
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

    //Makes given entity basic attack in given direction (0 = North, 1 = West, 2 = South, 3 = East)
    public void AttackTarget(GameObject entity, GameObject target, int direction)
    {
        string state;
        switch (direction)
        {
            case 0:
                state = "Attack_North";
                break;
            case 1:
                state = "Attack_West";
                break;
            case 2:
                state = "Attack_South";
                break;
            case 3:
                state = "Attack_East";
                break;
            default:
                state = "Attack_South";
                break;
        }
        animationManager.ChangeAnimationState(entity, state);
        Stats entityStats = entity.GetComponent<Stats>();
        Stats targetStats = target.GetComponent<Stats>();
        //targetStats.health -= (float)entityStats.damage;
        targetStats.TakeHit((float)entityStats.damage);
        Debug.Log("Target was damaged for " + entityStats.damage + " hp and is now at " + targetStats.health + " hp!");
    }

    public void AttackTargetIfValid(GameObject entity, int direction)
    {
        string state = "";
        Vector3 offset = new Vector3(0f, 0f, 0f);
        switch (direction)
        {
            case 0:
                state = "Attack_North";
                offset = new Vector3(0f, 1f, 0f);
                break;
            case 1:
                state = "Attack_West";
                offset = new Vector3(-1f, 0f, 0f);
                break;
            case 2:
                state = "Attack_South";
                offset = new Vector3(0f, -1f, 0f);
                break;
            case 3:
                state = "Attack_East";
                offset = new Vector3(1f, 0f, 0f);
                break;
        }

        animationManager.ChangeAnimationState(entity, state);
        Rigidbody2D rb2d = entity.GetComponent<Rigidbody2D>();
        RaycastHit2D hit = Physics2D.BoxCast(entity.transform.position + offset, new Vector2(0.5f, 0.5f), 0, new Vector2(0, 0));
        if (hit.collider != null)
        {
            if (((entity.tag == "Player" || entity.tag == "Ally") && hit.collider.tag == "Enemy") || (entity.tag == "Enemy" && (hit.collider.tag == "Player" || hit.collider.tag == "Ally")))
            {
                GameObject target = hit.collider.gameObject;
                Stats entityStats = entity.GetComponent<Stats>();
                Stats targetStats = target.GetComponent<Stats>();
                //targetStats.health -= (float)entityStats.damage;
                targetStats.TakeHit((float)entityStats.damage);
                Debug.Log("Target was damaged for " + entityStats.damage + " hp and is now at " + targetStats.health + " hp!");
            }
        }
    }

    public void IsAttackFinished(GameObject entity, int direction, Action onAttackComplete)
    {
        string state = "";
        Stats entityStats = entity.GetComponent<Stats>();
        AnimatorStateInfo info = entity.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0);

        switch (direction)
        {
            case 0:
                state = entityStats.ID + "_Idle_North";
                break;
            case 1:
                state = entityStats.ID + "_Idle_West";
                break;
            case 2:
                state = entityStats.ID + "_Idle_South";
                break;
            case 3:
                state = entityStats.ID + "_Idle_East";
                break;
        }

        //If entity is in idle state, attack is complete
        if (info.IsName(state))
            onAttackComplete();
    }
}
