using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementManager : MonoBehaviour
{
    public TurnManager turnManager;

    void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    //Checks if movement is valid for given entity at given direction (0 = up, 1 = left, 2 = down, 3 = right)
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
                Debug.Log("Movement invalid!");
                return false;
            }
        }
        Debug.Log("Movement valid!");
        return true;
    }

    //Moves given entity in given direction (0 = up, 1 = left, 2 = down, 3 = right) for given number of spaces
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
        entity.transform.position += offset * spaces;

        //Ensure entity is still centered properly
        entity.transform.position = new Vector3(Mathf.Round(entity.transform.position.x), Mathf.Round(entity.transform.position.y), 0f);

        onMovementComplete();
    }
}