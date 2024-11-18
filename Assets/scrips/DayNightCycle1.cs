using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle1 : MonoBehaviour
{
    public GameObject mainLight;

    public float timePassing = 0.1f;

    public float time;

    private void Update()
    {
        time += timePassing * Time.deltaTime;
        mainLight.transform.rotation = Quaternion.Euler(time, 0, 0);
    }
}
