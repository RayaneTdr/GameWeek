using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Repulsive : MonoBehaviour
{

    [SerializeField] SphereCollider col;
    [SerializeField] float time = 1f;
    [SerializeField] float radius = 1f;


    public void Drop(Vector3 position)
    {
        transform.position = position;

        col = GetComponent<SphereCollider>();
        col.radius = radius;
        AstarPath.active.Scan();
        Invoke("ResetAndDestroy", time);
    }


    public void Abort()
    {
        Destroy(gameObject);
    }


    void ResetAndDestroy() 
    {
        col.radius = 0f;
        AstarPath.active.Scan();
        Destroy(gameObject);
    }
}
