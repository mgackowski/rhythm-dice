using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAudioController : MonoBehaviour
{
    public AudioSource melodyAudioSource;
    public AudioSource melodyAudioSource2; // alternate audio source to prevent audio pops
    public AudioSource soundAudioSource;
    public AudioSource beatAudioSource;

    public AudioClip[] melodyClips;
    public AudioClip bounceClip;
    public AudioClip damageClip;
    public AudioClip hitClip;
    public AudioClip healthPowerupClip;
    public AudioClip doublePowerupClip;

    private bool alternateSource = false;

    public enum SoundEffect
    {
        Rebound, DealDamage, TakeDamage, CollectHealth, CollectDouble
    }

    public void PlayBeat()
    {
        beatAudioSource.Play();
    }
    
    public void PlayTone(int value)
    {
        AudioSource currentSource, otherSource;
        if (alternateSource)
        {
            currentSource = melodyAudioSource;
            otherSource = melodyAudioSource2;
        }
        else
        {
            currentSource = melodyAudioSource2;
            otherSource = melodyAudioSource;
        }
        alternateSource = !alternateSource;

        // TODO: To prevent audio clipping, implement two alternating audio sources, and use volume = 0 instead of Stop()
        otherSource.volume = 0f;
        currentSource.clip = melodyClips[value];
        currentSource.volume = 1f;
        currentSource.Play();
    }
    public void PlayChord(SoundEffect sound)
    {
        AudioClip clipToPlay = null;
        switch (sound)
        {
            case SoundEffect.Rebound:
                clipToPlay = bounceClip;
                break;
            case SoundEffect.DealDamage:
                clipToPlay = hitClip;
                break;
            case SoundEffect.TakeDamage:
                clipToPlay = damageClip;
                break;
            case SoundEffect.CollectHealth:
                clipToPlay = healthPowerupClip;
                break;
            case SoundEffect.CollectDouble:
                clipToPlay = doublePowerupClip;
                break;
        }
        if (clipToPlay != null) {
            soundAudioSource.clip = clipToPlay;
            soundAudioSource.Play();
        }
    }

}
