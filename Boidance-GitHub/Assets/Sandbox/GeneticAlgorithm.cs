using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm<T>
{
    public List<DNA<T>> Population { get; private set; }
    public int Generation { get; private set; }
    public float BestFitness { get; private set; }
    public T[] BestGenes { get; private set; }
    List<DNA<T>> newPopulation;
    public float mutationRate;
    private float fitnessSum;
    private System.Random random;

    public GeneticAlgorithm(int populationSize, int dnaSize, System.Random random, Func<T, T> getRandomGene, Func<int, float> fitnessFunction,
        float mutationRate = 0.01f)
    {
        Generation = 1;
        this.mutationRate = mutationRate;
        Population = new List<DNA<T>>(populationSize);
        newPopulation = new List<DNA<T>>(populationSize);
        this.random = random;
        BestGenes = new T[dnaSize];
        for (int i=0; i < populationSize; i++)
        {
            Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
        }
    }

    public void NewGeneration()
    {
        if (Population.Count <=0)
        {
            return;
        }

        CalculateFitness();
        
        newPopulation.Clear();

        for (int i=0; i<Population.Count; i++)
        {
            DNA<T> parent1 = ChooseParents();
            DNA<T> parent2 = ChooseParents();
            DNA<T> child = parent1.Crossover(parent2);
            child.Mutate(this.mutationRate);
            newPopulation.Add(child);
        }

        List<DNA<T>> tmpList = Population;
        Population = newPopulation;
        newPopulation = tmpList;
        Generation++;
    }

    public void prepareGeneration()
    {
        if (Population.Count <= 0)
        {
            return;
        }
        newPopulation.Clear();
    }

    public void endGeneration()
    {
        if (Population.Count <= 0)
        {
            return;
        }
        CalculateFitness();
        List<DNA<T>> tmpList = Population;
        Population = newPopulation;
        newPopulation = tmpList;
        Generation++;
    }
    public void newPlasmid(DNA<T> parent1, DNA<T> parent2)
    {
        DNA<T> child = parent1.Crossover(parent2);
        child.Mutate(this.mutationRate);
        newPopulation.Add(child);
    }

    public void CalculateFitness()
    {
        fitnessSum = 0;
        DNA<T> best = Population[0];

        for (int i=0; i< Population.Count; i++)
        {
            fitnessSum += Population[i].CalculateFitness(i);
            if (Population[i].Fitness > best.Fitness)
            {
                best = Population[i];
            }
        }

        BestFitness = best.Fitness;
        //Debug.Log(fitnessSum);
        best.Genes.CopyTo(BestGenes, 0);
    }

    private DNA<T> ChooseParents()
    {
        double randomNumber = random.NextDouble() * fitnessSum;

        for (int i = 0; i < Population.Count; i++)
        {
            if (randomNumber < Population[i].Fitness)
            {
                return Population[i];
            }

            randomNumber -= Population[i].Fitness;
        }

        return null;
    }
    

}
