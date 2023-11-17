using System.Collections;
using UnityEngine;

public class CrystalsSpawner : MonoBehaviour
{
    [SerializeField] private CrystalZone[] _spawnZones;
    [SerializeField] private float _maxCount;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _minScale;
    [SerializeField] private float _maxScale;

    private float _count = 0;
    
    private float _minAngle = 0;
    private float _maxAngle = 360;
    
    private bool _isSpawning = true;

    public float SpawnRadius => _spawnRadius;

    private void Start() =>    
        StartCoroutine(Spawn());

    private void Update()
    {
        if (_isSpawning == false)        
            StopCoroutine(Spawn());        
    }

    private void CreateCrystal()
    {
        if (_maxCount <= _count)        
            _isSpawning = false;        

        CrystalZone spawnZone = _spawnZones[Random.Range(0, _spawnZones.Length)];

        float randomAngle = Random.Range(_minAngle, _maxAngle);
        
        Vector3 randomPosition = Random.insideUnitSphere * _spawnRadius;
        randomPosition.y = 0f;

        Crystal crystal = Instantiate
            (spawnZone.CrystalTemplate, spawnZone.transform.position + randomPosition, Quaternion.Euler(0f, randomAngle, 0f));

        float randomScale = Random.Range(_minScale, _maxScale);

        crystal.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        crystal.transform.SetParent(spawnZone.transform);

        _count++;
    }

    private IEnumerator Spawn()
    {
        while (_isSpawning)
        {
            CreateCrystal();

            yield return null;
        }
    }
}