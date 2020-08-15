using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public Object MusicFolder = null;
    [HideInInspector]
    public string folder_path;
    [Header("Start point")]
    [Range(0f, 1f)]
    public float RandomStartRange = 0.5f;
    [Range(0f, 1f)]
    [Header("Volume")]
    public float BaseVolume = 1f;
    [Range(0f, 1f)]
    public float RandomVolumeRange = 0.5f;
    [Range(-3f, 3f)]
    [Header("Pitch")]
    public float BasePitch = 1f;
    [Range(0f, 1f)]
    public float RandomPitchRange = 0.5f;
    [Range(0f, 1f)]
    [Header("Spatial Bend")]
    public float BaseSpatialBend = 1f;
    [Range(0f, 1f)]
    public float RandomSpatialBendRange = 0.5f;
    [Range(0f, 1f)]
    [Header("Doppler Effect")]
    public float BaseDopplerEffect = 0f;
    [Range(0f, 3f)]
    public float RandomDopplerEffectRange = 1f;
    /*[HideInInspector]*/
    [Header("Effective List of sounds")]
    public List<Sound> sounds = new List<Sound>();
    void Awake()
    {
        foreach (Transform t in transform)
        {
            AudioSource source = t.gameObject.AddComponent<AudioSource>();
            Sound randomSound = sounds[Random.Range(0, sounds.Count)];
            source.clip = randomSound.clip;
            source.loop = randomSound.loop;
            source.playOnAwake = true;
        }

        ResetOrchestra();
    }

    public void ResetOrchestra()
    {
        foreach (Transform t in transform)
        {
            AudioSource source = t.gameObject.GetComponent<AudioSource>();
            source.volume = BaseVolume * (1 - Random.Range(-RandomVolumeRange, RandomVolumeRange));
            source.pitch = BasePitch * (1 - Random.Range(-RandomPitchRange, RandomPitchRange));
            source.spatialBlend = BaseSpatialBend - Random.Range(0, RandomSpatialBendRange);
            source.dopplerLevel = BaseDopplerEffect + Random.Range(0, RandomDopplerEffectRange);
            source.time = Random.Range(0.0f, RandomStartRange * source.clip.length);
            source.Play();
        }
    }

    public void ReloadOrchestra()
    {
        foreach (Transform t in transform)
        {
            AudioSource source = t.gameObject.GetComponent<AudioSource>();
            Sound randomSound = sounds[Random.Range(0, sounds.Count)];
            source.clip = randomSound.clip;
            source.loop = randomSound.loop;
            source.playOnAwake = true;
        }

        ResetOrchestra();
    }
}
