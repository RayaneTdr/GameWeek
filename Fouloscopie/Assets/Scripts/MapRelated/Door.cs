using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Door : MonoBehaviour
{

    Animator anim;

    List<Dummy> dummies = new List<Dummy>();

    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Dummy dummy)) 
        {
            dummies.Add(dummy);
            RefreshAnim();
        }
    
    }   

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Dummy dummy)) 
        {
            dummies.Remove(dummy);
            RefreshAnim();
        }
    }

    public void RefreshAnim() 
    {
        if (dummies.Count > 0)
            anim.SetTrigger("Open");
        else
            anim.SetTrigger("Close");
    }
}
