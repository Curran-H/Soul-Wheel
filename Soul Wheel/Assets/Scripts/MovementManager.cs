using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementManager : MonoBehaviour
{
    public AnimationManager animationManager;

    private void Awake()
    {
        animationManager = FindObjectOfType<AnimationManager>();
    }

    //Checks if movement is valid for given entity at given direction (0 = North, 1 = West, 2 = South, 3 = East)
    public bool IsMovementValid(GameObject entity, int direction)
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
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Player" || hit.collider.tag == "Ally" || hit.collider.tag == "Enemy")
            {
                //Debug.Log("Movement invalid!");
                return false;
            }
        }
        //Debug.Log("Movement valid!");
        return true;
    }

    //Moves given entity in given direction (0 = North, 1 = West, 2 = South, 3 = East) for given number of spaces
    public void MoveCharacter(GameObject entity, int direction, int spaces, Action onMovementComplete)
    {
        //Calculate offset and move given entity
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

        //Move entity 1/60th of given spaces (This will be repeated 60 times to move the full number of spaces, more spaces means more speed to avoid drawn-out movements of 3+ spaces
        entity.transform.position += offset * spaces * (1.0f/300.0f);

        //Begin playing animation if not already playing
        string state = "";
        switch (direction)
        {
            case 0:
                state = "Walk_North";
                break;
            case 1:
                state = "Walk_West";
                break;
            case 2:
                state = "Walk_South";
                break;
            case 3:
                state = "Walk_East";
                break;
        }
        animationManager.ChangeAnimationState(entity, state);

        //Finish movement
        onMovementComplete();
    }

    //Centers given entity in closest square and changes their animation state to idle in specified direction (0 = North, 1 = West, 2 = South, 3 = East), if applicable
    public void CenterCharacter(GameObject entity, int direction)
    {
        //Ensure entity is still centered properly
        entity.transform.position = new Vector3(Mathf.Round(entity.transform.position.x), Mathf.Round(entity.transform.position.y), 0f);

        switch (direction)
        {
            case 0:
                animationManager.ChangeAnimationState(entity, "Idle_North");
                break;
            case 1:
                animationManager.ChangeAnimationState(entity, "Idle_West");
                break;
            case 2:
                animationManager.ChangeAnimationState(entity, "Idle_South");
                break;
            case 3:
                animationManager.ChangeAnimationState(entity, "Idle_East");
                break;
        }
    }
}