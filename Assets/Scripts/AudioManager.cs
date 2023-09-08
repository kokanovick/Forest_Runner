using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioSource[] audioSource;
    [SerializeField] AudioSource[] backgroundMusic;
    private int backgroundMusicIndex;
    private void Update()
    {
        if (!backgroundMusic[backgroundMusicIndex].isPlaying)
        {
            PlayRandomBakcgroundMusic();
        }
    }
    private void Awake()
    {
        instance= this;
    }
    public void PlayRandomBakcgroundMusic()
    {
        backgroundMusicIndex = Random.Range(0, backgroundMusic.Length);
        PlayBackgroundMusic(backgroundMusicIndex);
    }
    public void PlayAudio(int index)
    {
        if(index < audioSource.Length) 
        {
            audioSource[index].pitch = Random.Range(.85f, 1.1f);
            audioSource[index].Play();
        }
    }
    public void StopAudio(int index) 
    {
        audioSource[index].Stop();
    }

    public void PlayBackgroundMusic(int index)
    {
        StopBackgroundMusic();
        backgroundMusic[index].Play();
    }
    public void StopBackgroundMusic()
    {
        for (int i = 0; i < backgroundMusic.Length; i++)
        {
            backgroundMusic[i].Stop();
        }
    }
}
