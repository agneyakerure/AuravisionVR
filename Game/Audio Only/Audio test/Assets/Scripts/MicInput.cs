using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using FFTWSharp;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]

public class MicInput : MonoBehaviour {
    AudioSource aud1, aud2;
    int signalLength = 256;
    public float[] _samples1 = new float[256];
    public float[] _samples2 = new float[256];

    public double[] _samples1d = new double[256];
    public double[] _samples2d = new double[256];

    public double[] _newSamples1 = new double[256];
    public double[] gcccorrelation = new double[(256 * 2) - 1];
    //public double[] _newSamples2 = new double[512];

    public float[] cc2 = new float[(256 * 2) - 1];

    public double[] dft = new double[5];

    public float gcoco;

    // Use this for initialization
    void Start ()
    {
        foreach (string device in Microphone.devices)
        {
            Debug.Log(device);
            //Debug.Log(Microphone.devices[3]);
        }
        aud1 = GetComponent<AudioSource>();
        aud1.clip = Microphone.Start(Microphone.devices[1], true, 1, 16000);
        aud1.loop = true;
        aud1.mute = false;

        aud2 = GetComponent<AudioSource>();
        aud2.clip = Microphone.Start(Microphone.devices[3], true, 1, 16000);
        aud2.loop = true;
        aud2.mute = false;

        //float[] temp1 = { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };
        //float[] temp2 = { 2.0f, 3.0f, 4.0f, 5.0f, 6.0f };
        //var cc3 = MyCrossCorr(temp1, temp2);
        //cc2 = cc3;
        //dft = FFT(temp1, true);
        //displayComplex(dft);
    }
	
	// Update is called once per frame
	void Update () {
        //var isSamplePos = false;
        aud1.clip.GetData(_samples1, 0);
        aud2.clip.GetData(_samples2, 1);

        //_samples1d = Array.ConvertAll(_samples1, x => (double)x);
        //_samples2d = Array.ConvertAll(_samples2, x => (double)x);

        cc2 = MyCrossCorr(_samples1, _samples2);

        int gccLen = (signalLength * 2) - 1;
        int phatLen = gccLen;
        _newSamples1 = Array.ConvertAll(cc2, x => (double)x);
        dft = FFT(_newSamples1, true);
        double[] absDFT = new double[dft.Length / 2];
        absDFT = absval(dft);
        double[] gcc = new double[(signalLength * 2) - 1];
        for (int i = 0, j = 0; i < gccLen; i++)
        {
            gcc[i] = dft[i] / absDFT[j];
            if ((i % 2 == 0) && (i != 0))
            {
                j = j + 1;
            }
        }

        gcccorrelation = IFFT(gcc);
        for (int i = 0, j = 0; i < gccLen; i++)
        {
            gcccorrelation[i] = Math.Abs(gcccorrelation[i]);
        }

        var indexAtMax = gcccorrelation.ToList().IndexOf(gcccorrelation.Max());

        int gccEstimation = 256 - indexAtMax;

        gcoco = ((gccEstimation + 10) * 180) / 20;

        //int maxIndex = cc2.ToList().IndexOf(cc2.Max());

        //_newSamples1 = Array.ConvertAll(_samples1, x => (double)x);
        //_newSamples2 = Array.ConvertAll(_samples2, x => (double)x);


    }

    static double[] toComplex(double[] real)
    {
        int n = real.Length;
        var comp = new double[n * 2];
        for(int i = 0; i < n; i++)
        {
            comp[2 * i] = real[i];
        }
        return comp;
    }

    static void displayComplex(double[] x)
    {
        if (x.Length % 2 != 0)
            throw new Exception("not even");
        for(int i = 0, n = x.Length; i < n; i = i + 2)
        {
            if(x[i+1] < 0)
            {
                Debug.Log(string.Format("{0} - {1}i", x[i], Math.Abs(x[i + 1])));
            }
            else
            {
                Debug.Log(string.Format("{0} + {1}i", x[i], x[i + 1]));
            }
        }
    }

    static double[] absval(double[] x)
    {
        double[] ans = new double[x.Length/2];
        if (x.Length % 2 != 0)
            throw new Exception("not even");
        for (int i = 0, j = 0, n = x.Length; i < n; i = i + 2, j = j + 1)
        {
            ans[j] = Math.Sqrt((x[i] * x[i]) + (x[i + 1] * x[i + 1]));
        }
        return ans;
    }


    static void displayReal(double[] x)
    {
        if (x.Length % 2 != 0)
            throw new Exception("not even");
        for (int i = 0, n = x.Length; i < n; i = i + 2)
        {
                Debug.Log(x[i]);
        }
    }

    static double[] FFT(double[] data, bool real)
    {
        if (real)
            data = toComplex(data);
        int n = data.Length;
        IntPtr ptr = fftw.malloc(n * 8);
        Marshal.Copy(data, 0, ptr, n);
        IntPtr plan = fftw.dft_1d(n / 2, ptr, ptr, fftw_direction.Forward, fftw_flags.Estimate);
        fftw.execute(plan);
        var fft = new double[n];
        Marshal.Copy(ptr, fft, 0, n);
        fftw.destroy_plan(plan);
        fftw.free(ptr);
        fftw.cleanup();
        return fft;

    }

    static double[] IFFT(double[] data)
    {
        int n = data.Length;
        IntPtr ptr = fftw.malloc(n * 8);
        Marshal.Copy(data, 0, ptr, n);
        IntPtr plan = fftw.dft_1d(n / 2, ptr, ptr, fftw_direction.Backward, fftw_flags.Estimate);
        fftw.execute(plan);
        var ifft = new double[n];
        Marshal.Copy(ptr, ifft, 0, n);
        fftw.destroy_plan(plan);
        fftw.free(ptr);
        fftw.cleanup();
        for (int i =0, nh = n / 2; i < n; i++)
        {
            ifft[i] /= nh;
        }
        return ifft;

    }

    float[] MyCrossCorr(float[] arr1, float[] arr2)
    {
        int lx = arr1.Length;
        int ly = arr2.Length;
        int jmin, jmax, index;
        index = 0;
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
        //Debug.Log(z);
        //var z = IFFT(FFT(arr1, true) * FFT(arr2, true));

        return z;
    }
}
