using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAudioController : MonoBehaviour
{
    public AudioSource melodyAudioSource;
    public AudioSource soundAudioSource;
    public AudioSource beatAudioSource;

    public AudioClip[] melodyClips;
    public AudioClip bounceClip;
    public AudioClip damageClip;
    public AudioClip hitClip;
    public AudioClip healthPowerupClip;
    public AudioClip doublePowerupClip;

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
        // TODO: To prevent audio clipping, implement two alternating audio sources, and use volume = 0 instead of Stop()
        melodyAudioSource.clip = melodyClips[value];
        melodyAudioSource.Play();
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
