using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    public float interval = 1f;

    private List<IMetronomeObserver> observers;
    private AudioSource audioSource;


    void Awake()
    {
        observers = new List<IMetronomeObserver>();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Beat(1f));
    }
    void Update()
    {
        
    }

    IEnumerator Beat(float initialDelaySeconds)
    {
        yield return new WaitForSeconds(initialDelaySeconds);
        while (true)
        {
            NotifyObservers();
            audioSource.Play();
            yield return new WaitForSeconds(interval);
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

    private void NotifyObservers()
    {
        MetronomeTick tick = new MetronomeTick(interval);
        foreach (IMetronomeObserver observer in observers)
        {
            observer.Notify(tick);
        }
    }
}
