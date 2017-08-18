using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {
    public float toExplode = 4.0f;
    public GameObject explosion;
    public AudioClip explosionSound;
    public AudioSource explosionHolder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        toExplode -= Time.deltaTime;
        
        if (toExplode<=0)
        {
            AudioSource explosionS = Instantiate(explosionHolder, transform.position, transform.rotation) as AudioSource;
            explosionS.clip = explosionSound;
            explosionS.Play();
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
	}
}
