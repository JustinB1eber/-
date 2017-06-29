using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;

public class RoleController : MonoBehaviour 
{
    private Animator _Animator = null;

    public static Hashtable UnityKinectJointMapping;

    public static Hashtable JointAdjacency;

    private static Vector3 LeftBottomPosition;
    private static Vector3 RightTopPosition;
    private static CameraSpacePoint? RolePosition;

    private Vector3 OriginalPosition;
    private Vector3 OriginalRotation;

   // private Collider LastCollider;

    private string Warning;

    private int Health = 100;

    private UdpClient client;

    private Rect rectWarning;
    private Rect rectGameOver;
    private Rect rectRestart;
    
	// Use this for initialization
	void Start () 
    {
        //if (JointAdjacency == null)
        //{
        //    JointAdjacency = new Hashtable();

        //    JointAdjacency.Add(JointType.ElbowLeft, JointType.WristLeft);
        //    JointAdjacency.Add(JointType.ShoulderLeft, JointType.ElbowLeft);
        //    JointAdjacency.Add(JointType.SpineShoulder, JointType.ShoulderLeft);

        //    JointAdjacency.Add(JointType.ElbowRight, JointType.WristRight);
        //    JointAdjacency.Add(JointType.ShoulderRight, JointType.ElbowRight);
        //    JointAdjacency.Add(JointType.SpineShoulder, JointType.ShoulderRight);

        //    JointAdjacency.Add(JointType.AnkleLeft, JointType.FootLeft);
        //    JointAdjacency.Add(JointType.KneeLeft, JointType.AnkleLeft);
        //    JointAdjacency.Add(JointType.HipLeft, JointType.KneeLeft);
        //    JointAdjacency.Add(JointType.SpineBase, JointType.HipLeft);

        //    JointAdjacency.Add(JointType.AnkleRight, JointType.FootRight);
        //    JointAdjacency.Add(JointType.KneeRight, JointType.AnkleRight);
        //    JointAdjacency.Add(JointType.HipRight, JointType.KneeRight);
        //    JointAdjacency.Add(JointType.SpineBase, JointType.HipRight);

        //    JointAdjacency.Add(JointType.SpineShoulder, JointType.Neck);
        //    JointAdjacency.Add(JointType.SpineMid, JointType.SpineShoulder);
        //    JointAdjacency.Add(JointType.SpineBase, JointType.SpineMid);

        //    //JointAdjacency.Add(JointType.WristLeft, JointType.ElbowLeft);
        //    //JointAdjacency.Add(JointType.ElbowLeft, JointType.ShoulderLeft);
        //    //JointAdjacency.Add(JointType.ShoulderLeft, JointType.SpineShoulder);

        //    //JointAdjacency.Add(JointType.WristRight, JointType.ElbowRight);
        //    //JointAdjacency.Add(JointType.ElbowRight, JointType.ShoulderRight);
        //    //JointAdjacency.Add(JointType.ShoulderRight, JointType.SpineShoulder);

        //    //JointAdjacency.Add(JointType.FootLeft, JointType.AnkleLeft);
        //    //JointAdjacency.Add(JointType.AnkleLeft, JointType.KneeLeft);
        //    //JointAdjacency.Add(JointType.KneeLeft, JointType.HipLeft);
        //    //JointAdjacency.Add(JointType.HipLeft, JointType.SpineBase);

        //    //JointAdjacency.Add(JointType.FootRight, JointType.AnkleRight);
        //    //JointAdjacency.Add(JointType.AnkleRight, JointType.KneeRight);
        //    //JointAdjacency.Add(JointType.KneeRight, JointType.HipRight);
        //    //JointAdjacency.Add(JointType.HipRight, JointType.SpineBase);

        //    //JointAdjacency.Add(JointType.Neck, JointType.SpineShoulder);
        //    //JointAdjacency.Add(JointType.SpineShoulder, JointType.SpineMid);
        //    //JointAdjacency.Add(JointType.SpineMid, JointType.SpineBase);
        //}

        //if (UnityKinectJointMapping == null)
        //{
        //    UnityKinectJointMapping = new Hashtable();

        //    UnityKinectJointMapping.Add(JointType.Neck, HumanBodyBones.Head);
        //    UnityKinectJointMapping.Add(JointType.SpineShoulder, HumanBodyBones.Neck);
        //    UnityKinectJointMapping.Add(JointType.SpineMid, HumanBodyBones.Chest);
        //    UnityKinectJointMapping.Add(JointType.SpineBase, HumanBodyBones.Hips);

        //    UnityKinectJointMapping.Add(JointType.ShoulderLeft, HumanBodyBones.LeftUpperArm);
        //    UnityKinectJointMapping.Add(JointType.ElbowLeft, HumanBodyBones.LeftLowerArm);
        //    UnityKinectJointMapping.Add(JointType.WristLeft, HumanBodyBones.LeftHand);


        //    UnityKinectJointMapping.Add(JointType.ShoulderRight, HumanBodyBones.RightUpperArm);
        //    UnityKinectJointMapping.Add(JointType.ElbowRight, HumanBodyBones.RightLowerArm);
        //    UnityKinectJointMapping.Add(JointType.WristRight, HumanBodyBones.RightHand);

        //    UnityKinectJointMapping.Add(JointType.HipLeft, HumanBodyBones.LeftUpperLeg);
        //    UnityKinectJointMapping.Add(JointType.KneeLeft, HumanBodyBones.LeftLowerLeg);
        //    UnityKinectJointMapping.Add(JointType.AnkleLeft, HumanBodyBones.LeftFoot);
        //    UnityKinectJointMapping.Add(JointType.FootLeft, HumanBodyBones.LeftToes);

        //    UnityKinectJointMapping.Add(JointType.HipRight, HumanBodyBones.RightUpperLeg);
        //    UnityKinectJointMapping.Add(JointType.KneeRight, HumanBodyBones.RightLowerLeg);
        //    UnityKinectJointMapping.Add(JointType.AnkleRight, HumanBodyBones.RightFoot);
        //    UnityKinectJointMapping.Add(JointType.FootRight, HumanBodyBones.RightToes);
        //}

        _Animator = GetComponent<Animator>();

        OriginalPosition = new Vector3(_Animator.transform.position.x, _Animator.transform.position.y, _Animator.transform.position.z);
        OriginalRotation = new Vector3(_Animator.transform.eulerAngles.x, _Animator.transform.eulerAngles.y, _Animator.transform.eulerAngles.z);

        if (LeftBottomPosition == null)
            LeftBottomPosition = new Vector3(185, 0, 270);
        if (RightTopPosition == null)
            RightTopPosition = new Vector3(245, 0, 330);
        rectWarning = new Rect(5, 5, 1000, 50);
        rectGameOver = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 25, 400, 50);
        rectRestart = new Rect(10, Screen.height - 50, 100, 35);
        Health = 100;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Body body = KinectSensorManager.GetBody();

