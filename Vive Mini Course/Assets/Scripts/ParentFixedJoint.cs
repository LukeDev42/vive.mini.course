using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentFixedJoint : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;
    public Rigidbody rigidBodyAttatchPoint;

    public Transform sphere;
    FixedJoint fixedJoint = null;

    // Use this for initialization
    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Debug.Log("You are holding 'PressUp' the Touchpad");
            sphere.transform.position = Vector3.zero;
            sphere.GetComponent<Rigidbody>().velocity = Vector3.zero;
            sphere.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        device = SteamVR_Controller.Input((int)trackedObj.index);
	}

    private void OnTriggerStay(Collider col)
    {
        Debug.Log("You have collided with " + col.name + " And activated OnTriggerStay");
        if(fixedJoint == null && device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            fixedJoint = col.gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = rigidBodyAttatchPoint;
        }
        else if (fixedJoint != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            GameObject go = fixedJoint.gameObject;
            Rigidbody rigidBody = go.GetComponent<Rigidbody>();
            Object.Destroy(fixedJoint);
            fixedJoint = null;
            tossObject(rigidBody);
        }
    }
    void tossObject(Rigidbody rigidbody)
    {
        Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
        if (origin != null)
        {
            rigidbody.velocity = origin.TransformVector(device.velocity);
            rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);
        }
        else
        {
            rigidbody.velocity = device.velocity;
            rigidbody.angularVelocity = device.angularVelocity;
        }
    }
}