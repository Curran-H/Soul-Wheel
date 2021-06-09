using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public double baseSpeed;
    public double speed;

    // Start is called before the first frame update
    void Start()
    {
        baseSpeed = 5;
        speed = baseSpeed * Random.Range(0.5f, 1.5f);
    }

    public double GetSpeed()
    {
        return speed;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
