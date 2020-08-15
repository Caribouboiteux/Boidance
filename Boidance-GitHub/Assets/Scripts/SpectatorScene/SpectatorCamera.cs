using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCamera : MonoBehaviour
{

    public Transform spectatorCamera;
    public Transform user;
    public float speed = 1;
    public float ZoomAmount;
    public float MaxToClamp = 3f;
    public float ROTSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(0.5f * transform.position.magnitude * speed * Time.deltaTime, 0, 0));
            transform.LookAt(user);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(0.5f* transform.position.magnitude * -speed * Time.deltaTime, 0, 0));
            transform.LookAt(user);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {

            transform.Translate(new Vector3(0, 0.5f * transform.position.magnitude * -speed * Time.deltaTime, 0));
            transform.LookAt(user);
            if (transform.position.y < 0.1f)
            {
                transform.position = new Vector3( transform.position.x, 0.1f, transform.position.z);
            }
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, 0.5f * transform.position.magnitude * speed * Time.deltaTime, 0));
            transform.LookAt(user);
        }
        ZoomAmount += Input.GetAxis("Mouse ScrollWheel");
        //ZoomAmount = Mathf.Clamp(ZoomAmount, -MaxToClamp, MaxToClamp);
        var translate = Mathf.Min(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")),Mathf.Abs(ZoomAmount));
        gameObject.transform.Translate(0, 0, translate * ROTSpeed * Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")));

    }
}
