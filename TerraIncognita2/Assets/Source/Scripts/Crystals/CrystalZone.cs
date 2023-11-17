using UnityEngine;

public class CrystalZone : MonoBehaviour
{
    [SerializeField] private Crystal _crystalTemplate;
    [SerializeField] private CrystalsSpawner _crystalsSpawner;

    public Crystal CrystalTemplate => _crystalTemplate;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _crystalsSpawner.SpawnRadius);
    }
}