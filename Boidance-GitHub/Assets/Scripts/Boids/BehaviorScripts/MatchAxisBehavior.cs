using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/MatchAxisPosition")]
public class MatchAxisBehavior : FlockBehavior
{
    public int toFollowIndex;
    public Vector3 position;
    public Vector3 move;
    public int axis;
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        switch (toFollowIndex)
        {
            case 0:
                position = flock.leftHand.position;
                break;
            case 1:
                position = flock.leftHand.position;
                break;
            default:
                position = flock.head.position;
                break;
        }
        move = Vector3.zero;
        switch (axis)
        {
            case 0:
                move.x = position.x - agent.transform.position.x;
                break;
            case 1:
                move.y = position.y - agent.transform.position.y;
                break;
            default:
                move.z = position.z - agent.transform.position.z;
                break;
        }
        return move; // quadratic back force
    }
}
