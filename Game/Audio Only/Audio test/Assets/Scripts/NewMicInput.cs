using UnityEngine;
using System.Collections;

public class NewMicInput : MonoBehaviour
{

    public int micIndex = 0;
    public string thisMicName = "";
    private AudioSource myAS;

    void Start()
    {
        myAS = GetComponent<AudioSource>();
    }

    void OnGUI()
    {
        if (myAS != null)
        {
            if (GUI.Button(new Rect(micIndex * 100, 0, 100, 30), micIndex.ToString()))
                StartCoroutine("listenToMic");
        }
    }

    IEnumerator listenToMic()
    {
        if (Microphone.devices.Length < micIndex + 1)
        {
            Debug.Log("mic index " + micIndex + " doesn't exist");
            yield break;
        }

        thisMicName = Microphone.devices[micIndex];
        int minf;
        int maxf;
        Microphone.GetDeviceCaps(thisMicName, out minf, out maxf);
        Debug.Log("listenToMic started for player " + micIndex + " : " + thisMicName + ":" + minf + ":" + maxf);

        int recLength = 5 * 60;
        myAS.clip = Microphone.Start(thisMicName, false, recLength, 44100);
        if (myAS.clip == null)
        {
            Debug.Log("mic start failed for: " + thisMicName);
            yield break;
        }

        float maxwait = Time.time + 3f;
        while (!(Microphone.GetPosition(thisMicName) > 0))
        { // Wait until the recording has started
            if (Time.time > maxwait)
            {
                Debug.Log("Mic position not found for: " + thisMicName);
                yield break;
            }
        }

        myAS.Play();
        float CloseTime = Time.time + recLength;
        while (Microphone.IsRecording(thisMicName))
        {
            yield return 1;
            if (Time.time >= CloseTime)
            {
                Debug.Log("listenToMic 1 Microphone.End");
                Microphone.End(thisMicName);
                myAS.Stop();
                myAS.clip = null;

                yield break;
            }
        }
        Debug.Log("listenToMic 2 Microphone.End");
        Microphone.End(thisMicName);
        myAS.Stop();
        myAS.clip = null;
    }
}