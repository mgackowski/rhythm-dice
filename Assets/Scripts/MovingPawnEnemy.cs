using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPawnEnemy : Enemy
{
    public Direction[] path;
    public int currentPathStep = 0;
    public GameObject animationContainer;

    public void Move()
    {
        if (path.Length == 0) return;

        Vector3 newPosition = transform.position;
        newPosition += path[currentPathStep].DirectionToVector3();
        transform.position = newPosition;
        //audioSource.Play(); too resource-intensive for everyone to play at once
        
        animationContainer.transform.localPosition = Vector3.down; //object is mid-jump but its position moved forward, so move its container backward to maintain visual continuity
        if (path[currentPathStep] == Direction.None) animationContainer.transform.localPosition = Vector3.zero;

        currentPathStep = (currentPathStep + 1) % path.Length;

        Physics.SyncTransforms();
    }

    public override void Notify(MetronomeTick tick)
    {   
        if (alive) Move();
    }


    public override void PreNotify(MetronomeTick tick)
    {

        if (path.Length == 0 || !alive) return;

        switch (path[currentPathStep])
        {
            case Direction.Up: transform.rotation = Quaternion.Euler(0, 0, 0); break;
            case Direction.Right: transform.rotation = Quaternion.Euler(0, 0, -90); break;
            case Direction.Down: transform.rotation = Quaternion.Euler(0, 0, 180); break;
            case Direction.Left: transform.rotation = Quaternion.Euler(0, 0, 90); break;
            default: break;
        }

        if (path[currentPathStep] != Direction.None) animator.SetTrigger("JumpForwardTrigger");

    }

}
