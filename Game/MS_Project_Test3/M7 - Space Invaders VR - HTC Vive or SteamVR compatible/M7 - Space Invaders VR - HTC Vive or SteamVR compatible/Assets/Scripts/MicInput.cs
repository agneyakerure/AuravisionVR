﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicInput : MonoBehaviour {

    float spawnTimer;
    float spawnRate = 1f;

    private int sampleRate;
    private const int numSamples = 1024;
    private const int ccsize = 2047; //2*numSamples-1 CrossCorrelationSize


    public GameObject target;

    float[] _samples1, _samples2;
    float loudness;
    float sensitivity = 100;

    AudioSource aud1, aud2;
	// Use this for initialization
	void Start () {
        _samples1 = new float[numSamples];
        _samples2 = new float[numSamples];
        //foreach(string device in Microphone.devices)
        //{
        //    Debug.Log("Name: " + device);
        //}
        //AudioConfiguration acf = AudioSettings.GetConfiguration();
        //sampleRate = acf.sampleRate;
        //Debug.Log(sampleRate);
        aud1 = GetComponent<AudioSource>();
        aud2 = GetComponent<AudioSource>();

        aud1.clip = Microphone.Start(Microphone.devices[3], true, 1, 44100);
        aud2.clip = Microphone.Start(Microphone.devices[4], true, 1, 44100);
        aud1.loop = true;
        aud1.mute = false;
        aud2.loop = true;
        aud2.mute = false;
        //Debug.Log("Please work");
    }
	
	// Update is called once per frame
	void Update () {
        loudness = GetAverageVolume(aud1) * sensitivity;
        AnalyzeSound();
        //aud1.clip.GetData(_samples1, 0);
        //aud2.clip.GetData(_samples2, 0);


        if (loudness > 3)
        {
            if (spawnTimer < spawnRate)
            {
                return;
            }

            Instantiate(target, new Vector3(GameObject.Find("SpawnPoint").transform.position.x, GameObject.Find("SpawnPoint").transform.position.y, GameObject.Find("SpawnPoint").transform.position.z), Quaternion.identity);
            spawnTimer = 0.0f;
        }

        if (spawnTimer < spawnRate)
        {
            spawnTimer += Time.deltaTime;
        }
    }

    float GetAverageVolume(AudioSource aud)
    {
        float[] data = new float[256];
        float a = 0;
        aud.clip.GetData(data, 0);
        foreach (float s in data)
        {
            a = a + Mathf.Abs(s);
        }
        return a / 256;
    }

    void AnalyzeSound()
    {
        float[] cc = new float[ccsize];
        aud1.clip.GetData(_samples1, 0);
        //Debug.Log("Hello ji : " + _samples1);
        aud2.clip.GetData(_samples2, 0);
        cc = MyCrossCorr(_samples1, _samples2);
        Debug.Log("CROSS CORR: " + cc.ToString());
    }
    float[] MyCrossCorr(float[] arr1, float[] arr2)
    {
        int lx = arr1.Length;
        int ly = arr2.Length;
        int jmin, jmax, index;
        int lconv = lx + ly - 1;
        float[] z = new float[lconv];
        for (int i = 0; i < lconv; i++)
        {
            if (i >= ly)
            {
                jmin = i - ly + 1;
            }
            else
            {
                jmin = 1;
            }

            if (i < lx)
            {
                jmax = i;
            }
            else
            {
                jmax = lx;
            }

            for (int j = jmin; j <= jmax - 1; j++)
            {
                index = ly - i + j;
                z[i] = z[i] + (arr1[j] * arr2[index]);
            }
        }
        //Debug.Log(z);
        return z;
    }
}
