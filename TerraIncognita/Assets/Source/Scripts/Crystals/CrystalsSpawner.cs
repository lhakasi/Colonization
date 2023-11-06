using System.Collections;
using UnityEngine;

public class CrystalsSpawner : MonoBehaviour
{
    [SerializeField] private CrystalZone[] _spawnZones;
    [SerializeField] private float _delay;
    [SerializeField] private float _spawnRadius;

    private float _minAngle = 0;
    private float _maxAngle = 360;

    private bool _isSpawning = true;

    public float SpawnRadius => _spawnRadius;

    private void Start() =>    
        StartCoroutine(Spawn());

    private void CreateCrystal()
    {
        CrystalZone spawnZone = _spawnZones[Random.Range(0, _spawnZones.Length)];

        float randomAngle = Random.Range(_minAngle, _maxAngle);
        
        Vector3 randomPosition = Random.insideUnitSphere * _spawnRadius;
        randomPosition.y = 0f;

        Crystal crystal = Instantiate
            (spawnZone.CrystalTemplate, spawnZone.transform.position + randomPosition, Quaternion.Euler(0f, randomAngle, 0f));
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (_isSpawning)
        {
            yield return delay;

            CreateCrystal();
        }
    }
}