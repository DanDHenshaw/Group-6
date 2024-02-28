using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiotimer : MonoBehaviour
{
   public AudioSource Sound;
    public int chance;
    int  Cap;

    // Start is called before the first frame update
    void Start()
    {
        Cap = 100;
        InvokeRepeating("TimerLoop", 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TimerLoop()
    {
        int X = Random.Range(1, Cap);
        if ( X < chance)
        {
            if(Sound != null)
            {
                Sound.Play();
            }
        }
    }
}
