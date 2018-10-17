using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float minSpeed;
    public float maxSpeed;
    public float turnSpeed;            

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
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
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

        _rigidbody.velocity = transform.forward * speed;
    }


    private void Turn()
    {
        float turn = _turnInput * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
    }
}