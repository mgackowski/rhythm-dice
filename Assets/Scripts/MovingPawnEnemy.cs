using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPawnEnemy : Enemy
{
    public Direction[] path;
    public int currentPathStep = 0;

    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public void Move()
    {
        if (path.Length == 0) return;

        Vector3 newPosition = transform.position;
        newPosition += path[currentPathStep].DirectionToVector3();
        transform.position = newPosition;

        currentPathStep = (currentPathStep + 1) % path.Length;

    }

    public override void Notify(MetronomeTick tick)
    {
        Move();
    }


    public override void PreNotify(MetronomeTick tick)
    {
        //Debug.Log("Inherited pre-tick!");
        //animator.Play("JumpForward");
    }


}
