using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class DSP : MonoBehaviour {
    public float[] testVal1 = new float[256];
    public float[] testVal2 = new float[256];

    public float[] cc2 = new float[(256 * 2) - 1];

    public int indexAtMax;
    public float max;
    public int ccEstimation;
    public float loudness = 0;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        loudness = GetAveragedVolume();
        
        {
            testVal1 = GameObject.Find("MicControllerC1").GetComponent<MicControlC>().GetRawValue();
            testVal2 = GameObject.Find("MicControllerC2").GetComponent<MicControlC>().GetRawValue();

            cc2 = MyCrossCorr(testVal1, testVal2);

            max = cc2.Max();
            indexAtMax = Array.IndexOf(cc2, max);
            ccEstimation = 256 - indexAtMax;
        }
        
    }

    float GetAveragedVolume()
    {
        float a = 0;
        foreach (float s in testVal1)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }

    float[] MyCrossCorr(float[] arr1, float[] arr2)
    {
        int lx = arr1.Length;
        int ly = arr2.Length;
        int jmin, jmax, index;
        index = 0;
        int lconv = lx + ly - 1;
        float[] z = new float[lconv];
        for (int i = 0; i <= lconv; i++)
        {
            if (i >= ly)
            {
                jmin = i - ly + 1;
            }
            else
            {
                jmin = 0;
            }

            if (i < lx)
            {
                jmax = i;
            }
            else
            {
                jmax = lx - 1;
            }

            for (int j = jmin; j <= jmax; j++)
            {
                index = ly - i + j - 1;
                z[i] = z[i] + (arr1[j] * arr2[index]);

            }
        }
        return z;
    }
}
