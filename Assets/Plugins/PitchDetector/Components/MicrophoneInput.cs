using System.Collections.Generic;
using UnityEngine;

namespace FinerGames.PitchDetector
{
    public class MicrophoneInput : MonoBehaviour
    {
        public AudioSource Source;

        public string DeviceName;
        public bool IsRecording = false;

        public int SampleRate = 44100;
    }
}