using UnityEngine;
using System.Collections;

public class Initialize : MonoBehaviour 
{
    private Rect windowRect = new Rect(100, 300, 100, 100);
 
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnGUI()
    {
        GUI.Label(new Rect(100, 100, 100, 30), "Test");
        if (GUI.Button(new Rect(100, 200, 100, 30), "Button"))
        {
            Debug.Log("Button Clicked");
        }

        windowRect = GUI.Window(0, windowRect, setWindow, "A Window");
    }

    void setWindow(int windowID)
    {
        GUI.DragWindow();
    }
}