        if (RoleControllerNet.IsKinectPause)
        {
            RolePosition = null;
        }

        if (body != null && !RoleControllerNet.IsKinectPause)
        {
            CameraSpacePoint mCurrentPosition = body.Joints[JointType.SpineBase].Position;

            if (RolePosition != null)
            {
                CameraSpacePoint point = (CameraSpacePoint)RolePosition;
                _Animator.transform.Translate(
                    new Vector3((mCurrentPosition.X - point.X) * 8, 0, (point.Z - mCurrentPosition.Z) * 8),
                    Space.Self
                    );
            }

            RolePosition = mCurrentPosition;
        }

        Vector3 rotation = _Animator.transform.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;

        if (RoleControllerNet.IsRightTurning)
        {
            rotation.y += 1;
        }
        else if (RoleControllerNet.IsLeftTurning)
        {
            rotation.y -= 1;
        }

        _Animator.transform.eulerAngles = rotation;
	}

    Quaternion GetOrientationVector(Transform current, Transform child)
    {
        return new Quaternion(
                    child.position.x - current.position.x,
                    child.position.y - current.position.y,
                    child.position.z - current.position.z,
                    1
                );
    }

    Transform CalcVector(Transform origin)
    {
        Vector3 vectPosition = origin.localPosition;
        Vector3 vectParentPosition = origin.parent.localPosition;
        double deltaX = vectParentPosition.x - vectPosition.x;
        double deltaY = vectParentPosition.y - vectPosition.y;
        double deltaZ = vectParentPosition.z - vectPosition.z;
        double deltaLength = deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ;

        return origin;
    }

    public void OnCollisionStay(Collision collisionInfo)
    {
        //Debug.Log("Collider stay");
    }

    public void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collider Exit");
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter");

        switch (collision.gameObject.tag)
        { 
            case "Vehicle":
                Warning = "Hitted by a Vehicle, HP - 100";
                Health -= 100;
                break;
            case "Dangerous":
                Warning = "Walking in the road, HP - 10";
                Health -= 10;
                break;
            case "StreetRoadHorizontal":
                if (!LightControl.state)
                { 
                    Warning = "Jumping the red light, HP - 20";
                    Health -= 20;
                }
                
                break;
            case "StreetRoadVerticle":
                if (LightControl.state)
                {
                    Warning = "Jumping the red light, HP - 20";
                    Health -= 20;
                }
                
                break;
        }
    }

    public void OnGUI()
    {
        GUI.skin.label.fontSize = 35;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
       // GUI.Label(rectWarning, String.Format("HP: {0}\t\t{1}", Health >= 0 ? Health : 0, Warning));

        if (Health <= 0)
        {
            GUI.skin.label.fontSize = 28;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.Label(rectGameOver, String.Format("Training Failed!"));
        }
        
        if (GUI.Button(rectRestart, "Restart"))
            {
                Health = 100;
                Warning = "";

                _Animator.transform.position = new Vector3(OriginalPosition.x, OriginalPosition.y, OriginalPosition.z);
                _Animator.transform.eulerAngles = new Vector3(OriginalRotation.x, OriginalRotation.y, OriginalRotation.z);

                RolePosition = null;
            }
    }
}


