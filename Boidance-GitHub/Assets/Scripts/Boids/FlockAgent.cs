using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    Collider agentCollider;
    AudioSource audioSource;
    public float fitness;
    Flock agentFlock;
    [HideInInspector]
    public ProceduralPyramid pyramid;
    public Flock AgentFlock { get { return agentFlock; } }
    public Collider AgentCollider { get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider>();
        pyramid = GetComponentInChildren<ProceduralPyramid>();
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector3 velocity)
    {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime;
    }
}
