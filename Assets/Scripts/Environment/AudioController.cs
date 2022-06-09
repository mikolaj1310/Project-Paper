using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource MusicBase;
    public AudioSource BossMusic;

    // Start is called before the first frame update
    void Start()
    {
        BaseMusic();
    }

    public void BaseMusic()
    {
        MusicBase.Play();
        BossMusic.Stop();
    }

    public void PlayBoss()
    {
        print("Music");
        MusicBase.Stop();
        BossMusic.Play();
    }
}
