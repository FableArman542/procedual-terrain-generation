using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    bool preventOcclusion = true;
    bool hitSomething = false;
    float storedDistance = 1; 
    [SerializeField]
    Vector3 occlusionCorrection;
    Vector3 offset;
    Vector2 initialMousePos;
    Vector2 currentMousePos;
    float pi = 3.141596f;
    [SerializeField]
    float _sensitivity = 1f;
    [SerializeField]
    float _zoomSensitivity = 0.1f;
    [SerializeField]
    float _inverseSmoothness = 0.1f;
    [SerializeField]
    float _minimumYAngle = 0; //in radians
    [SerializeField]
    float _maximumYAngle = 1.5f; // in radians
    float rotXValue;
    float rotYValue;
    float lastRotXFinal = 0;
    float lastRotYFinal = 0;
    float initialTheta;
    [SerializeField]
    float minCamDistance = 0f;
    [SerializeField]
    float maxCamDistance = 15f;
    float cameraDistanceFromPlayer;
    [SerializeField]
    bool invertXAxis;
    [SerializeField]
    bool invertYAxis;
    [SerializeField]
    bool invertZoom;

    bool needsCamUpdate = false;
    [SerializeField]
    private Transform target;

    private Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
        offset = target.position - transform.position;
        initialTheta = Mathf.Atan(offset.y / offset.GetFlatMagnitude());
        cameraDistanceFromPlayer = offset.magnitude;
    }

    void Update()
    {
        transform.FollowWithDefinedOffset(target,offset);
        if (!invertZoom)
            cameraDistanceFromPlayer = Mathf.Clamp(cameraDistanceFromPlayer * (1-_zoomSensitivity* Input.mouseScrollDelta.y), minCamDistance, maxCamDistance);
        else cameraDistanceFromPlayer = Mathf.Clamp(cameraDistanceFromPlayer * (1 + _zoomSensitivity * Input.mouseScrollDelta.y), minCamDistance, maxCamDistance);
        //set offset here for scroll wheel
        if (!hitSomething)
            offset = Vector3.Lerp(offset, offset.normalized * cameraDistanceFromPlayer, _inverseSmoothness);
        else offset = Vector3.Lerp(offset, offset.normalized * storedDistance * .8f, _inverseSmoothness);
        transform.rotation = Quaternion.LookRotation(offset+occlusionCorrection, Vector3.up);

        #region rotateWithMiddleMouse
        if (Input.GetMouseButtonDown(2))
            initialMousePos = Input.mousePosition;
        if (Input.GetMouseButton(2))
        {
            currentMousePos = Input.mousePosition;
            if (!invertXAxis)
                rotXValue = initialMousePos.x - currentMousePos.x + lastRotXFinal;
            else rotXValue = -initialMousePos.x + currentMousePos.x + lastRotXFinal;
            if (!invertYAxis)
                rotYValue = Mathf.Clamp( initialMousePos.y - currentMousePos.y + lastRotYFinal,_minimumYAngle/_sensitivity/0.01f,_maximumYAngle/_sensitivity/0.01f);
            else rotYValue = Mathf.Clamp( -initialMousePos.y + currentMousePos.y + lastRotYFinal, _minimumYAngle / _sensitivity / 0.01f, _maximumYAngle / _sensitivity / 0.01f);
            //calculate x and z offset
            offset = Vector3.Lerp(offset,CalculateXZOffset(),_inverseSmoothness);
            //calculate y offset and new xz distance
            //clamp to prevent verticals, verticals have tangent divide by 0
            float theta = Mathf.Clamp( initialTheta - rotYValue*0.01f*_sensitivity,-_maximumYAngle,-_minimumYAngle);
            if (theta == _maximumYAngle || theta == _minimumYAngle)
            {
                StoreRotations();
                initialMousePos = Input.mousePosition;
            }
            offset = Vector3.Lerp(offset, CalculateYValue(theta), _inverseSmoothness);
            if (hitSomething)
            {
                offset = Vector3.Lerp(offset, offset.normalized*storedDistance * 0.9f,_inverseSmoothness);
            }
        }
        if (Input.GetMouseButtonUp(2))
        {
            StoreRotations();
        }
        #endregion rotateWithMiddleMouse

        if (preventOcclusion)
        {
            Ray ray = new Ray(target.position + occlusionCorrection, -offset);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, offset.magnitude* cameraDistanceFromPlayer))
            {
                if (hit.transform!= null)
                {
                    hitSomething = true;
                    storedDistance = (hit.point - target.position+occlusionCorrection).magnitude;
                }
                
            }
            else
            {
                offset = Vector3.Lerp(offset, offset.normalized * cameraDistanceFromPlayer, _inverseSmoothness);
                hitSomething = false;
                storedDistance = 1;
            }
        }
    }
    Vector3 CalculateXZOffset()
    {
        if (!hitSomething)
            return new Vector3(
                offset.GetFlatMagnitude() * Mathf.Cos(pi / 2f + rotXValue * _sensitivity * .01f),
                offset.y,
                offset.GetFlatMagnitude() * Mathf.Sin(pi / 2f + rotXValue * _sensitivity * .01f)
            );
        else
        {
            Vector3 temp = new Vector3(
            offset.GetFlatMagnitude() * Mathf.Cos(pi / 2f + rotXValue * _sensitivity * .01f),
            offset.y,
            offset.GetFlatMagnitude() * Mathf.Sin(pi / 2f + rotXValue * _sensitivity * .01f)
            );
            return Vector3.Lerp(temp, temp.normalized * storedDistance * .8f, _inverseSmoothness);
        }
    }
    Vector3 CalculateYValue(float theta)
    {
        float yval = Mathf.Sin(theta) * cameraDistanceFromPlayer;
        float newFlatMagnitude = Mathf.Cos(theta) * cameraDistanceFromPlayer;
        Vector3 temp = offset.GetFlatVector3().normalized * newFlatMagnitude;
        temp += Vector3.up * yval;
        if (hitSomething)
        {
            temp = Vector3.Lerp(temp.normalized * offset.magnitude, temp.normalized * storedDistance * .8f,_inverseSmoothness);
        }
        return temp;
    }
    void StoreRotations()
    {
        lastRotXFinal = rotXValue;
        lastRotYFinal = rotYValue;
    }
}
