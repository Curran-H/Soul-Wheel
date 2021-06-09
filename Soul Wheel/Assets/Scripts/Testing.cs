using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private void CreateGrid()
    {

    }

    private void Start()
    {
        GridMap grid = new GridMap(5, 5, 1f);
    }
}
