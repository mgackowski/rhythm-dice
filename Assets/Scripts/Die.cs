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
        MoveOneStep(); // keep rotating north for testing only
    }

    public void MoveOneStep()
    {
        DieMovementModel.DieMoveResult movement = DieMovementModel.Move(currentSide, topFaceOrientation, movementDirection);
        topFaceOrientation = movement.NewOrientation;
        currentSide = movement.NewSide;
        currentAttack = currentSide.Value;

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
        
    }

    void Destroy()
    {
        metronome.GetComponent<Metronome>().RemoveObserver(this);
    }
}
