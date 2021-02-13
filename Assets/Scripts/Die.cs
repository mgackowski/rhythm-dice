using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour, IMetronomeObserver
{
    public Metronome metronome; //TODO: Find single instance instead of using Inspector

    public int currentAttack = 1;
    public Direction topFaceOrientation = Direction.Up;
    public Direction movementDirection = Direction.Up;

    private DieModel dieModel;
    private DieSide currentSide;

    public void Notify(MetronomeTick tick)
    {
        Debug.Log("Tick!");
        MoveOneStep();
    }

    public void MoveOneStep()
    {
        DieMovementModel.DieMoveResult movement = DieMovementModel.Move(currentSide, topFaceOrientation, movementDirection);
        topFaceOrientation = movement.NewOrientation;
        currentSide = movement.NewSide;
        currentAttack = currentSide.Value;

        Vector3 thisPosition = transform.position;
        if (movementDirection == Direction.Up) thisPosition.y += 1;
        if (movementDirection == Direction.Down) thisPosition.y -= 1;
        if (movementDirection == Direction.Left) thisPosition.x -= 1;
        if (movementDirection == Direction.Right) thisPosition.x += 1;
        transform.position = thisPosition;

    }

    // Start is called before the first frame update
    void Start()
    {
        metronome.GetComponent<Metronome>().AddObserver(this);
        dieModel = new DieModel();
        currentSide = dieModel.Sides[Side.Top];
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (verticalInput > 0.5f) movementDirection = Direction.Up;
        if (verticalInput < -0.5f) movementDirection = Direction.Down;
        if (horizontalInput > 0.5f) movementDirection = Direction.Right;
        if (horizontalInput < -0.5f) movementDirection = Direction.Left;
    }

    void Destroy()
    {
        metronome.GetComponent<Metronome>().RemoveObserver(this);
    }

}
