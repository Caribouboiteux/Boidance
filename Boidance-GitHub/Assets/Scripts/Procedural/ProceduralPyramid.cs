using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class ProceduralPyramid : MonoBehaviour
{
    public float base_height = 1;
    public float base_weight = 1;
    public float height = 1;
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;

    }
    void Start()
    {
        base_weight = base_weight * 0.5f; // center the pyramid
        base_height = base_height * 0.5f;
        MakeMeshData();
        CreateMesh();
    }

    public void ResetMesh()
    {
        MakeMeshData();
        CreateMesh();
    }

    void MakeMeshData()
    {
        // create array of vertices
        vertices = new Vector3[] {      new Vector3(-base_weight, -base_height, 0), new Vector3(-base_weight, base_height, 0), new Vector3(base_weight, -base_height, 0), new Vector3(base_weight, base_height, 0),
                                        new Vector3(0, 0, height),   new Vector3(-base_weight, -base_height, 0),           new Vector3(-base_weight, base_height, 0),
                                        new Vector3(0, 0, height),   new Vector3(base_weight, -base_height, 0),           new Vector3(-base_weight, -base_height, 0),
                                        new Vector3(0, 0, height),   new Vector3(base_weight, base_height, 0), new Vector3(base_weight, -base_height, 0),
                                        new Vector3(0, 0, height),   new Vector3(-base_weight, base_height, 0), new Vector3(base_weight, base_height, 0),
                                    };

        // create an array of integers
        triangles = new int[] { 2, 1, 3, 0, 1, 2,
                                6, 5, 4,
                                9, 8, 7,
                                12, 11, 10,
                                15, 14, 13};
        colors = new Color[vertices.Length];
        InitColor();
    }

    public void MakeMeshData(List<Vector3> positions, int[] order)
    {
        vertices = positions.ToArray();
        triangles = order;
        ResetColor(vertices.Length);
        CreateMesh();
    }
    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.colors = colors;
    }

    public void ResetColor(int j)
    {
        Color[] newColor = new Color[j];
        for (int i = 0; i < j; i++)
        {
            newColor[i] = colors[0];
        }
        colors = newColor;
    }

    public void InitColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = new Color(r, g, b);
        }
    }

    public void UpdateColor(float r, float g, float b, float lerpTime)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = Color.Lerp(colors[i], new Color(r, g, b), lerpTime);
        }
        mesh.colors = colors;
    }

    public Color getColor()
    {
        return this.colors[0];
    }

}
