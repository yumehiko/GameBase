using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.Audio.Music
{
    /// <summary>
    /// 一曲の音楽。
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/MusicClip")]
    public class MusicClip : ScriptableObject
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private string title;
        [SerializeField] private string composer;
        [SerializeField] private float bpm = -1.0f;

        public AudioClip Clip => clip;
        public string Title => title;
        public string Composer => composer;
        public float Length => clip.length;
        public float BPM => bpm;
    }
}