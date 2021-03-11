using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    public float initialDelaySeconds = 1f;
    public float interval = 1f;
    public float leadUpTime = 0.33f;

    private List<IMetronomeObserver> observers;
    private AudioSource audioSource;


    void Awake()
    {
        observers = new List<IMetronomeObserver>();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Beat(initialDelaySeconds));
    }
    void Update()
    {
        
    }

    IEnumerator Beat(float initialDelaySeconds)
    {
        float intervalPreBeat;
        float intervalPostBeat;
        yield return new WaitForSeconds(initialDelaySeconds);
        while (true)
        {
            intervalPreBeat = interval * (1f - leadUpTime);
            intervalPostBeat = interval * leadUpTime;
            PreNotifyObservers();
            //audioSource.Play();
            yield return new WaitForSeconds(intervalPreBeat);
            NotifyObservers();
            audioSource.Play();
            yield return new WaitForSeconds(intervalPostBeat);
        }
    }

    public void AddObserver(IMetronomeObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IMetronomeObserver observer)
    {
        observers.Remove(observer);
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
    }
}
