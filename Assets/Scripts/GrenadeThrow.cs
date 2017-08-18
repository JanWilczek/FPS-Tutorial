using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour {
    public Rigidbody grenade;

    private int currentGrenades = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F) && currentGrenades > 0)
        {
            Rigidbody clone = Instantiate(grenade, transform.position, transform.rotation) as Rigidbody;
            clone.AddForce(transform.TransformDirection(Vector3.forward * 1000));
            currentGrenades--;
        }
	}
}
