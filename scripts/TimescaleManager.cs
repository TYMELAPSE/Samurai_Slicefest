using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimescaleManager : MonoBehaviour
{
    public float slowdown = 0.1f;
    public float slowdownAtDeath = 0.02f;
    public void SlowdownWhenCharging()
    {
        Time.timeScale = slowdown;
        Time.fixedDeltaTime = slowdown * 0.02f;
    }

    public void SlowdownWhenDead()
    {
        Time.timeScale = slowdownAtDeath;
        Time.fixedDeltaTime = slowdownAtDeath * 0.02f;
    }

    public void ResetTimescale()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 1f * 0.02f;
    }
}
