using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMove : MonoBehaviour
{
    public float RotateSpeed = 5f;
    public float Radius = 5f;

    private Vector3 _centre;
    public float _angle;
    // Start is called before the first frame update
    void Start()
    {
        _centre = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _angle += RotateSpeed * Time.deltaTime;
        var offset = new Vector3(Mathf.Sin(_angle), 0, Mathf.Cos(_angle)) * Radius;
        transform.position = _centre + offset;
    }
}
