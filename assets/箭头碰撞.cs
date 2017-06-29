using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class 箭头碰撞 : MonoBehaviour {
    public GameObject Quad;
    float x1, z1, x2, z2;//定义两个点
    int j;//用来判断最后的角度应该是+还是减
    int k;//用来让arctan角都是正数
          // Use this for initialization
    void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
        int k = j = 1;
        x1 = 0.29f; z1 = -19f;//目标点的x，z的值
        x2 = transform.position.x;//物体当前的x值
        z2 = transform.position.z;//物 体当前的z值
        float a = x1 - x2;
        float b = z1 - z2;
        if (a * b < 0) k = -1;
        float c = a / b * k;
        if (x1 < x2) j = -1;
        double r2 = Math.Atan(c) / Math.PI * 180 * j;
        float r3 = (float)r2;//把转过的角度变成float
        float r1 = 17.9f + r3;//17.9是物体原来的y方向角度

        Quad.transform.rotation = Quaternion.Euler(-81f, r1, -107f);








    }
    void OnTriggerEnter(Collider other) {
        Quad.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }
    void OnTriggerExit(Collider other)
    {

        Quad.transform.localScale = new Vector3(2.5f, 0.5f, 1f);
    }






}
