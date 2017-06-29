using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class KinectSensorManager :MonoBehaviour
{
    private static KinectSensor _Sensor;
    private static BodyFrameReader _Reader;

    public void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        if (_Sensor != null && _Sensor.IsOpen)
        {
            _Sensor.Close();
            _Sensor = null;
        }
    }

    void Start()
    {
        if (_Sensor != null)
        {
            _Sensor.Close();
            _Sensor = null;
        }

        _Sensor = KinectSensor.GetDefault();

        if (_Sensor != null)
        {
            _Reader = _Sensor.BodyFrameSource.OpenReader();
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }
    }

    public static Body GetBody()
    {
        if (_Reader == null)
            return null;

        BodyFrame frame = _Reader.AcquireLatestFrame();
        if (frame == null) return null;
        Body[] bodies = new Body[frame.BodyCount];
        frame.GetAndRefreshBodyData(bodies);
        frame.Dispose();
        frame = null;

        if (bodies == null) return null;

        foreach (var body in bodies)
        {
            foreach (var joint in body.Joints)
            {
                if (joint.Value.TrackingState == TrackingState.Tracked)
                {
                    return body;
                }
            }
        }
        return null;
    }
}
