using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinematicFin : MonoBehaviour
{

    public GameObject endPrinter;

    public void StartPrinter()
    {
        endPrinter.SetActive(true);
        endPrinter.GetComponentInChildren<TicketRenderer>().Display();
    }
}
