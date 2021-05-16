using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public Slider volumeSlider;
    public GameObject objectMusic;
    private float musicVolume = 0f;
    private AudioSource audioSourceMusic;

    // Start is called before the first frame update
    void Start()
    {
        objectMusic = GameObject.FindWithTag("Music");
        audioSourceMusic = objectMusic.GetComponent<AudioSource>();
        

        musicVolume = PlayerPrefs.GetFloat("volume");
        audioSourceMusic.volume = musicVolume;
        volumeSlider.value = musicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        audioSourceMusic.volume = musicVolume;
        PlayerPrefs.SetFloat("volume", musicVolume);
        

    }

    public void volumeUpdater(float volume)
    {
        musicVolume = volume;
    }
}
