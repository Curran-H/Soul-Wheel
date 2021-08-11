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
        Vector2 offset = new Vector2(0f, 0f);
        switch (direction)
        {
            case 0:
                offset = new Vector2(0f, 1f);
                break;
            case 1:
                offset = new Vector2(-1f, 0f);
                break;
            case 2:
                offset = new Vector2(0f, -1f);
                break;
            case 3:
                offset = new Vector2(1f, 0f);
                break;
        }
        
        Rigidbody2D rb2d = entity.GetComponent<Rigidbody2D>();
        RaycastHit2D hit = Physics2D.BoxCast(rb2d.position + offset, new Vector2(0.5f, 0.5f), 0, new Vector2(0, 0));
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
        Rigidbody2D rb2d = entity.GetComponent<Rigidbody2D>();

        //Calculate offset and move given entity
        Vector2 offset = new Vector2(0f, 0f);
        switch (direction)
        {
            case 0:
                offset = new Vector2(0f, 1f);
                break;
            case 1:
                offset = new Vector2(-1f, 0f);
                break;
            case 2:
                offset = new Vector2(0f, -1f);
                break;
            case 3:
                offset = new Vector2(1f, 0f);
                break;
        }

        //Move entity 1/60th of given spaces (This will be repeated 60 times to move the full number of spaces, more spaces means more speed to avoid drawn-out movements of 3+ spaces
        entity.transform.position = rb2d.position + offset * spaces * (1.0f/60.0f);

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
        Rigidbody2D rb2d = entity.GetComponent<Rigidbody2D>();

        //Ensure entity is still centered properly
        entity.transform.position = new Vector2(Mathf.Round(rb2d.position.x), Mathf.Round(rb2d.position.y));

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