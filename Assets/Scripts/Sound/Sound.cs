using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume=0;
    [Range(.1f, 3f)]
    public float pitch=1;
    public float randomPitchFac = 0;
    public bool loop=true;

    [HideInInspector]
    public AudioSource source;

    public List<AudioClip> altClips;
    
    public void init(GameObject audioManager)
    {
        if (altClips == null)
        {
            altClips = new List<AudioClip>();
        }
        altClips.Add(clip);
        source = audioManager.AddComponent<AudioSource>();
        source.volume = volume;
        source.pitch = pitch; 
        source.loop = loop;
    }

    public void play()
    {
        int clipId = Mathf.RoundToInt(Random.Range(0f, altClips.Count-1));
        // Debug.Log(name + " num clips: " + altClips.Count + " rand " + clipId);
        source.clip = altClips[clipId];
        // if (randomPitchFac != 0)
        // {
        //     float random = Random.Range(0f, 1f) * randomPitchFac;
        // }
        source.Play();
    }
    
}
