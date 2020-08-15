using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Stay In Radius")]

public class StayInRadiusBehavior : FlockBehavior
{

    public Vector3 center;
    public float radius = 1f;
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector3 centerOffset = center - agent.transform.position;
        float t = centerOffset.magnitude / radius;
        if (t < 0.5f)
        {
            return Vector3.zero;
        }
        return centerOffset * t * t; // quadratic back force
    }
}
//https://forum.unity.com/threads/vr-and-non-vr-in-a-two-player-game.440904/
