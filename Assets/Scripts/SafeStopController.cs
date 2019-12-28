using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SafeStopController : MonoBehaviour
{
    public UnityEvent hit;

    [SerializeField]
    private AudioClip stompSound;

    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        var stopper = transform.parent.Find("SafeStop");
        stopper.transform.position = new Vector3(0f, stopper.transform.position.y, stopper.transform.position.z);
        
        hit.AddListener(onHit);
    }

    private void onHit()
    {
        _player.GetComponent<AudioSource>().PlayOneShot(stompSound);
    }
}
