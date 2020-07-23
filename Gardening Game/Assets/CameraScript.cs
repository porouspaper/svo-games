using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        transform.localPosition = new Vector3(0, 60, 0);
        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(target, target.up);


        // Same as above, but setting the worldUp parameter to Vector3.left in this example turns the camera on its side
    }
}
