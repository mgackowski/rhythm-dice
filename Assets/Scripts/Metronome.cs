using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class Metronome : MonoBehaviour
{
    public float initialDelaySeconds = 1f;
    public float interval = 1f;
    public float leadUpTime = 0.33f;
    public bool playTensionClip = false;

    public AudioClip[] beatClip;
    public AudioClip tensionClip;

    private List<IMetronomeObserver> observers;
    private List<IMetronomeObserver> lateObservers;
    private AudioSource[] audioSource; // use two sources to alternate beat and prevent audio crackle
    private int beatNumber = 0;


    void Awake()
    {
        observers = new List<IMetronomeObserver>();
        lateObservers = new List<IMetronomeObserver>();
    }

    private void Start()
    {
        audioSource = GetComponents<AudioSource>();
        //StartCoroutine(Beat(initialDelaySeconds));

        InvokeRepeating("PreBeatThenBeat", initialDelaySeconds, interval);
    }
    void Update()
    {
        
    }

    private void PreBeatThenBeat()
    {
        if (Mathf.Approximately(interval, float.MaxValue)) return;

        float intervalPreBeat = interval * (1f - leadUpTime); ;
        PreNotifyObservers();
        Invoke("Beat", intervalPreBeat);

    }

    private void Beat()
    {
        NotifyObservers();
        if (playTensionClip)
        {
            audioSource[beatNumber].clip = tensionClip;
            playTensionClip = false;
        }
        else
        {
            audioSource[beatNumber].clip = beatClip[beatNumber];
        }
        audioSource[(beatNumber + 1) % 2].volume = 0f;
        audioSource[beatNumber].volume = 1f;
        audioSource[beatNumber].Play();
        beatNumber = (beatNumber + 1) % beatClip.Length;
    }

    /* Override next beat with a different audio clip set as the 'tensionClip' */
    public void SetPlayTensionClip()
    {
        playTensionClip = true;
    }

    public void StopBeat()
    {
        CancelInvoke("PreBeatThenBeat");
    }

    /*IEnumerator Beat(float initialDelaySeconds)
    {
        float intervalPreBeat;
        float intervalPostBeat;
        yield return new WaitForSeconds(initialDelaySeconds);
        while (true)
        {
            intervalPreBeat = interval * (1f - leadUpTime);
            intervalPostBeat = interval * leadUpTime;
            PreNotifyObservers();
            yield return new WaitForSeconds(intervalPreBeat);
            NotifyObservers();
            if(playTensionClip) {
                audioSource.clip = tensionClip;
                playTensionClip = false;
            }
            else
            {
                audioSource.clip = beatClip[beatNumber];
            }
            audioSource.Play();
            beatNumber = (beatNumber + 1) % beatClip.Length;
            yield return new WaitForSeconds(intervalPostBeat);
        }
    }*/

    public void AddObserver(IMetronomeObserver observer)
    {
        observers.Add(observer);
    }

    public void AddLateObserver(IMetronomeObserver observer)
    {
        lateObservers.Add(observer);
    }

    public void RemoveObserver(IMetronomeObserver observer)
    {
        observers.Remove(observer);
        lateObservers.Remove(observer);
    }

    private void PreNotifyObservers()
    {
        MetronomeTick tick = new MetronomeTick(interval);
        foreach (IMetronomeObserver observer in observers)
        {
            observer.PreNotify(tick);
        }
    }

    private void NotifyObservers()
    {
        MetronomeTick tick = new MetronomeTick(interval);
        foreach (IMetronomeObserver observer in observers)
        {
            observer.Notify(tick);
        }
        foreach (IMetronomeObserver observer in lateObservers)  // use late observers for player after game state is updated
        {
            observer.Notify(tick);
        }
    }

}
