using System.Collections.Generic;
using System;
public class DNA<T>
{
    public T[] Genes { get; private set; }
    public float Fitness { get; private set; }

    private  Random random;
    private Func<T, T> getRandomGene;
    private Func<int, float> fitnessFunction;
    public DNA(int size, Random random, Func<T, T> getRandomGene, Func<int, float> fitnessFunction, bool shouldInitGenes = true)
    {
        Genes = new T[size];
        this.random = random;
        this.fitnessFunction = fitnessFunction;
        this.getRandomGene = getRandomGene;
        if (shouldInitGenes)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                Genes[i] = getRandomGene(Genes[i]);
            }
        }

    }

    public float CalculateFitness(int index)
    {
        Fitness = fitnessFunction(index);
        return Fitness;
    }

    public DNA<T> Crossover(DNA<T> otherParent)
    {
        DNA<T> child = new DNA<T>(Genes.Length, random, getRandomGene, fitnessFunction, shouldInitGenes: false);

        for (int i=0; i < Genes.Length; i++){
            child.Genes[i] = random.NextDouble() < 0.5 ? Genes[i]: otherParent.Genes[i];
            child.Genes[i] = Genes[i];
        }
        return child;
    }

    public void Mutate(float mutationRate)
    {
        for (int i=0; i < Genes.Length; i++)
        {
            if (random.NextDouble() < mutationRate)
            {
                Genes[i] = getRandomGene(Genes[i]);
            }
        }
    }
}
