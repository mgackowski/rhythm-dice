using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IMetronomeObserver
{
    public int attackPower = 0;
    public Metronome metronome;

    public void Notify(MetronomeTick tick)
    {
        Debug.Log("Menacing tick!");
    }

    // Start is called before the first frame update
    void Start()
    {
        metronome.AddObserver(this);
    }

    private void OnDestroy()
    {
        metronome.RemoveObserver(this);
    }

}
