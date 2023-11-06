using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanningRadius;
    [SerializeField] private LayerMask _crystalLayer;

    public bool IsFinded { get; private set; }

    private void Awake() =>    
        IsFinded = false;    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _scanningRadius);
    }

    public List<Collider> GetFindedCrystals()
    {
        List<Collider> foundCrystals = new List<Collider>();

        foundCrystals.AddRange(Physics.OverlapSphere
            (transform.position, _scanningRadius, _crystalLayer).ToList());

        if (foundCrystals.Count > 0)
            IsFinded = true;
        else
            IsFinded = false;

        return foundCrystals;
    }

    public void TurnOff() =>
        IsFinded = false;
}