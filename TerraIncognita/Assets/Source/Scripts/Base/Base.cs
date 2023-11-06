using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private Collector[] _collectors;

    private Scanner _scanner;
    private List<Collider> _foundCrystals;

    private Queue<Collider> _crystalQueue;
    private Queue<Collector> _workersQueue;

    public int CollectedCrystals { get; private set; }

    private void Awake()
    {
        _scanner = GetComponent<Scanner>();

        _foundCrystals = new List<Collider>();
        _crystalQueue = new Queue<Collider>();
        _workersQueue = new Queue<Collector>();

        CollectedCrystals = 0;
    }       

    private void Update()
    {
        if (_scanner.IsFinded)        
            FillCrystalQueue();        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Collector collector))
        {
            if (collector.IsCrystalOnBoard)
                CollectedCrystals += collector.GiveCrystals();
        }
    }

    public void FindCrystals() =>    
        _foundCrystals = _scanner.GetFindedCrystals();

    private void FillCrystalQueue()
    {
        _scanner.TurnOff();

        foreach (Collider crystal in _foundCrystals)
            _crystalQueue.Enqueue(crystal);

        StartCoroutine(CollectCrystals());
    }    

    private void FillWorkersQueue()
    {
        foreach (Collector collector in _collectors)
        {
            if (collector.IsWorking == false)
                _workersQueue.Enqueue(collector);
        }
    }

    private IEnumerator CollectCrystals()
    {
        while (_crystalQueue.Count > 0)
        {
            FillWorkersQueue();

            while (_crystalQueue.Count > 0 && _workersQueue.Count > 0)
            {
                Crystal crystal = _crystalQueue.Dequeue().GetComponent<Crystal>();

                _workersQueue.Peek().GetComponent<TargetReceiver>().SetTarget(crystal);
                _workersQueue.Peek().SetWorkingStatus(true);
                _workersQueue.Dequeue();
            }

            yield return null;
        }
    }
}