using UnityEngine;
using System.Collections;

public class collisiondetection : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other) {
        print("进入");
    }
    void OnTriggerStay() {
       print("保持");
    }
    //void OnTriggerExit() {
    //    print("离开");
    //}
}
