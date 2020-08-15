using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Obstacle")]
public class ObstacleBehavior : FlockBehavior
{
    public LayerMask mask;
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {

        // if no mask, return no adjustment
        if (mask == 0)
        {
            return Vector3.zero;
        }

        Vector3 obstacleMove = Vector3.zero;

        RaycastHit hit;
        if (Physics.SphereCast(agent.transform.position, 1.5f, agent.transform.forward, out hit, flock.obstacleLength, mask))
        {
            obstacleMove = Vector3.Reflect(agent.transform.forward, hit.normal);
            obstacleMove *= flock.SquareObstacleLength - 2 * flock.obstacleLength * hit.distance + hit.distance*hit.distance;
        }

        return obstacleMove;
    }
}
