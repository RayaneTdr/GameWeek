using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DummiesCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    void Update()
    {
        int count = WaveManager.Instance.dummies.Count;
        if (count < 10)
            text.text = "0+count";
        else
            text.text = count.ToString();
    }
}
