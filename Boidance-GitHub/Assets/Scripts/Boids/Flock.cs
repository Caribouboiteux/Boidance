using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Flock : MonoBehaviour
{

    [Header("Boids Attribute")]
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public SaveSystem saveSystem;
    public Transform leftHand;
    public Transform rightHand;
    public Transform head;
    public Toggle matchSpeedController;
    private float sqrtSpeed;
    private float speed;
    private Vector3 previousPos;
    private Vector3 previousPos2;
    private Vector3 previousPos3;
    public ColorPicker colorPiker;
    public Transform pointToFollow;
    [Range(1, 500)]
    public float startingCount = 250f;
    const float AgentDensity = 0.08f;

    [Header("Agent Attribute")]
    [Range(0.01f, 1)]
    public float agentSize = 1;
    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(0f, 50f)]
    public float maxSpeed = 5f;
    [Range(0.01f, 5f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;
    [Range(0f, 10f)]
    public float obstacleLength = 10f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    float squareObstacleLength;

    private bool leftMenuPressed;
    private bool rightMenuPressed;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } set { squareAvoidanceRadius = value; } }
    public float SquareObstacleLength { get { return squareObstacleLength; } }



    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        squareObstacleLength = obstacleLength * obstacleLength;

        ChangeNumberOfAgent();

    }

    public void ChangeNumberOfAgent()
    {
        for (int i = agents.Count - 1; i >= 0; i--)
        {
            Destroy(agents[i].gameObject);
        }
        agents.Clear();
        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                UnityEngine.Random.insideUnitSphere * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.one * UnityEngine.Random.Range(0f, 360f)),
                transform
                );

            newAgent.name = "Agent " + i;
            newAgent.transform.localScale = new Vector3(agentSize, agentSize, agentSize);
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
    }

    public void ChangeSizeOfAgent()
    {
        for (int i = 0; i < startingCount; i++)
        {
            agents[i].transform.localScale = new Vector3(agentSize, agentSize, agentSize);
        }
    }
    public void ChangeSizeOfAgent(float size)
    {
        for (int i = 0; i < startingCount; i++)
        {
            agents[i].transform.localScale = new Vector3(size, size, size);
        }
    }

    public void ChangeShapeOfAgent(List<Vector3> positions, int[] order, bool reset)
    {
        if (reset)
        {
            for (int i = 0; i < startingCount; i++)
            {
                agents[i].pyramid.ResetMesh();
            }
            return;
        }
        for (int i = 0; i < startingCount; i++)
        {
            agents[i].pyramid.MakeMeshData(positions, order);
        }
    }

    public void ChangeColorOfAgent(Color originalColor)
    {
        if (agents[0].pyramid != null)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].pyramid.UpdateColor(originalColor.r, originalColor.g, originalColor.b, 1);
            }
        }
    }

    private float GetTriggerValue(bool left, bool right)
    {
        float floatValue;
        float result = 0f;
        var devices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left, devices);
        UnityEngine.XR.InputFeatureUsage<float> featureValue = CommonUsages.trigger;
        if (left)
        {
            foreach (var device in devices)
            {
                if (device.TryGetFeatureValue(featureValue, out floatValue))
                {
                    result += floatValue;
                }
            }
        }

        devices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Right, devices);
        if (right)
        {
            foreach (var device in devices)
            {
                if (device.TryGetFeatureValue(featureValue, out floatValue))
                {
                    result += floatValue;
                }
            }
        }

        return result;
    }

    private void GetMenuValue()
    {
        var devices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left, devices);
        UnityEngine.XR.InputFeatureUsage<bool> featureValue = CommonUsages.primaryButton;
        foreach (var device in devices)
        {
            if (device.TryGetFeatureValue(featureValue, out bool valueL))
            {
                if (valueL & !leftMenuPressed)
                {
                    saveSystem.OnDown();
                    leftMenuPressed = true;
                }
                else if (!valueL)
                {
                    leftMenuPressed = false;
                }
            }
        }

        devices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Right, devices);
        foreach (var device in devices)
        {
            if (device.TryGetFeatureValue(featureValue, out bool valueR))
            {
                if (valueR & !rightMenuPressed)
                {
                    saveSystem.OnUp();
                    rightMenuPressed = true;
                }
                else if (!valueR)
                {
                    rightMenuPressed = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetMenuValue();
        if (saveSystem.CurrentData.matchWheel)
        {
            colorPiker.getNewColor();
        }
        if (saveSystem.CurrentData.followRight | saveSystem.CurrentData.followLeft | saveSystem.CurrentData.followHead)
        {
            float newSpeed = 50 * ((previousPos - leftHand.position).magnitude * (saveSystem.CurrentData.followLeft? 1 : 0)
                + (previousPos2 - rightHand.position).magnitude * (saveSystem.CurrentData.followRight ? 1 : 0)
                + (previousPos3 - head.position).magnitude * (saveSystem.CurrentData.followHead ? 1 : 0)
                );
            previousPos = leftHand.position;
            previousPos2 = rightHand.position;
            previousPos3 = head.position;
            speed = Mathf.Lerp(speed, newSpeed, 6f * Time.deltaTime);
        }
        else
        {
            speed = maxSpeed;
        }
        if (saveSystem.currentData.triggerLeft | saveSystem.currentData.triggerRight)
        {
            speed *= (1 - GetTriggerValue(saveSystem.currentData.triggerLeft, saveSystem.currentData.triggerRight));
        }
        if (speed < 0.01)
        {
            return;
        }
        foreach (FlockAgent agent in agents)
        {
            if (saveSystem.CurrentData.compositeBehavior != null)
            {
                List<Transform> context = GetNearbyObjects(agent);
                Vector3 move = saveSystem.CurrentData.compositeBehavior.CalculateMove(agent, context, this);
                move *= driveFactor;
                if (move.sqrMagnitude > speed * speed)
                {
                    move = move.normalized * speed;
                }
                agent.Move(move);
            }

        if (saveSystem.CurrentData.compositeGene != null)
            {
            List<FlockAgent> geneticContext = GetNearbyDNA(agent);
            saveSystem.CurrentData.compositeGene.Evolve(agent, geneticContext, this);
            }
        }
        Vector3 d = new Vector3(head.position.x, 0, head.position.z);
        if (saveSystem.currentData.matchShape)
        {
            int[] order = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
            List<Vector3> positions = new List<Vector3> { head.position, leftHand.position, rightHand.position,
                rightHand.position, leftHand.position, d,
                head.position, d, leftHand.position,
                head.position, rightHand.position, d
            };
            ChangeShapeOfAgent(positions, order, false);
        }
        if (saveSystem.currentData.matchSize)
        {
            ChangeSizeOfAgent((Vector3.Distance(d, head.position)
                + Vector3.Distance(d, leftHand.position)
                + Vector3.Distance(d, rightHand.position)
                + Vector3.Distance(rightHand.position, leftHand.position)
                + Vector3.Distance(rightHand.position, head.position)
                + Vector3.Distance(head.position, leftHand.position)) * 0.01f);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach(Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }

    List<FlockAgent> GetNearbyDNA(FlockAgent agent)
    {
        List<FlockAgent> context = new List<FlockAgent>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                FlockAgent item = c.GetComponent<FlockAgent>();
                if (item != null)
                {
                    context.Add(c.GetComponent<FlockAgent>());
                }
            }
        }
        return context;
    }
}
