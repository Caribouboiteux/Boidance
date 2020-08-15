using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DynamicMusic : MonoBehaviour
{
    public float SineSpeed = 1f;
    public float Radius = 1f;
    public float DepthSpeed = 1f;
    public float Depth = 1f;
    public float LateralSpeed = 1f;
    public bool DynPitch = false;
    public bool DynDoppler = false;
    public bool DynRolloff = false;
    public Transform displayer;

    private Vector3 _centre;
    private float _x_pos;
    private float _z_pos = 0;
    private float _angle;
    private AudioSource _audioSource;

    
    private Transform rollOffText;
    private Transform pitchText;
    private Transform DopplerText;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _centre = transform.position;
        rollOffText = displayer.transform.GetChild(0);
        DopplerText = displayer.transform.GetChild(1);
        pitchText = displayer.transform.GetChild(2);
        rollOffText.gameObject.SetActive(false);
        DopplerText.gameObject.SetActive(false);
        pitchText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // control position
        _angle += SineSpeed * Time.deltaTime;
        _x_pos += LateralSpeed * Time.deltaTime;
        var offset = new Vector3(_x_pos, Mathf.Sin(_angle) * Radius, Mathf.Sin(_z_pos) * Mathf.Sin(_z_pos) * Depth);
        transform.position = _centre + offset;
        if (_x_pos > 2)
        {
            _x_pos = -2f;
        }
        // control pitch
        if (DynPitch)
        {
            _audioSource.pitch = Mathf.Sin(_angle);
            pitchText.gameObject.SetActive(true);

        }
        else
        {
            pitchText.gameObject.SetActive(false);
            _audioSource.pitch = 1;
        }
        // control Doppler
        if (DynDoppler)
        {
            DopplerText.gameObject.SetActive(true);
            if (_x_pos > 0)
            {
                _audioSource.dopplerLevel = 10;
            }
            else
            {
                _audioSource.dopplerLevel = 1;

            }
        }
        else
        {
            DopplerText.gameObject.SetActive(false);
            _audioSource.dopplerLevel = 1;
        }
        if (DynRolloff)
        {
            rollOffText.gameObject.SetActive(true);
            _z_pos += DepthSpeed * Time.deltaTime;
            if (_x_pos > 0)
            {
                _audioSource.rolloffMode = AudioRolloffMode.Linear;
                _audioSource.maxDistance = 500;
            }
            else
            {
                _audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
                _audioSource.maxDistance = 50;

            }
        }
        else
        {
            rollOffText.gameObject.SetActive(false);
            _z_pos = 0;
        }
    }
}
