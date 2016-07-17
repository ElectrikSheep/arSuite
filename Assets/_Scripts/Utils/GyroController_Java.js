﻿#pragma strict
 
static var gyroBool : boolean;
private var gyro : Gyroscope;
private var quatMult : Quaternion;
private var quatMap : Quaternion;
private var camGrandparent : GameObject;
function Awake() {
    // find the current parent of the camera's transform
    var currentParent : Transform = transform.parent;
    // instantiate a new transform
    var camParent : GameObject = new GameObject("camParent");
    // match the transform to the camera position
    camParent.transform.position = transform.position;
    // make the new transform the parent of the camera transform
    transform.parent = camParent.transform;
 
    // Also creates a grandparent (camGrandparent) which can be rotated with localEulerAngles.y
    // This node allows an arbitrary heading to be added to the gyroscope reading
    // so that the virtual camera can be facing any direction in the scene, no matter what the phone's heading
    camGrandparent = new GameObject("camGrandParent");
    // match the transform to the camera position
    camGrandparent.transform.position = transform.position;
    // make the new transform the parent of the camera transform
    camParent.transform.parent = camGrandparent.transform;
    // make the original parent the grandparent of the camera transform
    camGrandparent.transform.parent = currentParent;
 
    // check whether device supports gyroscope
    gyroBool = SystemInfo.supportsGyroscope;
 
    if (gyroBool) {
        gyro = Input.gyro;
        gyro.enabled = true;
     
        #if UNITY_IPHONE
            camParent.transform.eulerAngles = Vector3(90, 90, 0);
            quatMult = Quaternion(0, 0, 1, 0);
        #endif
     
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    } else {
        print("NO GYRO");
    }
}
 
function Start() {
    // First, check if user has location service enabled
    if (!Input.location.isEnabledByUser)
        return;
 
    // Start service before querying location
    Input.location.Start();
    Input.compass.enabled = true;
 
    // Wait until service initializes
    var maxWait : int = 20;
    while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
        yield WaitForSeconds(1);
        maxWait--;
    }
 
    if (maxWait < 1) {
        // Service didn't initialize in 20 seconds
        print("Timed out");
        return;
    }
 
    if (Input.location.status == LocationServiceStatus.Failed) {
        // Connection has failed
        print("Unable to determine device location");
        return;
    }
}
function Update() {
    if (gyroBool) {
        #if UNITY_IPHONE
            quatMap = gyro.attitude;
        #endif
     
        camGrandparent.transform.localEulerAngles.y = Input.compass.trueHeading;
        transform.localRotation = quatMap * quatMult;
    }
}