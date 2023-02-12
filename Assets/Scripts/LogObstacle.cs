using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogObstacle : MonoBehaviour
{
    public bool isFromLeft = true;
    private float WayLength = 8f;
    private Vector3 initialPosition;
    private Vector3 destinationPosition;
    public float moveSpeed;

    void Start()
    {
        initialPosition = transform.position;
        float xDestinationPos = isFromLeft ? initialPosition.x + WayLength : initialPosition.x - WayLength;
        destinationPosition = new Vector3(xDestinationPos, transform.position.y, transform.position.z);
    }
    void Update()
    {
        MoveLog();
    }

    public void MoveLog()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinationPosition, Time.deltaTime * moveSpeed);

        if (transform.position == destinationPosition)
        {
            SwapPositions();
        }
    }

    private void SwapPositions()
    {
        Vector3 t = initialPosition;
        initialPosition = destinationPosition;
        destinationPosition = t;
    }
}
