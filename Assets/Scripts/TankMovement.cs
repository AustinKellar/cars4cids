using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float speed;
    public float minSpeed;
    public float maxSpeed;
    public float turnSpeed;            

    private Rigidbody _rigidbody;             
    private float _movementInput;         
    private float _turnInput;            

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        _movementInput = Input.GetAxis("Vertical");
        _turnInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }


    private void Move()
    {
        if (_movementInput == 1 && speed < maxSpeed) // if pressing forward, increase speed
        {
            speed++;   
        }
        else if (_movementInput == -1 && speed > minSpeed) // if pressing back, decrease speed
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