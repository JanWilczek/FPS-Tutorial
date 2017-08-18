using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    public Texture GameLogo;
    public float buttonWidth = 200;
    public float buttonHeight = 40;
    public GUISkin skin;
    public GUIStyle disabledButton;

    private float buttonMargin = 10;

    private void OnGUI()
    {
        GUI.skin = skin;
        GUI.DrawTexture(new Rect(0, 0, 500, 200), GameLogo);

        GUI.BeginGroup(new Rect(300, 200, buttonWidth, (buttonHeight + buttonMargin) * 3));
            if (GUI.Button(new Rect(0, 0, buttonWidth, buttonHeight), "New Game"))
            {
               Application.LoadLevel("scene1");
            }
            if (GUI.Button(new Rect(0, 0 + buttonHeight + buttonMargin, buttonWidth, buttonHeight), "Options", disabledButton)) ;
            if (GUI.Button(new Rect(0, 0 + (buttonHeight + buttonMargin) * 2, buttonWidth, buttonHeight), "Exit"))
            {
                Application.Quit();
            }
        GUI.EndGroup();
    }

    // Use this for initialization
    void Start () {
        buttonWidth = (buttonWidth * Screen.width) / 1920;
        buttonHeight = (buttonHeight * Screen.height) / 1080;
        buttonMargin = (buttonMargin * Screen.height) / 1080;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
