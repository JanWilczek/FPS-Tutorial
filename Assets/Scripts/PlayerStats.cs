using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    // Player stats
    private float maxHealth = 100;
    private float currentHealth = 100;
    private float maxStamina = 100;
    private float currentStamina = 100;
    private float maxArmour = 100;
    private float currentArmour = 100;

    // Player status
    public float runSpeed = 20.0f;
    public float walkSpeed = 10.0f;
    private float canHeal = 0.0f;
    private float canRegenerate = 0.0f;

    // Textures for bars: health bar etc.
    public Texture2D healthTexture;
    public Texture2D staminaTexture;
    public Texture2D armourTexture;

    // Bars
    private float barWidth;
    private float barHeight;

    //  Controllers
    private CharacterController chCont;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsC;

    // Additional members
    private Vector3 lastPosition;

    // This will execute before Start
    private void Awake()
    {
        // assign the controllers
        chCont = GetComponent<CharacterController>();
        fpsC = gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();

        // set bar size
        barHeight = Screen.height * 0.02f;
        barWidth = barHeight * 10.0f;

        // initialize lastPosition
        lastPosition = transform.position;
    }

    // GUI part
    void OnGUI()
    {
        // Draws health, stamina and armour bars on the screen in the given coordinates
        GUI.DrawTexture(new Rect(Screen.width - barWidth - 10, Screen.height - barHeight * 4 - 60, barWidth * currentHealth / maxHealth, barHeight), healthTexture);
        GUI.DrawTexture(new Rect(Screen.width - barWidth - 10, Screen.height - barHeight * 3 - 50, barWidth * currentArmour / maxArmour, barHeight), armourTexture);
        GUI.DrawTexture(new Rect(Screen.width - barWidth - 10, Screen.height - barHeight * 2 - 40, barWidth * currentStamina / maxStamina, barHeight), staminaTexture);
    }

    // TakeHit function decreases armour or health (when armour<0 ) depending on the given damage 
    void TakeHit(float damage)
    {
        if (currentArmour >= damage)
        {
            currentArmour -= damage;
        }
        else
        {
            currentHealth += (currentArmour - damage);
            currentArmour = 0;
        }

        if (currentHealth < maxHealth)
        {
            canHeal = 3.0f;
        }

       currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
       currentArmour = Mathf.Clamp(currentArmour, 0, maxArmour);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P))
        {
            TakeHit(30);
        }

        if (canHeal > 0.0f)
        {
            canHeal -= Time.deltaTime;
        }
        if (canRegenerate > 0.0f)
        {
            canRegenerate -= Time.deltaTime;
        }

        if (canHeal <= 0.0f && currentHealth < maxHealth)
        {
            regenerate(ref currentHealth, maxHealth);
        }
        if (canRegenerate <= 0.0f && currentStamina < maxStamina )
        {
            regenerate(ref currentStamina, maxStamina);
        }
	}

    private void FixedUpdate()
    {
        if (chCont.isGrounded && Input.GetKey(KeyCode.LeftShift) && lastPosition != transform.position && currentStamina > 0)
        {
            lastPosition = transform.position;
            currentStamina -= 1;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            canRegenerate = 3.0f;
        }
        if (currentStamina > 0)
        {
            fpsC.CanRun = true;
        }
        else
        {
            fpsC.CanRun = false;
        }

    }

    private void regenerate(ref float currentStat, float maxStat)
    {
        currentStat += maxStat * 0.005f;
        currentStat = Mathf.Clamp(currentStat, 0, maxStat);
    }
}
