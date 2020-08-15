using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/MatchControllerSpeed")]

public class MatchDeviceSpeed : FlockBehavior
{
    public int toFollowIndex;
    public Vector3 previousPosition = Vector3.zero;
    public Vector3 previousPosition2 = Vector3.zero;
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        float speed;
        switch (toFollowIndex)
        {
            case 0:
                speed = Vector3.Distance(previousPosition, flock.leftHand.position) * Time.deltaTime;
                previousPosition = flock.leftHand.position;
                break;
            case 1:
                speed = Vector3.Distance(previousPosition, flock.leftHand.position);
                previousPosition = flock.leftHand.position;
                break;
            case 2:
                speed = 0.5f*(Vector3.Distance(previousPosition, flock.leftHand.position) + Vector3.Distance(previousPosition2, flock.leftHand.position));
                previousPosition = flock.leftHand.position;
                previousPosition2 = flock.rightHand.position;
                break;
            default:
                speed = Vector3.Distance(previousPosition, flock.leftHand.position);
                previousPosition = flock.head.position;
                break;
        }
        flock.maxSpeed = speed * 10f;
        Debug.Log(flock.maxSpeed);
        return Vector3.zero; // quadratic back force
    }
}
