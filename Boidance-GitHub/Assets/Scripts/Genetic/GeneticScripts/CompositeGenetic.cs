using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Genetic/Composite")]

public class CompositeGenetic : GeneticBehavior
{

    public GeneticBehavior[] genes;

    public override void Evolve(FlockAgent agent, List<FlockAgent> context, Flock flock)
    {
        if (genes != null)
        {

            for (int i = 0; i < genes.Length; i++)
            {
                try
                {
                    genes[i].Evolve(agent, context, flock);
                }
                catch (NullReferenceException)
                {
                }

            }
        }
    }

    public string[] GetGenes()
    {
        string[] genes = new string[this.genes.Length + 1];
        genes[0] = "None";
        for (int i = 0; i < this.genes.Length; i++)
        {
            genes[i + 1] = this.genes[i].name;
        }
        return genes;
    }
    public void RemoveGene(int index)
    {
        GeneticBehavior[] newGenes = new GeneticBehavior[genes.Length - 1];
        int j = 0;
        for (int i = 0; i < genes.Length; i++)
        {
            if (i != index)
            {
                newGenes[j] = genes[i];
                j++;
            }
        }
        genes = newGenes;
    }

    public void AddGene(GeneticBehavior gene)
    {
        int oldCount = (this.genes != null) ? this.genes.Length : 0;
        GeneticBehavior[] newGenes = new GeneticBehavior[oldCount + 1];
        for (int i = 0; i < oldCount; i++)
        {
            newGenes[i] = this.genes[i];
        }
        newGenes[oldCount] = gene;
        this.genes = newGenes;
    }
}
