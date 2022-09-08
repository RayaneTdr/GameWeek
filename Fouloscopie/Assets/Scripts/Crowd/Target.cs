using UnityEngine;

public class Target : MonoBehaviour
{

    private void OnEnable() => WaveManager.Instance.targets.Add(this);

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dummy"))
            other.GetComponent<Dummy>().SetEntryAsDestination();
    }

}
