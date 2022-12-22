using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public int cycleMinutes = 1;
    private int degreesForSecons = 0;
    private float updateDegrees;

    void Start()
    {
        degreesForSecons = 360 / (cycleMinutes * 60);
    }

    private void Update()
    {
        updateDegrees = degreesForSecons * Time.deltaTime;

        transform.Rotate(updateDegrees, 0, 0);
    }
}
