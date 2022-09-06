using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
#region SINGLETON
    GameManager instance = null;
    public GameManager Instance => instance;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this);
    }
#endregion


}
