using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MobileBuzzsawScript : MonoBehaviour
{
    [SerializeField] private int xOffset;
    [SerializeField] private int yOffset;
    [SerializeField] private int xRange;
    [SerializeField] private int yRange;
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;

    private Transform markerPos;
    private bool directionX;
    private bool directionY;

    void Awake()
    {
        markerPos = transform.parent.gameObject.transform;

        directionX = true;
        directionY = true;

        transform.position = transform.position + new Vector3(xOffset, yOffset, 0);
    }

    private void FixedUpdate()
    {
        if(xRange > 0)
            if (directionX) MoveUpwardX();
            else MoveDownwardX();

        if (yRange > 0)
            if (directionY) MoveUpwardY();
            else MoveDownwardY();
    }

    void MoveUpwardX()
    {
        if (transform.position.x <= markerPos.position.x + xRange) transform.position = transform.position + new Vector3(0.1f * xSpeed, 0, 0);
        else directionX = false;
    }

    void MoveDownwardX()
    {
        if (transform.position.x >= markerPos.position.x) transform.position = transform.position + new Vector3(-0.1f * xSpeed, 0, 0);
        else directionX = true;
    }

    void MoveUpwardY()
    {
        if (transform.position.y <= markerPos.position.y + yRange) transform.position = transform.position + new Vector3(0, 0.1f * ySpeed, 0);
        else directionY = false;
    }

    void MoveDownwardY()
    {
        if (transform.position.y >= markerPos.position.y) transform.position = transform.position + new Vector3(0, -0.1f * ySpeed, 0);
        else directionY = true;
    }
}
