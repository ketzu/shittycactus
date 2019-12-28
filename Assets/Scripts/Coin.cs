using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private AudioClip collectionsound;

    private Scores _scorekeeper;
    private AudioSource _audio;

    void Start()
    {
        _scorekeeper = GameObject.FindObjectOfType<Scores>();
        _audio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _scorekeeper.collectCoin();
            _audio.PlayOneShot(collectionsound);
            Destroy(gameObject);
        }
    }
}
