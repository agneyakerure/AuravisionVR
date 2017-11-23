using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechTest : MonoBehaviour {

    [SerializeField]
    private string[] m_Keywords;

    private KeywordRecognizer m_recognizer;
    //public GameObject cube;
    private void Start()
    {
        m_Keywords = new string[5];
        m_Keywords[0] = "Go";
        m_Keywords[1] = "Goo";
        m_Keywords[2] = "Gooo";
        m_Keywords[3] = "Goooo";
        m_Keywords[4] = "Gooooo";
        m_recognizer = new KeywordRecognizer(m_Keywords);
        m_recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_recognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        //Debug.Log("Hello World!");
        if (args.text == m_Keywords[0] /*&& args.confidence > ConfidenceLevel.Low*/)
        {
            Debug.Log(args.text);
        }
    }

}
