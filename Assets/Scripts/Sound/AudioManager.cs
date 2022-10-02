
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager: MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    private void Awake()
    {
        instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.speed = s.speed;
        }
    }
    
    public static void Play(string name)
    {
        Sound s = Array.Find(instance.sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogError("Sound: " + name + " not found");
            return;
        }

        s.source.Play();
        Debug.Log("playing " + name);
    }

    public static void PlayRandom(string[] names)
    {
        List<Sound> soundList = new List<Sound>();
        foreach(string name in names)
        {
            Sound s = Array.Find(instance.sounds, sound => sound.name == name);
            soundList.Add(s);
        }

        int rand = new System.Random().Next(0, soundList.Count);
        soundList[rand].source.Play();
    }
}
