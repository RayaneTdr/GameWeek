using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Promotion : MonoBehaviour
{
    public void DestroyStand() 
    {
        WaveManager.Instance.ResetDistraction(transform.root.gameObject);

    }
}
