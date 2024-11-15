using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MoveClick : MonoBehaviour
{
    public GameObject objectToMove;

    [SerializeField]
    private float moveSpeed = 30f;

    private Vector3 newPosition = Vector3.zero;
    private bool isMoving = false;
    void SelectNewPos(Vector3 newPos)
    {
        newPosition = newPos;
        isMoving = true;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                SelectNewPos(hit.point);
            }

            if (isMoving)
            {
                objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, newPosition, moveSpeed * Time.deltaTime);
            }

            if (objectToMove.transform.position == newPosition)
            {
                isMoving = false;
            }
        }
    }
}
