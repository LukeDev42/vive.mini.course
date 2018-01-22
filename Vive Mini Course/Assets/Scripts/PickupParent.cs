using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PickupParent : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    public Transform sphere;
    // Use this for initialization
    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        device = SteamVR_Controller.Input((int)trackedObj.index);
        if(device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You are holding 'Touch' the Trigger");
        }
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You are holding 'TouchDown' the Trigger");
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You are holding 'TouchUp' the Trigger");
        }
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You are holding 'Press' the Trigger");
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You are holding 'PressDown' the Trigger");
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You are holding 'PressUp' the Trigger");
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Debug.Log("You are holding 'PressUp' the Touchpad");
            sphere.transform.position = Vector3.zero;
            sphere.GetComponent<Rigidbody>().velocity = Vector3.zero;
            sphere.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        Debug.Log("You have collided with " + col.name + " And activated OnTriggerStay");
        if(device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You have collided with " + col.name + " while holding down Touch");
            col.attachedRigidbody.isKinematic = true;
            col.gameObject.transform.SetParent(gameObject.transform);

        }
        if(device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You have released Touch while colliding wih " + col.name);
            col.gameObject.transform.SetParent(null);
            col.attachedRigidbody.isKinematic = false;

            tossObject(col.attachedRigidbody);

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
