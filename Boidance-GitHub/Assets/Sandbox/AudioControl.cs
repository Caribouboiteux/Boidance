using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioControl : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float _start_time = 0f;
    [Range(0.0f, 1.0f)]
    public float _end_time =1f;
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_audioSource.time < _start_time * _audioSource.clip.length)
        {
            _audioSource.time = _start_time * _audioSource.clip.length;
        }
        if (_audioSource.time > _end_time * _audioSource.clip.length)
        {
            _audioSource.time = _start_time * _audioSource.clip.length;
        }
    }
}
