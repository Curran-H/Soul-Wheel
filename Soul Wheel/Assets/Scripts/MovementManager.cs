using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
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

        /*Collider[] colliders = Physics.OverlapBox(entity.transform.position + offset, entity.transform.GetComponent<BoxCollider2D>().size / 2);
        for(int colCount = 0; colCount < colliders.Length; colCount++)
        {
            if (colliders[colCount].tag == "Wall" || colliders[colCount].tag == "Player" || colliders[colCount].tag == "Ally" || colliders[colCount].tag == "Enemy")
                return false;
        }
        return true;*/
    }

    //Moves given entity in given direction (0 = up, 1 = left, 2 = down, 3 = right) for given number of spaces
    public void MoveCharacter(GameObject entity, int direction, int spaces)
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
        entity.transform.position += (offset * spaces);
    }
}
