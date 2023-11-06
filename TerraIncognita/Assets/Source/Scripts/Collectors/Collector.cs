using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Trunk _trunk;

    private AudioSource _dischargeSound;

    public bool IsWorking { get; private set; }
    public bool IsCrystalOnBoard { get; private set; }

    private void Awake()
    {
        _dischargeSound = GetComponent<AudioSource>();

        IsCrystalOnBoard = false;
        IsWorking = false;
    }

    public void SetWorkingStatus(bool status) =>
        IsWorking = status;

    public void Collect(Crystal crystal)
    {
        if (IsCrystalOnBoard)
            return;

        crystal.transform.position = _trunk.transform.position;
        crystal.transform.SetParent(_trunk.transform);
        crystal.SetColliderActive(false);

        IsCrystalOnBoard = true;
    }

    public int GiveCrystals()
    {
        Crystal crystal = GetComponentInChildren<Crystal>();

        Destroy(crystal.gameObject);

        _dischargeSound.Play();

        SetCrystalInTrunkStatus(false);
        SetWorkingStatus(false);

        return crystal.RecourceCapacity;
    }

    private void SetCrystalInTrunkStatus(bool status) =>
        IsCrystalOnBoard = status;
}