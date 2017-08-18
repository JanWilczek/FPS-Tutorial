using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsInventory : MonoBehaviour {

    public GameObject[] gunsList = new GameObject[10];

    private bool[] guns = new bool[] { false, true, false, false, false, false, false, false, false, false };
    private KeyCode[] keys = new KeyCode[] { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };
    private int maxGuns = 1;
    private int currentGun = 1;

    public void addGun(int weaponNumber)
    {
        guns[weaponNumber] = true;
        maxGuns++;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i<keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]) && guns[i])
            {
                hideGuns();
                gunsList[i].SetActive(true);
                currentGun = i;
            }
        }
	}

    private void hideGuns()
    {
        for (int i = 1; i < maxGuns + 1; i++)
        {
            gunsList[i].SetActive(false);
        }
    }

    void addAmmo(Vector2 param)
    {
        GameObject gun = gunsList[(int)param.y];

        if (gun.GetComponent<Shooting>().canGetAmmo())
        {
            gun.SetActive(true);
            gun.SendMessage("addAmmo", param);
            if ((int) param.y != currentGun)
            {
                gun.SetActive(false);
            }
        }
    }


}
