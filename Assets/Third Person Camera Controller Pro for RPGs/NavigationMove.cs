using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationMove : MonoBehaviour
{
    Transform playerTransform;
    NavMeshAgent nav;
    Vector3 mousePosition;
    public Camera cam;

    void Start()
    {
        playerTransform = GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            mousePosition = hit.point;
        }

        if (Input.GetMouseButtonDown(0))
            nav.SetDestination(mousePosition);
    }
    
}
