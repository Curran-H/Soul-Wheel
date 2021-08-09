using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFinishCheck : MonoBehaviour
{
    public TurnManager turnManager;

    public void AttackFinished()
    {
        turnManager = FindObjectOfType<TurnManager>();
        turnManager.attackFinished = true;
    }
}
