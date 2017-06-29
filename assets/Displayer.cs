using UnityEngine;
using System.Collections;

public class Displayer : MonoBehaviour {
    private RenderTexture LeftTexture;
    private RenderTexture RightTexture;

    GUIStyle style = GUIStyle.none;

    private Rect rectLeftWindow = new Rect(10.0f, Screen.height / 2 - 120, 276f, 168f);
    private Rect rectRightWindow = new Rect(Screen.width - 330, Screen.height / 2 - 120, 276f, 168f);
    private Rect rectExit = new Rect(150, Screen.height - 50, 100, 35);
	// Use this for initialization
	void Start () 
    {
        Camera[] cameras= FindObjectsOfType<Camera>();
        foreach (var camera in cameras)
        {
            switch (camera.tag)
            { 
                case"LeftCamera":
                    LeftTexture = camera.targetTexture;
                    break;
                case "RightCamera":
                    RightTexture = camera.targetTexture;                         
                    break;
            }
        }

        GUIStyle style = new GUIStyle();
        style.normal.background =Texture2D.blackTexture;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnGUI()
    {
        rectLeftWindow = GUI.Window(0, rectLeftWindow, LeftScreenFunction, "左侧视野");
        rectRightWindow = GUI.Window(1, rectRightWindow, RightScreenFunction, "右侧视野");


        if (GUI.Button(rectExit, "Exit"))
        {
            Application.Quit();
        }
    }

    private void RightScreenFunction(int id)
    {
        GUI.DrawTexture(new Rect(10f, 30f, 256f, 128f), RightTexture, ScaleMode.ScaleAndCrop, false);
        GUI.DragWindow();
    }

    private void LeftScreenFunction(int id)
    {
        GUI.DrawTexture(new Rect(10f, 30f, 256f, 128f), LeftTexture, ScaleMode.ScaleAndCrop, false);
        GUI.DragWindow();
    }    
}
