using UnityEngine;

public class CollectorsSpawner : MonoBehaviour
{
    [SerializeField] private Collector _collectorBlueprint;
    [SerializeField] private CollectorsHolder _collectorsHolder;
    [SerializeField] private AudioSource _newCollectorWasCreatedSound;

    public Collector CreateNewCollector()
    {
        Collector collector = Instantiate
            (_collectorBlueprint, transform.position, Quaternion.identity);

        collector.transform.SetParent(_collectorsHolder.transform);
        collector.GetComponent<TargetReceiver>().SetBase(_collectorsHolder.GetComponentInParent<CollectorsBase>());

        _newCollectorWasCreatedSound.Play();

        return collector;
    }

    public int GetCollectorPrice() => _collectorBlueprint.Price;    
}