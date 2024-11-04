using UnityEngine;

public class DestroyAllCubesOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject[] cubes = GameObject.FindGameObjectsWithTag("Breackable");
            
            foreach (GameObject cube in cubes)
            {
                Destroy(cube);
            }
        }
    }
}
