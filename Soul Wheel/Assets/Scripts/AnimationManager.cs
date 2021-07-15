using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator animator;
    private string currentState;

    public static AnimationManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ChangeAnimationState(GameObject entity, string newState)
    {
        newState = entity.GetComponent<Stats>().ID + "_" + newState;
        animator = entity.GetComponent<Animator>();

        //Stop animation from playing over itself
        if (currentState == newState)
            return;

        //Play animation
        animator.Play(newState);

        //Reassign current animation state to new state
        currentState = newState;
    }
}