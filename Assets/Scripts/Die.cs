using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour, IMetronomeObserver
{
    public Metronome metronome;

    public void Notify(MetronomeTick tick)
    {
        Debug.Log("Tick!");
    }

    // Start is called before the first frame update
    void Start()
    {
        metronome.GetComponent<Metronome>().AddObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Destroy()
    {
        metronome.GetComponent<Metronome>().RemoveObserver(this);
    }
}
