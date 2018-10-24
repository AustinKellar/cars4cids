using UnityEngine;

using LibPDBinding;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float minSpeed;
    public float maxSpeed;
    public float turnSpeed;
    public float maxFuel;
    public float fuel;

    private Vector3 _checkpointPosition;
    private Quaternion _checkpointRotation;

    private Rigidbody _rigidbody;

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Wall"))
        {
            // freeze physics while the car resets so that it doesn't turn immediately after resetting
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
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
        transform.position = _checkpointPosition;
        transform.rotation = _checkpointRotation;
        speed = 35f;
        // resume physics
        _rigidbody.constraints = RigidbodyConstraints.None;
    }

    private void Move()
    {
        if (_moveForwardPressed && speed < maxSpeed) // if pressing forward, increase speed
        {
            speed++;   
        }

        if (_moveBackwardPressed && speed > minSpeed) // if pressing back, decrease speed
        {
            speed--;
        }

        Vector3 newVelocity = transform.forward * speed;
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