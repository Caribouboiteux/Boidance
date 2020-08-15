using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
[System.Serializable]
public class CompositeBehavior : FlockBehavior
{
    public FlockBehavior[] behaviors;

    public float[] weights;
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // handle user mismatch
        if (behaviors == null || weights.Length != behaviors.Length)
        {
            //Debug.LogError("Data mismatch in " + name, this);
            return Vector3.zero;
        }

        // set up move
        Vector3 move = Vector3.zero;
        for (int i=0; i < behaviors.Length; i++)
        {
            try
            {
                if (weights[i] > 0)
                {
                    Vector3 partialMove = behaviors[i].CalculateMove(agent, context, flock) * weights[i];

                    if (partialMove != Vector3.zero)
                    {
                        if (partialMove.sqrMagnitude > weights[i] * weights[i])
                        {
                            partialMove.Normalize();
                            partialMove *= weights[i];
                        }
                        move += partialMove;
                    }
                }
            }
            catch (NullReferenceException)
            {
            }
        }
        return move;

    }
    public string[] GetCompositeBehaviors()
    {
        string[] behaviors = new string[this.behaviors.Length + 1];
        behaviors[0] = "None";
        for (int i = 0; i < this.behaviors.Length; i++)
        {
            behaviors[i + 1] = this.behaviors[i].name;
        }
        return behaviors;
    }
    public void RemoveBehavior(int index)
    {
        FlockBehavior[] newBehaviors = new FlockBehavior[behaviors.Length - 1];
        float[] newWeights = new float[weights.Length - 1];
        int j = 0;
        for (int i = 0; i < behaviors.Length; i++)
        {
            if (i != index)
            {
                newBehaviors[j] = behaviors[i];
                newWeights[j] = weights[i];
                j++;
            }
        }
        behaviors = newBehaviors;
        weights = newWeights;
    }

    public void AddBehavior()
    {
        int oldCount = (this.behaviors != null) ? this.behaviors.Length : 0;
        FlockBehavior[] newBehaviors = new FlockBehavior[oldCount + 1];
        float[] newWeights = new float[oldCount + 1];
        for (int i = 0; i < oldCount; i++)
        {
            newBehaviors[i] = this.behaviors[i];
            newWeights[i] = this.weights[i];
        }
        newWeights[oldCount] = 1f;
        this.behaviors = newBehaviors;
        this.weights = newWeights;

    }


}
