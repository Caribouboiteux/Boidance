using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Physic Layer")]

public class PhysicsLayerFilter : ContextFilter
{
    public LayerMask mask;
    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        foreach (Transform item in original)
        {
            RaycastHit hit;
            if (Physics.Raycast(agent.transform.position, agent.transform.forward, out hit, 5.0f, mask))
            {
                Debug.Log(item.name);
                filtered.Add(item);
            }
        }

        return filtered;
    }
}
