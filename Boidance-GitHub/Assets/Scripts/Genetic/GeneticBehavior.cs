using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneticBehavior : ScriptableObject
{
    public float mutationRate;
    public float mutationRange;
    public float insertionRate;
    public float elitismRate;
    public float lerpTime;
    public abstract void Evolve(FlockAgent agent, List<FlockAgent> context, Flock flock);
}