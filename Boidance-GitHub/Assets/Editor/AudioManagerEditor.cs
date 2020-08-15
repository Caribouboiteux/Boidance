using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Object = UnityEngine.Object;

[CustomEditor(typeof(AudioManager))]

public class AudioManagerEditor : Editor
{

    private Object[] clips;
    public override void OnInspectorGUI()
    {

        AudioManager audioManager = (AudioManager)target;

        if (GUILayout.Button("Load Folder"))
        {
            audioManager.folder_path = AssetDatabase.GetAssetPath(audioManager.MusicFolder.GetInstanceID()).ToString();
            audioManager.sounds.Clear();
            clips = Resources.LoadAll(audioManager.folder_path.Replace("Assets/Resources/", ""), typeof(AudioClip));
            foreach (AudioClip clip in clips)
            {
                Sound s = new Sound();
                s.clip = clip;
                s.name = clip.name;
                audioManager.sounds.Add(s);
            }
            EditorUtility.SetDirty(audioManager);
        }

        if (GUILayout.Button("Reset Orchestra"))
        {
            audioManager.ResetOrchestra();
        }
        if (GUILayout.Button("Reload Orchestra"))
        {
            audioManager.ReloadOrchestra();
        }

        base.OnInspectorGUI();

 

    }
}

