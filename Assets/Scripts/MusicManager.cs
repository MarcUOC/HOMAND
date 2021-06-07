using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider volumeSliderSoundEffects;
    public GameObject objectMusic;
    private float musicVolume = 0f;
    private float soundEffectsVolume = 0f;
    private AudioSource audioSourceMusic;
    public AudioSource ASSEMouseOver;
    public AudioSource ASSEMouseClick;
    public AudioSource ASSEFire;
    public AudioSource ASSEJump;
    public AudioSource ASSEOrb;
    public AudioSource ASSEHurt;
    public AudioSource ASSEInvokerDead;
    public AudioSource ASSEShooterDead;
    public AudioSource ASSEChaserDead;
    public AudioSource ASSEBossDead;
    public AudioSource ASSEFrozen;
    public AudioSource ASSEPauseGame;
    public AudioSource ASSEUnpauseGame;

    void Start()
    {
        objectMusic = GameObject.FindWithTag("Music");
        audioSourceMusic = objectMusic.GetComponent<AudioSource>();

        //PLAYERPREFS GETFLOAT
        audioSourceMusic.volume = musicVolume;
        musicVolume = PlayerPrefs.GetFloat("volume");
        soundEffectsVolume = PlayerPrefs.GetFloat("volumeSoundEffects");

        //ASSIGN VOLUME TO SLIDERS
        volumeSlider.value = musicVolume;
        volumeSliderSoundEffects.value = soundEffectsVolume;

        SEVolume();
    }

    void Update()
    {
        //PLAYERPREFS SETFLOAT
        audioSourceMusic.volume = musicVolume;        
        PlayerPrefs.SetFloat("volume", musicVolume);
        PlayerPrefs.SetFloat("volumeSoundEffects", soundEffectsVolume);

        SEVolume();
    }

    public void volumeUpdater(float volume)
    {
        musicVolume = volume; //ASSIGN VARIABLES
    }

    public void soundEffectsUpdate(float volumeSoundEffects)
    {
        soundEffectsVolume = volumeSoundEffects; //ASSIGN VARIABLES
    }

    //ALL SOUND EFFECTS AND MUSIC
    void SEVolume()
    {
        ASSEMouseOver.volume = soundEffectsVolume;
        ASSEMouseClick.volume = soundEffectsVolume;
        ASSEFire.volume = soundEffectsVolume;
        ASSEJump.volume = soundEffectsVolume;
        ASSEOrb.volume = soundEffectsVolume;
        ASSEHurt.volume = soundEffectsVolume;
        ASSEInvokerDead.volume = soundEffectsVolume;
        ASSEShooterDead.volume = soundEffectsVolume;
        ASSEChaserDead.volume = soundEffectsVolume;
        ASSEBossDead.volume = soundEffectsVolume;
        ASSEFrozen.volume = soundEffectsVolume;
        ASSEPauseGame.volume = soundEffectsVolume;
        ASSEUnpauseGame.volume = soundEffectsVolume;
    }
}
