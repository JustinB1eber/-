using UnityEngine;
using System.Collections;

public class LightControl : MonoBehaviour 
{

    public static bool state
    {
        get;
        private set;
    }
    private Light[] StreetLights;

	// Use this for initialization
	void Start () 
    {
        StreetLights = FindObjectsOfType<Light>();
        InvokeRepeating("ChangeLight", 0, 30);

        state = true;
	}

    void ChangeLight()
    {
        Debug.Log("ChangeLight");

        foreach (var light in StreetLights)
        {
            switch (light.tag)
            {
                case "VerticalGreenLight":
                    light.enabled = state;
                    break;
                case "VerticalRedLight":
                    light.enabled = !state;
                    break;
                case "HorizontalGreenLight":
                    light.enabled = !state;
                    break;
                case "HorizontalRedLight":
                    light.enabled = state;
                    break;
            }
        }

       //GameObject.FindGameObjectWithTag("Vehicle").BroadcastMessage("LightChanged", state);

        state = !state;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
