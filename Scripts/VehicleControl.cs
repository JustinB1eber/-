using UnityEngine;
using System.Collections;
using System;

public class VehicleControl : MonoBehaviour {

    private System.Random random;
    private GameObject[] Vehicles;
    private ArrayList lstIdle;
    private ArrayList lstRunning;

    private ArrayList StartPoints;
    private ArrayList[] Streets;
    private ArrayList ChangedToIdle;

    private float[] StopPosition;
    private Hashtable VehicleStatus;

    private bool LightState;

	// Use this for initialization
	void Start () {
        random = new System.Random();
        Vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
        ChangedToIdle = new ArrayList();
        StopPosition = new float[] { 281.5f, 281.5f, 236f, 236f, 318f, 318f, 199f, 199f };
        VehicleStatus = new Hashtable();
        foreach (var vehicle in Vehicles)
        {
            VehicleStatus.Add(vehicle, true);
        }

        StartPoints = new ArrayList();
        StartPoints.Add(new DictionaryEntry(new Vector3(6.3f+212.7f, 0.2f, 57.65f+30.3f), new Vector3(0f, 0f, 1f)));       //1
        StartPoints.Add(new DictionaryEntry(new Vector3(9.96f + 212.7f, 0.2f, 60.47f + 30.3f), new Vector3(0f, 0f, 1f)));      //2
        StartPoints.Add(new DictionaryEntry(new Vector3(208.99f + 212.7f, 0.2f, 271.69f + 30.3f), new Vector3(-1f, 0f, 0f)));
        StartPoints.Add(new DictionaryEntry(new Vector3(209.13f + 212.7f, 0.2f, 275.21f + 30.3f), new Vector3(-1f, 0f, 0f)));
        StartPoints.Add(new DictionaryEntry(new Vector3(2.27f + 212.7f, 0.2f, 473.79f + 30.3f), new Vector3(0f, 0f, -1f)));
        StartPoints.Add(new DictionaryEntry(new Vector3(-1.08f + 212.7f, 0.2f, 474.2f + 30.3f), new Vector3(0f, 0f, -1f)));
        StartPoints.Add(new DictionaryEntry(new Vector3(-158.5f + 212.7f, 0.2f, 267.74f + 30.3f), new Vector3(1f, 0f, 0f)));
        StartPoints.Add(new DictionaryEntry(new Vector3(-158.7f + 212.7f, 0.2f, 264.17f + 30.3f), new Vector3(1f, 0f, 0f)));

        Streets = new ArrayList[8];
        for (int i = 0; i < 8; i++)
            Streets[i] = new ArrayList();

        ArrayList lstTemp = new ArrayList();
        lstIdle = new ArrayList();
        foreach (var u in Vehicles)
        {
            lstTemp.Add(u);
        }

        while (lstTemp.Count > 0)
        {
            GameObject go = (GameObject)lstTemp[random.Next(lstTemp.Count - 1)];
            lstIdle.Add(go);
            lstTemp.Remove(go);
        }

        lstRunning = new ArrayList();
       // Invoke("GenerateVehicles", random.Next(1, 10));
        Invoke("GenerateVehicles", 1);
	}

    void GenerateVehicles()
    {
        //Generated.Add(new DictionaryEntry((GameObject)Instantiate(Vehicles[0], new Vector3(219f, 0.25f, 95.12f), new Quaternion(0,180,0,1)),- random.NextDouble() / 2 + 0.1));
        if (lstIdle.Count == 0)
        {
            Invoke("GenerateVehicles", 1);
            return;
        } 
        GameObject go = (GameObject)lstIdle[random.Next(lstIdle.Count - 1)];
        int numStreet = random.Next(1,8);
        go.transform.position = (Vector3)((DictionaryEntry)StartPoints[numStreet]).Key;

        Vector3 rotation = go.transform.eulerAngles;
        switch (numStreet)
        { 
            case 0:
            case 1:
                break;
            case 2:
            case 3:
                rotation.y += 270;
                break;
            case 4:
            case 5:
                rotation.y+=180;
                break;
            case 6:
            case 7:
                rotation.y+=90;
                break;
        }
        go.transform.eulerAngles = rotation;

        Streets[numStreet].Add(new DictionaryEntry(go, random.NextDouble() / 4.0 + 0.2));
        lstIdle.Remove(go);
        //Invoke("GenerateVehicles", random.Next(1, 10)); 
        Invoke("GenerateVehicles", 1);
    }
	// Update is called once per frame
	void Update () 
    {
        //foreach (DictionaryEntry u in lstRunning)
        //{
        //    GameObject vehicle = (GameObject)u.Key;
        //    double speed = (double)u.Value;


        //    vehicle.transform.Translate(0, 0, Convert.ToSingle(speed), vehicle.transform);
        //}

        for (int i = 0; i < 8; i++)
        {
            ChangedToIdle.Clear();
            foreach (DictionaryEntry u in Streets[i])
            {
                if (LightControl.state)
                {
                    if (i == 0 || i == 1 || i == 4 || i == 5)
                    {
                        continue;
                    }
                }
                else
                {
                    if (i == 2 || i == 3 || i == 6 || i == 7)
                        continue;
                }
                

                GameObject vehicle = (GameObject)u.Key;
                double speed = (double)u.Value;

                Vector3 position = vehicle.transform.position;
                position.x += Convert.ToSingle(speed * ((Vector3)((DictionaryEntry)StartPoints[i]).Value).x);
                position.y += Convert.ToSingle(speed * ((Vector3)((DictionaryEntry)StartPoints[i]).Value).y);
                position.z += Convert.ToSingle(speed * ((Vector3)((DictionaryEntry)StartPoints[i]).Value).z);
                vehicle.transform.position = position;
                Vector3 rotation = vehicle.transform.eulerAngles;
                switch (i)
                {
                    case 0:
                    case 1:
                        if (position.z > 600)
                        {
                            ChangedToIdle.Add(u);
                        }
                        break;
                    case 2:
                    case 3:
                        if (position.x < 0)
                        {
                            rotation.y -= 270;
                            ChangedToIdle.Add(u);
                        }
                        break;
                    case 4:
                    case 5:
                        if (position.z < 0)
                        {
                            rotation.y -= 180;
                            ChangedToIdle.Add(u);
                        }
                        break;
                    case 6:
                    case 7:
                        if (position.x > 500)
                        {
                            rotation.y -= 90;
                            ChangedToIdle.Add(u);
                        }
                        break;
                }
                vehicle.transform.eulerAngles=rotation;
            }

            foreach(DictionaryEntry u in ChangedToIdle)
            {
                GameObject vehicle = (GameObject)u.Key;
                Streets[i].Remove(u);
                lstIdle.Add(vehicle);
            }
            
        }
	}
}
