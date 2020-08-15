using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserConvexHull : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Transform floor;

    public LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 8;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, head.position);
        line.SetPosition(1, leftHand.position);
        line.SetPosition(2, rightHand.position);
        line.SetPosition(3, head.position);
        line.SetPosition(4, new Vector3(head.position.x, floor.position.y, head.position.z));
        line.SetPosition(5, rightHand.position);
        line.SetPosition(6, leftHand.position);
        line.SetPosition(7, new Vector3(head.position.x, floor.position.y, head.position.z));
    }
}
