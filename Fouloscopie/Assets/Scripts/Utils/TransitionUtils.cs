using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionUtils : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Animator>().SetTrigger("backward");
    }
}
