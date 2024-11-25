using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private Transform teleportDestination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleportDestination.position;
        }
    }
}
