using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IMetronomeObserver
{
    public int attackPower = 0;
    private Metronome metronome;
    private AudioSource audioSource;

    public virtual void PreNotify(MetronomeTick tick)
    {
        //Debug.Log("Menacing Pre-tick!");
    }

    public virtual void Notify(MetronomeTick tick)
    {
        //Debug.Log("Menacing tick!");
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        metronome = GameObject.FindGameObjectWithTag("Metronome").GetComponent<Metronome>();
        metronome.AddObserver(this);
        audioSource = GetComponent<AudioSource>();

    }

    private void OnDestroy()
    {
        metronome.RemoveObserver(this);
    }

    public void playSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

}
