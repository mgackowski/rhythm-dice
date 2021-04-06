using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour, IMetronomeObserver
{
    public Metronome metronome; //TODO: Find single instance instead of using Inspector
    public MovementIndicator movementIndicator;

    public float dieRotationSpeed = 5f; // TODO: Make dependent on metronome speed

    public int currentAttack = 1;
    public Direction topFaceOrientation = Direction.Up;
    public Direction movementDirection = Direction.Up;
    public bool stopped = false;

    private DieModel dieModel;
    private DieSide currentSide;
    private SphereCollider obstacleDetector;
    private GameObject nextTile;
    private DieAudioController audioController;

    void Start()
    {
        metronome.GetComponent<Metronome>().AddLateObserver(this);
        dieModel = new DieModel();
        currentSide = dieModel.Sides[Side.Top];
        obstacleDetector = GetComponent<SphereCollider>();
        audioController = GetComponentInChildren<DieAudioController>();
    }

    void Update()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool displayMovementIndicator = false;

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

        if (Input.anyKeyDown) displayMovementIndicator = true;

        if (displayMovementIndicator)
        {
            Vector3 movementIndicatorPosition = transform.position;
            movementIndicatorPosition.z = 0.49f; //TODO: Magic number
            movementIndicatorPosition += movementDirection.DirectionToVector3();
            movementIndicatorPosition.x = Mathf.Round(movementIndicatorPosition.x);
            movementIndicatorPosition.y = Mathf.Round(movementIndicatorPosition.y);

            Instantiate(movementIndicator, movementIndicatorPosition, Quaternion.Euler(-90f, 0f, 0f));
        }

    }

    public void PreNotify(MetronomeTick tick)
    {
        //Debug.Log("Pre-Tick!");

    }

    public void Notify(MetronomeTick tick)
    {
        ReactToObstacles();
        if (ScanFutureNeighbours())
        {
            metronome.SetPlayTensionClip();
            Camera.main.GetComponent<CameraTracker>().ZoomIn();
        }
        else
        {
            Camera.main.GetComponent<CameraTracker>().ZoomBack();
        }
        if (!stopped) MoveOneStep();
        //Physics.SyncTransforms();
    }

    public bool ScanFutureNeighbours()
    {
        RaycastHit hit;
        Vector3 nextPosition = transform.position + movementDirection.DirectionToVector3();
        if (Physics.Raycast(nextPosition, Direction.Up.DirectionToVector3(), out hit, 1f))
        {
            if (hit.collider.gameObject.CompareTag("Enemy")) return true;
        }
        if (Physics.Raycast(nextPosition, Direction.Right.DirectionToVector3(), out hit, 1f))
        {
            if (hit.collider.gameObject.CompareTag("Enemy")) return true;
        }
        if (Physics.Raycast(nextPosition, Direction.Down.DirectionToVector3(), out hit, 1f))
        {
            if (hit.collider.gameObject.CompareTag("Enemy")) return true;
        }
        if (Physics.Raycast(nextPosition, Direction.Left.DirectionToVector3(), out hit, 1f))
        {
            if (hit.collider.gameObject.CompareTag("Enemy")) return true;
        }
        return false;
    }

    public void ReactToObstacles()
    {

        RaycastHit hit;

        // TODO: Refactor this condition
        if (Physics.Raycast(transform.position, movementDirection.DirectionToVector3(), out hit, 1f)
            || (Physics.Raycast(transform.position + movementDirection.DirectionToVector3(), movementDirection.ReverseDirection().DirectionToVector3(), out hit, 1f))
            && hit.collider.gameObject.GetComponent<MovingPawnEnemy>() != null
            && hit.collider.gameObject.GetComponent<MovingPawnEnemy>().lastDirection == movementDirection.ReverseDirection()) // reverse raycast to check for enemies that swap places with you
        {
            nextTile = hit.collider.gameObject;

            if (hit.collider.gameObject.CompareTag("Enemy"))
            {

                Enemy enemy = nextTile.GetComponent<Enemy>();
                enemy.playSound();
                //int nextDieAttack = DieMovementModel.Move(currentSide, topFaceOrientation, movementDirection).NewSide.Value;
                if (enemy.attackPower > currentAttack)
                {
                    movementDirection = movementDirection.ReverseDirection();
                    audioController.PlayChord(DieAudioController.SoundEffect.TakeDamage);
                    audioController.PlayBeat();
                    if (Physics.Raycast(transform.position, movementDirection.DirectionToVector3(), 1f)) stopped = true;

                    GameSession.instance.TakeDamage(enemy.attackPower - currentAttack);
                }
                else if (enemy.attackPower == currentAttack)
                {
                    movementDirection = movementDirection.ReverseDirection();
                    audioController.PlayChord(DieAudioController.SoundEffect.Rebound);
                    audioController.PlayBeat();
                    if (Physics.Raycast(transform.position, movementDirection.DirectionToVector3(), 1f)) stopped = true;
                    enemy.Bounce();
                }
                else
                {
                    audioController.PlayChord(DieAudioController.SoundEffect.DealDamage);
                    enemy.GetComponent<Enemy>().GetSquashed();
                    nextTile.SetActive(false); // bug: cuts off enemy sound too early
                }
            }

            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                audioController.PlayBeat();
                movementDirection = movementDirection.ReverseDirection();
                if (Physics.Raycast(transform.position, movementDirection.DirectionToVector3(), 1f)) stopped = true;
            }

            if (hit.collider.gameObject.CompareTag("HealthPickup"))
            {
                audioController.PlayChord(DieAudioController.SoundEffect.DealDamage);
                GameSession.instance.Heal();
                hit.collider.gameObject.SetActive(false);

            }
        }
        else
        {
            nextTile = null;
        }

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

        audioController.PlayTone(currentAttack);

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
