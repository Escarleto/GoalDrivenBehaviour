using UnityEngine;

public class SpawnPatients : MonoBehaviour
{
    [SerializeField] GameObject patient;
    [SerializeField] float spawnInterval = 3.5f;

    void Start()
    {
        InvokeRepeating("Spawn", 2, spawnInterval);
    }

    void Spawn()
    {
        Instantiate(patient, transform.position, Quaternion.identity);
    }
}
