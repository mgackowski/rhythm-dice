﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour, IMetronomeObserver
{
    public Metronome metronome; //TODO: Find single instance instead of using Inspector

    public float dieRotationSpeed = 5f; // TODO: Make dependent on metronome speed

    public int currentAttack = 1;
    public Direction topFaceOrientation = Direction.Up;
    public Direction movementDirection = Direction.Up;
    public bool stopped = false;

    private DieModel dieModel;
    private DieSide currentSide;
    private SphereCollider obstacleDetector;
    private GameObject nextTile;

    void Start()
    {
        metronome.GetComponent<Metronome>().AddObserver(this);
        dieModel = new DieModel();
        currentSide = dieModel.Sides[Side.Top];
        obstacleDetector = GetComponent<SphereCollider>();
    }

    void Update()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (verticalInput > 0.5f)
        {
            movementDirection = Direction.Up;
            stopped = false;
        }
        if (verticalInput < -0.5f)
        {
            movementDirection = Direction.Down;
            stopped = false;
        }
        if (horizontalInput > 0.5f)
        {
            movementDirection = Direction.Right;
            stopped = false;
        }
        if (horizontalInput < -0.5f)
        {
            movementDirection = Direction.Left;
            stopped = false;
        }

        UpdateColliderPosition();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            nextTile = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            nextTile = null;
        }
    }

    public void Notify(MetronomeTick tick)
    {
        Debug.Log("Tick!");

        ReactToObstacles();
        if (!stopped) MoveOneStep();
    }

    public void ReactToObstacles()
    {
        if (nextTile != null && nextTile.CompareTag("Enemy"))
        {
            Enemy enemy = nextTile.GetComponent<Enemy>();
            int nextDieAttack = DieMovementModel.Move(currentSide, topFaceOrientation, movementDirection).NewSide.Value;
            if (enemy.attackPower > nextDieAttack)
            {
                movementDirection = ReverseDirection(movementDirection);
                UpdateColliderPosition();
                // Take damage
            }
            else if (enemy.attackPower == nextDieAttack)
            {
                movementDirection = ReverseDirection(movementDirection);
                UpdateColliderPosition();
            }
            else
            {
                Destroy(nextTile);
            }
        }

        //No room to bounce back
        // TODO: replace moving collider with raycast

    }

    public void MoveOneStep()
    {
        // Compute new die value and orientation
        DieMovementModel.DieMoveResult movement = DieMovementModel.Move(currentSide, topFaceOrientation, movementDirection);
        topFaceOrientation = movement.NewOrientation;
        currentSide = movement.NewSide;
        currentAttack = currentSide.Value;


        // Physically move the die
        Vector3 thisPosition = transform.position;
        Vector3 rotationPoint = transform.position;
        Vector3 rotationAxis = Vector3.zero;

        if (movementDirection == Direction.Up)
        {
            thisPosition.y += 1;
            rotationPoint.y += 0.5f;
            rotationAxis = Vector3.right;
        }
        if (movementDirection == Direction.Down)
        {
            thisPosition.y -= 1;
            rotationPoint.y -= 0.5f;
            rotationAxis = Vector3.left;
        }
        if (movementDirection == Direction.Left)
        {
            thisPosition.x -= 1;
            rotationPoint.x -= 0.5f;
            rotationAxis = Vector3.up;
        }
        if (movementDirection == Direction.Right)
        {
            thisPosition.x += 1;
            rotationPoint.x += 0.5f;
            rotationAxis = Vector3.down;
        }
        //transform.position = thisPosition;
        rotationPoint.z += 0.5f;

        StartCoroutine(RotateSmoothly(rotationPoint, rotationAxis, thisPosition));

    }

    private Direction ReverseDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:      return Direction.Down;
            case Direction.Right:   return Direction.Left;
            case Direction.Down:    return Direction.Up;
            case Direction.Left:    return Direction.Right;
            default:                return Direction.Up;
        }
    }


    private void UpdateColliderPosition()
    {
        if (movementDirection == Direction.Up)
        {
            obstacleDetector.center = transform.InverseTransformPoint(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z));
        }
        if (movementDirection == Direction.Down)
        {
            obstacleDetector.center = transform.InverseTransformPoint(new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z));
        }
        if (movementDirection == Direction.Left)
        {
            obstacleDetector.center = transform.InverseTransformPoint(new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z));
        }
        if (movementDirection == Direction.Right)
        {
            obstacleDetector.center = transform.InverseTransformPoint(new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z));
        }
    }

    private IEnumerator RotateSmoothly(Vector3 rotationPoint, Vector3 rotationAxis, Vector3 newPosition)
    {
        float rotationCompleted = 0f;
        float rotationTarget = 90f;

        while (rotationCompleted < rotationTarget)
        {
            float nextRotation = 90f * dieRotationSpeed * Time.deltaTime;
            if (rotationCompleted + nextRotation > rotationTarget) nextRotation = rotationTarget - rotationCompleted;
            transform.RotateAround(rotationPoint, rotationAxis, nextRotation);
            rotationCompleted += nextRotation;
            yield return null;
        }
        transform.position = newPosition;

    }

    void Destroy()
    {
        metronome.GetComponent<Metronome>().RemoveObserver(this);
    }

}
