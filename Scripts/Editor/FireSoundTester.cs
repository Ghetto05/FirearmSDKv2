using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class FireSoundTester : MonoBehaviour
    {
        private List<List<AudioSource>> _audioSets = new();
        public List<AudioSource> set0;
        public List<AudioSource> set1;
        public List<AudioSource> set2;
        public List<AudioSource> set3;
        public List<AudioSource> set4;
        public List<AudioSource> set5;
        public List<AudioSource> set6;
        public List<AudioSource> set7;
        public List<AudioSource> set8;
        public List<AudioSource> set9;
        public int currentSet = 0;
        public float rpm;
        public bool automatic;
        private bool _playing;
        private float _lastPlayTime = 0f;

        private void Start()
        {
            if (set0.Any())
                _audioSets.Add(set0);
            if (set1.Any())
                _audioSets.Add(set1);
            if (set2.Any())
                _audioSets.Add(set2);
            if (set3.Any())
                _audioSets.Add(set3);
            if (set4.Any())
                _audioSets.Add(set4);
            if (set5.Any())
                _audioSets.Add(set5);
            if (set6.Any())
                _audioSets.Add(set6);
            if (set7.Any())
                _audioSets.Add(set7);
            if (set8.Any())
                _audioSets.Add(set8);
            if (set9.Any())
                _audioSets.Add(set9);
        }

        [EasyButtons.Button]
        public void Play()
        {
            if (automatic)
                _playing = true;
            else
                Util.PlayRandomAudioSource(_audioSets[currentSet]);
        }

        [EasyButtons.Button]
        public void Stop()
        {
            _playing = false;
        }

        [EasyButtons.Button]
        public void Next()
        {
            currentSet++;
            if (currentSet >= _audioSets.Count)
                currentSet = 0;
        }

        [EasyButtons.Button]
        public void Previous()
        {
            if (currentSet == 0)
                currentSet = _audioSets.Count;
            currentSet--;
        }

        private void Update()
        {
            if (_playing)
            {
                float timeBetweenShot = 60 / rpm;
                if (Time.time - _lastPlayTime >= timeBetweenShot)
                {
                    Util.PlayRandomAudioSource(_audioSets[currentSet]);
                    _lastPlayTime = Time.time;
                }
            }
        }
    }
}
