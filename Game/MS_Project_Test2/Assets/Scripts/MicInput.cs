using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
public class MicInput : MonoBehaviour {

    public float sensitivity = 100;
    public float loudness1 = 0;
    public float loudness2 = 0;
    public GameObject target;

    AudioSource aud1, aud2;

    // Use this for initialization
    void Start()
    {
        foreach(string device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
        aud1 = GetComponent<AudioSource>();
        aud2 = GetComponent<AudioSource>();
        aud1.clip = Microphone.Start(Microphone.devices[3], true, 1, 44100);
        aud2.clip = Microphone.Start(Microphone.devices[4], true, 1, 44100);
        aud1.loop = true;
        aud1.mute = false;
        aud2.loop = true;
        aud2.mute = false;
        //aud1.Play();
    }

    void Update()
    {
        loudness1 = GetAverageVolume(aud1) * sensitivity;
        loudness2 = GetAverageVolume(aud2) * sensitivity;
        Debug.Log("Loudness1: " + loudness1);
        Debug.Log("Loudness2: " + loudness2);
    }

    float GetAverageVolume(AudioSource aud)
    {
        float[] data = new float[256];
        float a = 0;
        aud.clip.GetData(data, 0);
        foreach(float s in data)
        {
            a = a + Mathf.Abs(s);
        }
        return a / 256;
    }
}
