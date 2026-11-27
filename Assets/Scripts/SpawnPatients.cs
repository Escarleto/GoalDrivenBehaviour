using UnityEngine;

public class SpawnPatients : MonoBehaviour
{
    [SerializeField] GameObject patientPrefab;
    [SerializeField] int numPatients;

    void Start()
    {
        for (int i = 0; i < numPatients; i++)
        {
            Instantiate(patientPrefab, this.transform.position, Quaternion.identity);
        }
        Invoke("SpawnPatient", 6.5f);
    }

    void SpawnPatient()
    {
        Instantiate(patientPrefab, transform.position, Quaternion. identity);
        Invoke("SpawnPatient", Random.Range(2, 10));
    }
}
