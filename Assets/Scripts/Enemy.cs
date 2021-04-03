using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IMetronomeObserver
{
    public int attackPower = 0;
    public bool alive = true;
    public GameObject atkIndicatorPrefab;

    private Metronome metronome;
    
    protected  AudioSource audioSource;
    protected Animator animator;

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
        animator = GetComponentInChildren<Animator>();

        //Vector3 atkIndicatorPosition = transform.position;
        //atkIndicatorPosition.z -= 2f;
        //GameObject atkIndicator = Instantiate(atkIndicatorPrefab,atkIndicatorPosition,Quaternion.Euler(-90f,0f,0f),transform);
        //atkIndicator.GetComponent<TextMesh>().text = attackPower + "";

    }

    public void GetSquashed()
    {
        if (animator != null & alive)
        {
            animator.SetTrigger("GetSquashedTrigger");
            alive = false;

            // Trying something different
            //metronome.RemoveObserver(this);
            //Rigidbody thisRigidbody = gameObject.AddComponent<Rigidbody>();
            //thisRigidbody.AddExplosionForce(10f, transform.position + Vector3.forward, 2f);
            //this.enabled = false;

        }
    }

    public void Bounce()
    {
        //TODO: Investigate if this could be causing animation errors
        //if (animator != null & alive) animator.SetTrigger("BounceTrigger");
    }

    private void OnDestroy()
    {
        metronome.RemoveObserver(this);
    }

    public void playSound()
    {
        if (audioSource != null && alive)
        {
            audioSource.Play();
        }
    }


}
