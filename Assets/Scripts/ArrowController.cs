using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public GameObject goal;
	
	void Update ()
    {
        transform.LookAt(goal.transform);
        Quaternion q = new Quaternion();
        q.eulerAngles = new Vector3(0f, 0f, 0f) + gameObject.transform.rotation.eulerAngles;
        gameObject.transform.rotation = q;
    }
}
