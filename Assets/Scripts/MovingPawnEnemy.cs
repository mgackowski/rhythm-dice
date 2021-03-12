using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPawnEnemy : Enemy
{
    public Direction[] path;
    public int currentPathStep = 0;
    public GameObject animatedObject;

    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
    }

    public void Move()
    {
        if (path.Length == 0) return;

        Vector3 newPosition = transform.position;
        newPosition += path[currentPathStep].DirectionToVector3();
        transform.position = newPosition;
        Physics.SyncTransforms();

        animatedObject.transform.position -= path[currentPathStep].DirectionToVector3();

        currentPathStep = (currentPathStep + 1) % path.Length;

        //Debug.Log("Pawn - " + transform.position);

    }

    public override void Notify(MetronomeTick tick)
    {
        Move();
    }


    public override void PreNotify(MetronomeTick tick)
    {
        //animator.StopPlayback();

        if (path.Length == 0) return;

        animatedObject.transform.position = Vector3.zero;

        switch (path[currentPathStep])
        {
            case Direction.Up: transform.rotation = Quaternion.Euler(0, 0, 0); break;
            case Direction.Right: transform.rotation = Quaternion.Euler(0, 0, -90); break;
            case Direction.Down: transform.rotation = Quaternion.Euler(0, 0, 180); break;
            case Direction.Left: transform.rotation = Quaternion.Euler(0, 0, 90); break;
            default: break;
        }

        //animator.StartPlayback();
        if (path[currentPathStep] != Direction.None) animator.Play("JumpForward");
    }


}
