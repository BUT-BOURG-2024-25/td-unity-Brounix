using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Vector3 offset;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Le joueur n'est pas assigné dans le script CameraFollow.");
        }
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;

            transform.LookAt(player);
        }
    }
}
