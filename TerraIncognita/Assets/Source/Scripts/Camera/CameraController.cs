using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private BoxCollider _movementBounds;
    [SerializeField] private float _speed;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * _speed * Time.deltaTime;
        
        transform.Translate(movement);
                
        Vector3 currentPosition = transform.position;

        currentPosition.x = Mathf.Clamp(currentPosition.x, _movementBounds.bounds.min.x, _movementBounds.bounds.max.x);
        currentPosition.y = Mathf.Clamp(currentPosition.y, _movementBounds.bounds.min.y, _movementBounds.bounds.max.y);
        currentPosition.z = Mathf.Clamp(currentPosition.z, _movementBounds.bounds.min.z, _movementBounds.bounds.max.z);
        
        transform.position = currentPosition;
    }
}