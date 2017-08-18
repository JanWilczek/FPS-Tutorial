using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Shooting : MonoBehaviour {
    // Variables
    public Texture2D crosshairTexture;
    public AudioClip pistolShot;
    public AudioClip reloadSound;
    public int maxAmmo = 100;
    public int clipSize = 10;
    public GUIText ammoText;
    public float reloadTime = 2.0f;
    public GUIText reloadText;
    public float range = 20.0f;
    public bool automatic = false;
    public float shotDelay = 0.5f;
    public GameObject bulletHole;
    public GameObject bloodParticles;
    public float damage;

    private float zoomFieldOfView = 40.0f;
    private float defaultFieldOfView = 60.0f;
    private float shotDelayCounter = 0.0f;
    private int currentAmmo = 30;
    private int currentClip;
    private Rect position;
    private GameObject pistolSparks;
    private Vector3 fwd;
    private RaycastHit hit;
    private bool isReloading = false;

    private float timer = 0.0f;


	// Use this for initialization
	void Start () {
        Cursor.visible = false;
        position = new Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height - crosshairTexture.height) / 2, crosshairTexture.width, crosshairTexture.height);

        pistolSparks = GameObject.Find("Sparks");
        pistolSparks.GetComponent<ParticleEmitter>().emit = false;
        GetComponent<AudioSource>().clip = pistolShot;

        currentClip = clipSize;
	}

    // GUI appearance
    void OnGUI()
    {
        GUI.DrawTexture(position, crosshairTexture);
        ammoText.pixelOffset = new Vector2(-Screen.width / 2 + 100, -Screen.height / 2 + 30);
        ammoText.text = currentClip + "/" + currentAmmo;

        if (currentClip == 0)
        {
            reloadText.enabled = true;
        }
        else
        {
            reloadText.enabled = false;
        }
    }

    // Update is called once per frame
    void Update () {
        fwd = transform.TransformDirection(Vector3.forward);

        if (currentClip>0 && !isReloading)
        {
            if ((Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && automatic)) && shotDelayCounter <= 0)
            {
                shotDelayCounter = shotDelay;
                currentClip--;
                pistolSparks.GetComponent<ParticleEmitter>().Emit();
                GetComponent<AudioSource>().Play();

                if (Physics.Raycast(transform.position, fwd, out hit))
                {
                    if (hit.transform.tag == "Enemy" && hit.distance < range)
                    {
                        Debug.Log("Enemy hit in distance " + hit.distance);
                        hit.transform.SendMessage("TakeHit", damage);
                        GameObject go;
                        go = Instantiate(bloodParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
                        Destroy(go, 0.3f);
                    }
                    else if (hit.distance < range)
                    {
                        Debug.Log("Wall hit in distance " + hit.distance);
                        GameObject go;
                        go = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
                        Destroy(go, 5);
                    }
                }
            }
        }

        if (shotDelayCounter > 0) shotDelayCounter -= Time.deltaTime;

        if ((Input.GetButtonDown("Fire1") && currentClip == 0) ||(Input.GetButtonDown("Reload") && currentClip < clipSize))
        {
            if (currentAmmo > 0)
            {
                GetComponent<AudioSource>().clip = reloadSound;
                GetComponent<AudioSource>().Play();
                isReloading = true;
            }
        }

        if (isReloading)
        {
            /*  //Ignore this part
            if (gameObject.name== "MachineGun_handle")
            {
                Transform child = gameObject.transform.Find("MachineGun_00");
                child.SendMessage("PlayAnimation","MachineGun_reload");
            }
            */

            timer += Time.deltaTime;
            if (timer >= reloadTime)
            {
                int needAmmo = clipSize - currentClip;
                if (currentAmmo >= needAmmo)
                {
                    currentClip = clipSize;
                    currentAmmo -= needAmmo;
                }
                else
                {
                    currentClip = currentAmmo;
                    currentAmmo = 0;
                }

                GetComponent<AudioSource>().clip = pistolShot;
                isReloading = false;
                timer = 0.0f;
            }
        }
        if (gameObject.GetComponentInParent<Camera>() is Camera)
        {
            Camera cam = gameObject.GetComponentInParent<Camera>();
            if (Input.GetButton("Fire2"))
            {
                if (cam.fieldOfView > zoomFieldOfView)
                {
                    cam.fieldOfView--;
                }
            }
            else
            {
                if (cam.fieldOfView < defaultFieldOfView) cam.fieldOfView++;
            }

        }
    }


    // These two methods should be implemented in GunsInventory.cs so that at any time the appriopriate ammo type can be added to the appriopriate weapon.
    // Right now these methods won't work, when an other type of weapon than "Gun" is being used.
    public bool canGetAmmo()
    {
        return currentAmmo < maxAmmo;
    }

    void addAmmo(Vector2 data)
    {
        if (currentAmmo + (int) data.x >= maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
        else
        {
            currentAmmo += (int) data.x;
        }
    }

    
}
