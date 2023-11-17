using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private BoxCollider _movementBounds;
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _speedMultiplier;

    private float _currentSpeed;

    private void Awake()
    {
        _currentSpeed = _baseSpeed;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * _currentSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            _currentSpeed = _baseSpeed * _speedMultiplier;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            _currentSpeed = _baseSpeed;
        
        transform.Translate(movement);
                
        Vector3 currentPosition = transform.position;

        currentPosition.x = Mathf.Clamp(currentPosition.x, _movementBounds.bounds.min.x, _movementBounds.bounds.max.x);
        currentPosition.y = Mathf.Clamp(currentPosition.y, _movementBounds.bounds.min.y, _movementBounds.bounds.max.y);
        currentPosition.z = Mathf.Clamp(currentPosition.z, _movementBounds.bounds.min.z, _movementBounds.bounds.max.z);
        
        transform.position = currentPosition;
    }
}