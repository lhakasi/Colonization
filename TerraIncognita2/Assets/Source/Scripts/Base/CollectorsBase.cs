using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
[RequireComponent(typeof(CollectorsSpawner))]
public class CollectorsBase : MonoBehaviour
{
    [SerializeField] private float _price;
    [SerializeField] private AudioSource _insufficientFundsSound;
    [SerializeField] private List<Collector> _collectors;

    private readonly object _queueLock = new object();

    private Scanner _scanner;
    private CollectorsSpawner _collectorsSpawner;
    private FounderOfNewCollectorsBase _founderOfNewCollectorsBase;

    private List<Collider> _foundCrystals;

    private Queue<GameObject> _workQueue;
    private Queue<Collector> _workersQueue;

    public int CollectedCrystals { get; private set; }
    public int CollectorsCount => _collectors.Count;

    private void Awake()
    {
        _scanner = GetComponent<Scanner>();
        _collectorsSpawner = GetComponent<CollectorsSpawner>();
        _founderOfNewCollectorsBase = GetComponent<FounderOfNewCollectorsBase>();

        _foundCrystals = new List<Collider>();
        _workQueue = new Queue<GameObject>();
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
                CollectedCrystals +=
                    collector.GetComponent<CollectorBrainController>().GiveCrystals();
        }
    }

    public void FindCrystals() =>
        _foundCrystals = _scanner.GetFindedCrystals();

    public void CreateNewCollector()
    {
        if (CollectedCrystals >= _collectorsSpawner.GetCollectorPrice())
            _collectors.Add(_collectorsSpawner.CreateNewCollector());
        else
            _insufficientFundsSound.Play();
    }

    public void StartEstablishNewCollectorsBaseCoroutine() =>
        StartCoroutine(EstablishNewCollectorsBase());

    public void AppropriateCollector(Collector collector) =>
        _collectors.Add(collector);

    private void FillCrystalQueue()
    {
        lock (_queueLock)
        {
            _scanner.TurnOff();

            foreach (Collider crystal in _foundCrystals)
                _workQueue.Enqueue(crystal.gameObject);

            StartCoroutine(PlaneWork());
        }
    }

    private void FillWorkersQueue()
    {
        lock (_queueLock)
        {
            foreach (Collector collector in _collectors)
            {
                if (collector.IsWorking == false)
                    _workersQueue.Enqueue(collector);
            }
        }
    }

    private IEnumerator PlaneWork()
    {
        while (_workQueue.Count > 0)
        {
            lock (_queueLock)
            {
                FillWorkersQueue();

                while (_workersQueue.Count > 0)
                {
                    GameObject currentWork = _workQueue.Peek();

                    if (_workQueue.Peek().TryGetComponent<FutureBuilding>(out FutureBuilding futureBuilding))
                    {
                        futureBuilding = currentWork.GetComponent<FutureBuilding>();

                        SetUpWork(futureBuilding.transform.position, true);
                    }
                    else if (_workQueue.Peek().TryGetComponent<Crystal>(out Crystal crystal))
                    {
                        crystal = currentWork.GetComponent<Crystal>();

                        SetUpWork(crystal.transform.position, false);
                    }

                    _workQueue.Dequeue();
                }
            }

            yield return null;
        }
    }

    private IEnumerator EstablishNewCollectorsBase()
    {
        _founderOfNewCollectorsBase.EstablishNewCollectorsBase();

        while (!_founderOfNewCollectorsBase.IsPlaceChosen())
            yield return null;

        while (CollectedCrystals < _price)
            yield return null;

        _workQueue.Enqueue(_founderOfNewCollectorsBase.CollectorsBase.gameObject);

        StartCoroutine(PlaneWork());
    }

    private void SetUpWork(Vector3 target, bool isGiveAwayWorker)
    {
        _workersQueue.Peek().GetComponent<TargetReceiver>().
                    SetTarget(target);

        _workersQueue.Peek().GetComponent<CollectorBrainController>().
            SetWorkingStatus(true);

        if (isGiveAwayWorker)
            _collectors.Remove(_workersQueue.Peek());

        _workersQueue.Dequeue();
    }
}