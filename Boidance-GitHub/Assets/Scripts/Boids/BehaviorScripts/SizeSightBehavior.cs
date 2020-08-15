
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Size Sight")]

public class SizeSightBehavior : FlockBehavior
{
    public float rangeSizeMultiplier;
    public int toFollowIndex;
    public Vector3 direction;
    public Vector3 position;
    public float fieldOfViewAngle = 110f;
    public float lerpTime = 0.8f;
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
        if (Vector3.Angle(result, direction) > fieldOfViewAngle * 0.5f)
        {
            agent.transform.localScale = Vector3.Lerp(agent.transform.localScale, Vector3.one * flock.agentSize, lerpTime);

            return Vector3.zero;
        }
        agent.transform.localScale = Vector3.Lerp(agent.transform.localScale, Vector3.one * flock.agentSize * rangeSizeMultiplier, lerpTime);
        return Vector3.zero;
    }
}
