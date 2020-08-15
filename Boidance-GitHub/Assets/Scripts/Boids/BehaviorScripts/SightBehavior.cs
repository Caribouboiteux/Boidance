using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Sight")]

public class SightBehavior : FlockBehavior
{
    public bool afraid;
    public int toFollowIndex;
    public Vector3 direction;
    public Vector3 position;
    public float fieldOfViewAngle = 110f;
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        switch (toFollowIndex)
        {
            case 0:
                direction = flock.leftHand.forward;
                position = flock.leftHand.position;
                break;
            case 1:
                direction = flock.rightHand.forward;
                position = flock.rightHand.position;
                break;
            default:
                direction = flock.head.forward;
                position = flock.head.position;
                break;
        }
        Vector3 result = agent.transform.position - position;
        if (afraid)
        {
            if (Vector3.Angle(result, direction) > fieldOfViewAngle * 0.5f)
            {
                return Vector3.zero;
            }
            return Vector3.Reflect(result, -1 * direction);
        }
        
        if (Vector3.Angle(result, direction) < fieldOfViewAngle * 0.5f)
        {
            return Vector3.zero;
        }
        return (result*-1 + direction).normalized;
    }
}