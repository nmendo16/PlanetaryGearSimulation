using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private int minZoom, maxZoom;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform objectToRotate;
    void Update()
    {
        float axis = Input.GetAxis("CameraRotate");
        float scrollwhellAxis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            if (objectToRotate == null)
                this.transform.Rotate(new Vector3(0, speed * axis * Time.deltaTime, 0));
            else
                objectToRotate.Rotate(new Vector3(0, speed * axis * Time.deltaTime, 0));
        }
        if (scrollwhellAxis != 0)
        {
            float posZ = cameraTransform.localPosition.z + (-scrollwhellAxis) * zoomSpeed * Time.deltaTime;
            posZ = Mathf.Clamp(posZ, minZoom, maxZoom);
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, cameraTransform.localPosition.y, posZ);
        }
    }
}
