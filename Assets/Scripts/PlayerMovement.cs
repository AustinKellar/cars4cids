using UnityEngine;
using LibPDBinding;

public class PlayerMovement : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    public float turnSpeed;
    public float maxFuel;
    public float fuel;

    private float _speed;
    private float _startingSpeed;

    private Vector3 _checkpointPosition;
    private Quaternion _checkpointRotation;

    private Rigidbody _rigidbody;

    public float speed // speed is a readonly property. Other scripts won't be able to change it directly
    {
        get { return _speed; }
    }

    private bool _moveForwardPressed
    {
        get { return Input.GetAxis("Vertical") == 1; }
    }

    private bool _moveBackwardPressed
    {
        get { return Input.GetAxis("Vertical") == -1; }
    }

    private float _turnInput
    {
        get { return Input.GetAxis("Horizontal"); }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _checkpointPosition = transform.position;
        _checkpointRotation = transform.rotation;
        _startingSpeed = 0;
        _speed = _startingSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Wall"))
        {
            // freeze physics while the car resets so that it doesn't turn immediately after resetting
            ResetToCheckpoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Checkpoint"))
        {
            CheckpointScript checkpoint = other.gameObject.GetComponent<CheckpointScript>();
            _checkpointPosition = checkpoint.CheckpointPosition;
            _checkpointRotation.eulerAngles = checkpoint.CheckpointRotation;
        }
    }

    private void Update()
    {
        LibPD.SendFloat("velocity", speed);
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void ResetToCheckpoint()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        transform.position = _checkpointPosition;
        transform.rotation = _checkpointRotation;
        _speed = _startingSpeed;
        _rigidbody.constraints = RigidbodyConstraints.None;
    }

    private void Move()
    {
        if (_speed < minSpeed) // if you are moving slower than the min speed, speed increases slowly to minSpeed
        {
            _speed += 0.3f;
        }

        if (_moveForwardPressed && _speed < maxSpeed) // if pressing forward, increase speed
        {
            _speed++;
        }

        if (_moveBackwardPressed && _speed > minSpeed) // if pressing back, decrease speed
        {
            _speed--;
        }

        Vector3 newVelocity = transform.forward * _speed;
        newVelocity.y = _rigidbody.velocity.y;

        _rigidbody.velocity = newVelocity;
    }


    private void Turn()
    {
        float turn = _turnInput * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
    }
}