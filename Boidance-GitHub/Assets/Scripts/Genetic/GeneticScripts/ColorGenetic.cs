using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Genetic/Color")]

public class ColorGenetic : GeneticBehavior
{
    public Material targetColor;
    public bool complementaryColor = true;
    public override void Evolve(FlockAgent agent, List<FlockAgent> context, Flock flock)
    {
        // if no neighbors, just return mutation

        if (context.Count == 0)
        {
            if (Random.Range(0f, 1f) < mutationRate)
            {
                Mutate(agent);
            }
            return;
        }
        Color originalColor = agent.pyramid.getColor();
        foreach (FlockAgent item in context)
        {
            if (Random.Range(0f, 1f) < item.fitness - elitismRate)
            {
                if (Random.Range(0f, 1f) < insertionRate)
                {

                    originalColor = Crossover(originalColor, item.pyramid.getColor());
                }
            }
        }
        agent.pyramid.UpdateColor(originalColor.r, originalColor.g, originalColor.b, lerpTime);
        Mutate(agent);
        agent.fitness = FitnessFunction(originalColor);
        return;
    }
    public void Mutate(FlockAgent agent)
    {
        Color colors = agent.pyramid.getColor();
        agent.pyramid.UpdateColor(
                GetRandomFloat(colors[0]),
                GetRandomFloat(colors[1]),
                GetRandomFloat(colors[2]),
                lerpTime);
    }
    private float GetRandomFloat(float original)
    {
        float f = original + UnityEngine.Random.Range(-mutationRange, mutationRange);
        f = f < 0 ? 0 : f;
        f = f > 1 ? 1 : f;
        return f;
    }

    public Color Crossover(Color original, Color newColor)
    {
        float r = Random.Range(0f, 1f) < 0.5 ? original.r : newColor.r;
        float g = Random.Range(0f, 1f) < 0.5 ? original.g : newColor.g;
        float b = Random.Range(0f, 1f) < 0.5 ? original.b : newColor.b;
        return new Color(r, g, b);
    }

    private float FitnessFunction(Color color)
    {
        Color targetColor = this.targetColor.GetColor("_Tint");
        if (complementaryColor)
        {
            Color.RGBToHSV(targetColor, out float H, out float S, out float V);
            float negativeH = (H + 0.5f) % 1f;
            targetColor = Color.HSVToRGB(negativeH, S, V);
        }
        float score = 1;
        score -= Mathf.Abs(color.r - targetColor.r) / 3;
        score -= Mathf.Abs(color.g - targetColor.g) / 3;
        score -= Mathf.Abs(color.b - targetColor.b) / 3;

        score = (Mathf.Pow(score, 6));
        return score;
    }
}
