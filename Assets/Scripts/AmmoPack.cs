using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour {

    public float ammunition = 25.0f;
    public float gunType = 1.0f;

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GunsInventory inventory = other.GetComponent<GunsInventory>();
            inventory.SendMessage("addAmmo", new Vector2(ammunition, gunType));
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(new Vector3(0, 1, 0));
	}
}
