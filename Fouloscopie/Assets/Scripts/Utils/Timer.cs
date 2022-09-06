using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public Timer(float _max) { max = _max; }

    // ------ Public
    public float max = 1f;

    // ------ Timer Logic
    float time   = 0f;
    bool started = false;
    bool paused  = false;
    // ------ Getters
    public bool Started => started;
    public bool Paused => paused;
    public float Time => time;
    public float Remaining => max - time;

    public bool Tick(float dt) // return true if the timer ended
    {
        if (!started) return false;
        if (!paused)  time += dt;

        return Remaining <= 0f;
    }

    public void Start()
    {
        started = true;
        paused = false;
        time = 0f;
    }

    public void Stop()   => started = false;
    public void Reset()  => time = 0f;
    public void Toggle() => paused = !paused;

}
