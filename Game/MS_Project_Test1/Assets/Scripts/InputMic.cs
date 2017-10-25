using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class InputMic : MonoBehaviour {
    public float sensitivity = 100;
    public float loudness = 0;
    
    public GameObject target;


    AudioSource audio;
    void Start() {
        audio = GetComponent<AudioSource>();
        audio.clip = Microphone.Start(Microphone.devices[0], true, 1, 44100);
        audio.loop = true; // Set the AudioClip to loop
        audio.mute = true; // Mute the sound, we don't want the player to hear it
        while (!(Microphone.GetPosition(null) > 0)){} // Wait until the recording has started
        audio.Play(); // Play the audio source!
        
    }

    void Update(){
        loudness = GetAveragedVolume() * sensitivity;
        Debug.Log("Loudness is: " + loudness);
        if (loudness > 3) {
            Instantiate(target, new Vector3(GameObject.Find("SpawnPoint").transform.position.x, GameObject.Find("SpawnPoint").transform.position.y, GameObject.Find("SpawnPoint").transform.position.z), Quaternion.identity);
            target.AddComponent<HealthController>();
        }
    }

    float GetAveragedVolume()
    { 
        float[] data = new float[256];
        float a = 0;
        audio.clip.GetData(data,0);
        foreach(float s in data)
        {
            a += Mathf.Abs(s);
        }
        Debug.Log("A is :" + (a*100/256));
        return a/256;
    }
}