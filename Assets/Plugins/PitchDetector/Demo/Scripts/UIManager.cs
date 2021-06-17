using FinerGames.PitchDetector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] MicrophoneInput microInput;
    [SerializeField] Graph graph;
    [SerializeField] Text score;
    [SerializeField] Text buttonText;

    private LineRenderer lineRenderer;
    private int counter = 0;

    private AudioSource source;

    //temporary audio vector we write to every second while recording is enabled..
    List<float> tempRecording = new List<float>();

    private void Start()
    {
        lineRenderer = graph.GetComponent<LineRenderer>();
        source = GetComponent<AudioSource>();
    }

    public void ChangeText()
    {
        counter = (counter + 1)%2;
        if (counter == 1)
        {
            buttonText.text = "Arrêter";
            graph.Reset();
            score.text = "";
            microInput.IsRecording = true;
            tempRecording.Clear();
            Invoke(nameof(ResizeRecording), 1);
        }
        else
        {
            buttonText.text = "Démarrer";
            microInput.IsRecording = false;

            //stop recording, get length, create a new array of samples
            int length = Microphone.GetPosition(null);

            float[] clipData = new float[length];
            microInput.Source.clip.GetData(clipData, 0);

            //create a larger vector that will have enough space to hold our temporary
            //recording, and the last section of the current recording
            float[] fullClip = new float[clipData.Length + tempRecording.Count];
            for (int i = 0; i < fullClip.Length; i++)
            {
                //write data all recorded data to fullCLip vector
                if (i < tempRecording.Count)
                    fullClip[i] = tempRecording[i];
                else
                    fullClip[i] = clipData[i - tempRecording.Count];
            }

            source.clip = AudioClip.Create("recorded samples", fullClip.Length, 1, 44100, false);
            source.clip.SetData(fullClip, 0);

            // Calculs 
         
            int numberCount = lineRenderer.positionCount;
            if (numberCount > 0)
            {
                Vector3[] positions = new Vector3[numberCount];
                lineRenderer.GetPositions(positions);
                double[] freq = new double[numberCount];
                for (int i = 0; i < numberCount; i++)
                {
                    freq[i] = positions[i].y;
                }

                // Médiane

                int halfIndex = numberCount / 2;
                List<double> sortedNumbers = freq.OrderBy(n => n).ToList();
                double median;
                if ((numberCount % 2) == 0)
                {
                    median = (sortedNumbers[halfIndex] + sortedNumbers[halfIndex - 1]) / 2;
                }
                else
                {
                    median = sortedNumbers[halfIndex];
                }
                Debug.Log(median);
                score.text = Math.Round(freq[numberCount - 1] - median, 1).ToString();
            }
            
        }
    }

    void ResizeRecording()
    {
        if (microInput.IsRecording)
        {
            //add the next second of recorded audio to temp vector
            int length = 44100;
            float[] clipData = new float[length];
            microInput.Source.clip.GetData(clipData, 0);
            tempRecording.AddRange(clipData);
            Invoke(nameof(ResizeRecording), 1);
        }
    }

    public void Replay()
    {
        source.Play();
    }
}
