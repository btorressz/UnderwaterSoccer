using UnityEngine;

public class TeamAgent : MonoBehaviour
{
    // Reference to the game manager
    private GameManager gameManager;

    // Reference to the agent's rigidbody
    private Rigidbody agentRB;

    // Reference to the agent's decision-making component
    private DecisionMaking decisionMaking;

    // The target that the agent is currently pursuing
    private Transform target;

    // The maximum speed of the agent
    public float maxSpeed = 10.0f;

    // The force with which the agent accelerates
    public float acceleration = 5.0f;

    void Start()
    {
        // Get references to the necessary components
        gameManager = FindObjectOfType<GameManager>();
        agentRB = GetComponent<Rigidbody>();
        decisionMaking = GetComponent<DecisionMaking>();
    }

    void FixedUpdate()
    {
        // Determine the agent's next action
        Action nextAction = decisionMaking.DecideAction();

        // Perform the action
        switch (nextAction)
        {
            case Action.MoveToBall:
                MoveTo(gameManager.ball.transform);
                break;
            case Action.KickBall:
                KickBall();
                break;
            case Action.MoveToGoal:
                MoveTo(gameManager.opponentGoal.transform);
                break;
            case Action.CoverOpponent:
                CoverOpponent();
                break;
            default:
                break;
        }
    }

    void MoveTo(Transform target)
    {
        // Set the target for the agent to pursue
        this.target = target;

        // Calculate the direction to the target
        Vector3 direction = target.position - transform.position;
        direction.y = 0.0f;
        direction.Normalize();

        // Calculate the desired velocity of the agent
        Vector3 desiredVelocity = direction * maxSpeed;

        // Calculate the steering force necessary to achieve the desired velocity
        Vector3 steeringForce = (desiredVelocity - agentRB.velocity) * acceleration;

        // Apply the steering force to the agent's rigidbody
        agentRB.AddForce(steeringForce, ForceMode.Acceleration);

        // Rotate the agent to face the target
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void KickBall()
    {
        // Kick the ball towards the opponent's goal
        Vector3 direction = gameManager.opponentGoal.transform.position - gameManager.ball.transform.position;
        direction.y = 0.0f;
        direction.Normalize();
        gameManager.ball.GetComponent<Rigidbody>().AddForce(direction * 1000.0f, ForceMode.Impulse);
    }

    void CoverOpponent()
    {
        // Move to a position to block the opponent's path
        Vector3 targetPosition = gameManager.opponent.transform.position;
        targetPosition.x -= 5.0f;
        MoveTo(targetPosition);
    }
}

public enum Action
{
    MoveToBall,
    KickBall,
    MoveToGoal,
    CoverOpponent
}

public class DecisionMaking : MonoBehaviour
{
    // The current state of the agent
    private State currentState;

    void Start()
    {
        // Set the initial state of the agent
        currentState = State.Idle;
    }

    public Action DecideAction()
    {
        // Determine the next action based on the current state
        switch (currentState)
        {
            case State.Idle:
                currentState = State.PursueBall;
                return Action.MoveToBall;
            case State.PursueBall:
                if (IsInKickingRange())
                {
                    currentState = State.KickBall;
                    return Action.KickBall;
                }
                else if (IsOpponentBetweenBallAndGoal())
                {
                    currentState = State.Cover
                }
        }
    }