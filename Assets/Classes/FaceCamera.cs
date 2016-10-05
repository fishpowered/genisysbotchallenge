using UnityEngine;
using System.Collections;

/// <summary>
/// Used for making world space GUI elements always face the camera
/// </summary>
public class FaceCamera : MonoBehaviour {

    public Camera cameraToFace;

    void Start()
    {
        //transform.Rotate( 180,0,0 );
    }

    void Update()
    {
        /*Vector3 v = cameraToFace.transform.position - transform.position;
        v.x = 0.0f;
        v.z = 0.0f;
        transform.LookAt(cameraToFace.transform.position - v);
        *///transform.Rotate(0, 180, 0);
        transform.LookAt(transform.position + cameraToFace.transform.rotation * Vector3.back,
            cameraToFace.transform.rotation * Vector3.up);
    }
}
