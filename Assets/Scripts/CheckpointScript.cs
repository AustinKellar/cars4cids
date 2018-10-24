using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private Vector3 _checkpointPosition;
    private Vector3 _checkpointRotation;

    public Vector3 CheckpointPosition
    {
        get { return _checkpointPosition; }
        set { _checkpointPosition = value; }
    }

    public Vector3 CheckpointRotation
    {
        get { return _checkpointRotation; }
        set { _checkpointRotation = value; }
    }

    private void Start()
    {
        _checkpointPosition = gameObject.transform.position;
        _checkpointRotation = gameObject.transform.rotation.eulerAngles;
        
    }
}
