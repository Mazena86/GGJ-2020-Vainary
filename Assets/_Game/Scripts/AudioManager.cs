using System;
using UnityEngine;
using UnityEngine.UI;

/**
README

The Audio files need to be dropped into a list.
Make sure that the ptch != 0 so that the audio will play

Before calling any of the functions, make sure that the
isDone bool is true.
*/
public enum SoundType
{
    SFX = 0,
    Music = 1
}

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }
    [SerializeField] private Sound[] sounds;
    [SerializeField] private Image audioButton;
    [SerializeField] private Sprite isOnButtonSSprite; 
    [SerializeField] private Sprite isOffButtonSSprite; 

    private float musicVolume = 0.5f;
    private float sfxVolume = 0.5f;
    public bool isDone = false;
    private bool isOn = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.loop = sound.loop;
                sound.source.volume = musicVolume;
                sound.source.pitch = sound.pitch;
            }

            isDone = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleSound()
    {
        if(isOn)
        {
            StopAll();
            isOn = false;
            audioButton.sprite = isOffButtonSSprite;
        }
        else
        {
            isOn = true;
            audioButton.sprite = isOnButtonSSprite;
        }
    }

    public float MusicVolume
    {
        get { return musicVolume; }
        set
        {
            musicVolume = value;
            PlayerPrefs.SetFloat("musicVolume", value);
            foreach (Sound sound in sounds)
            {
                if (sound.type == SoundType.Music)
                {
                    sound.source.volume = value;
                }
            }
        }
    }

    public float SfxVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = value;
            PlayerPrefs.SetFloat("sfxVolume", value);
            foreach (Sound sound in sounds)
            {
                if (sound.type == SoundType.SFX)
                {
                    sound.source.volume = value;
                }
            }
        }
    }

    public void Play(string name)
    {
        if(isOn)
        {
            Sound sound = Array.Find(sounds, x => x.name == name);
            if (sound == null)
            {
                Debug.Log("Sound named " + name + " was not found.");
                return;
            }
            if (!sound.source.isPlaying)
            {
                //Debug.Log("Play audio: " + sound.name);
                sound.source.Play();
            }
        }
    }

    public void PlayOnce(string name, bool allowOverlapping = false)
    {
        if(isOn)
        {
            Sound sound = Array.Find(sounds, x => x.name == name);
            if (sound == null)
            {
                Debug.Log("Sound named " + name + " was not found.");
                return;
            }
            if (!sound.source.isPlaying || allowOverlapping)
            {
                sound.source.PlayOneShot(sound.clip);
            }
            // Debug.Log("Play audio: " + sound.name);
        }
    }

    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, x => x.name == name);
        if (sound == null)
        {
            Debug.Log("Sound named " + name + " was not found.");
            return;
        }
        sound.source.Stop();
        // Debug.Log("Stop audio: " + sound.name);
    }

    public void StopAll()
    {
        foreach(Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }

    public void Pause(string name)
    {
        Sound sound = Array.Find(sounds, x => x.name == name);
        if (sound == null)
        {
            Debug.Log("Sound named " + name + " was not found.");
            return;
        }
        sound.source.Pause();
        // Debug.Log("Paused audio: " + sound.name);
    }

    public void Resume(string name)
    {
        if(isOn)
        {
            Sound sound = Array.Find(sounds, x => x.name == name);
            if (sound == null)
            {
                Debug.Log("Sound named " + name + " was not found.");
                return;
            }
            sound.source.UnPause();
            // Debug.Log("Resumed audio: " + sound.name);
        }
    }
}

[Serializable]
public class Sound
{
    public string name;
    public SoundType type;
    public AudioClip clip;
    public bool loop;
    public float pitch = 1;
    [HideInInspector] public AudioSource source;
}
