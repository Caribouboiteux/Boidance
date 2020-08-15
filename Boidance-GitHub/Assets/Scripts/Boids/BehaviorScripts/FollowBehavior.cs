using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Follow")]

public class FollowBehavior : FlockBehavior
{
    public int toFollowIndex;
    public float radius;
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector3 centerOffset = new Vector3();
        switch (toFollowIndex)
        {
            case 0:
                centerOffset = flock.leftHand.position - agent.transform.position;
                break;
            case 1:
                centerOffset = flock.rightHand.position - agent.transform.position;
                break;
            case 2:
                centerOffset = (flock.leftHand.position + flock.leftHand.position - 2*agent.transform.position)/2;
                break;
            default:
                centerOffset = flock.head.position - agent.transform.position;
                break;
        }
        float t = centerOffset.sqrMagnitude / radius;/// radius;
        if (t < 0.5)
        {
            return Vector3.zero;
        }
        return centerOffset * t; // quadratic back force
    }
}
