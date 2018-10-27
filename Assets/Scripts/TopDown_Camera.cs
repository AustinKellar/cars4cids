using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDown_Camera : MonoBehaviour {
    
    
    #region variables
    [SerializeField]
    private Transform camTarget;

    [SerializeField]
    private float height;

    [SerializeField]
    private float distance;

    [SerializeField]
    private float angle;

    [SerializeField]
    private float smoothSpeed;

    [SerializeField]
    private float zoomHeightSpeed;

    [SerializeField]
    private float zoomDistanceSpeed;

    [SerializeField]
    private float minZoom;

    [SerializeField]
    private float maxZoom;

    private Vector3 refVelocity;
    #endregion

    #region unityMethods
    // Use this for initialization
    void Start () {
        HandleCamera();
	}

    // Update is called once per frame
    private void Update()
    {
        HandleNavigatorClicks();
    }
    void FixedUpdate () {
        zoom();
        HandleCamera();
	}
    #endregion

    #region helperMethod
    void HandleCamera()
    {
        if (!camTarget)
        {
            return;
        }

        Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);

        Vector3 rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;

        Vector3 flatTargetPosition = camTarget.position;

        flatTargetPosition.y = 0f;

        Vector3 finalPosition = flatTargetPosition + rotatedVector;

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
        transform.LookAt(flatTargetPosition);
    }

    void zoom()
    {
        height -= Input.GetAxis("Mouse ScrollWheel") * zoomHeightSpeed;
        height = Mathf.Clamp(height, minZoom, maxZoom);
        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomDistanceSpeed;
        distance = Mathf.Clamp(distance, minZoom, maxZoom);
    }

    void HandleNavigatorClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var camera = Camera.allCameras[1];
            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Destroyable Wall")
                {
                    Destroy(GameObject.Find(hit.transform.name));
                }
            }
        }
    }
    #endregion
}