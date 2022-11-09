using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Cinemachine;
using System.IO;
using UnityEngine.Experimental.Rendering.Universal;
using Photon.Realtime;
using JetBrains.Annotations;
using TMPro;
using ExitGames.Client.Photon.StructWrapping;
using System.Drawing;

[RequireComponent(typeof(Rigidbody))]

public class CarController : MonoBehaviour
{
   
    public List<AxleInfo> axleInfos;
    public Transform carTransform;
    public new Rigidbody rigidbody;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public bool ready;
    private PhotonView view;
    private TextMeshPro _nickname;
    public TextMeshPro nickname { get { return _nickname; } set { _nickname = value; } }

    





    private void Start()
    {
        view = GetComponent<PhotonView>();
        nickname = GetComponentInChildren<TextMeshPro>();
       
        nickname.text = view.Owner.NickName;
        ready = false;

       
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }







    /*public void TireEffect()
    {
        RearWheelDrive rearWheelDrive;

        foreach (var wheel in wheels)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rearWheelDrive.GetComponentInChildren<TrailRenderer>().emitting = true;

            }
            else rearWheelDrive.GetComponentInChildren<TrailRenderer>().emitting = false;
        }
    }*/
    public void FixedUpdate()
    {
        if (view.IsMine)
        {
            float motor = maxMotorTorque * Input.GetAxis("Vertical");
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal");


            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {

                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;

                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;

                }

                if (Input.GetKey(KeyCode.Space))
                {
                    if (axleInfo.braking)
                    {
                        axleInfo.rightWheel.GetComponent<WheelCollider>().brakeTorque = 1000;
                        axleInfo.leftWheel.GetComponent<WheelCollider>().brakeTorque = 1000;

                        
                        












                    }


                }
                else
                {
                    axleInfo.rightWheel.GetComponent<WheelCollider>().brakeTorque = 0;
                    axleInfo.leftWheel.GetComponent<WheelCollider>().brakeTorque = 0;
                  
                }

                if (Input.GetKey(KeyCode.R))
                {
                    Respawn();
                }
                ApplyLocalPositionToVisuals(axleInfo.leftWheel);
                ApplyLocalPositionToVisuals(axleInfo.rightWheel);


                
            }
        }
    }


    public void Respawn()
    {
        
            GameObject.Find("GameManager").TryGetComponent(out SpawnPlayers spawn);
            var randomSpawnPos = spawn.spawnPoints[Random.Range(0, spawn.spawnPoints.Length)].transform.position; 
            transform.position = new Vector3(randomSpawnPos.x, randomSpawnPos.y, randomSpawnPos.z);
            transform.rotation = new Quaternion (0,0,0,0);
            rigidbody.velocity = new Vector3(0, 0, 0);
            
        
    }




    [System.Serializable]
    public class AxleInfo
    {

        public WheelCollider leftWheel;
        public GameObject leftWheelVisuals;
        private bool leftGrounded = false;
        private float travelL = 0f;
        private float leftAckermanCorrectionAngle = 0;
        public WheelCollider rightWheel;
        public GameObject rightWheelVisuals;
        //public GameObject wheelEffectsObj;
        
        private bool rightGrounded = false;
        private PhotonView view;
        private float travelR = 0f;
        private float rightAckermanCorrectionAngle = 0;
        public bool motor;
        public bool steering;
        public bool braking;


        public float Antiroll = 10000;
        private float AntrollForce = 0;
        
        public float ackermanSteering = 1f;
        public void ApplyLocalPositionToVisuals()
        {
           
                //left wheel
                if (leftWheelVisuals == null)
                {
                    return;
                }
                Vector3 position;
                Quaternion rotation;
                leftWheel.GetWorldPose(out position, out rotation);

                leftWheelVisuals.transform.position = position;
                leftWheelVisuals.transform.rotation = rotation;

                //right wheel
                if (rightWheelVisuals == null)
                {
                    return;
                }
                rightWheel.GetWorldPose(out position, out rotation);
            
                rightWheelVisuals.transform.position = position;
                rightWheelVisuals.transform.rotation = rotation;
            
            }
        public void CalculateAndApplyAntiRollForce(Rigidbody theBody)
        {
            WheelHit hit;

            leftGrounded = leftWheel.GetGroundHit(out hit);
            if (leftGrounded)
                travelL = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;
            else
                travelL = 1f;

            rightGrounded = rightWheel.GetGroundHit(out hit);
            if (rightGrounded)
                travelR = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;
            else
                travelR = 1f;

            AntrollForce = (travelL - travelR) * Antiroll;

            if (leftGrounded)
                theBody.AddForceAtPosition(leftWheel.transform.up * -AntrollForce, leftWheel.transform.position);
            if (rightGrounded)
                theBody.AddForceAtPosition(rightWheel.transform.up * AntrollForce, rightWheel.transform.position);

        }
        public void CalculateAndApplySteering(float input, float maxSteerAngle, List<AxleInfo> allAxles)
        {
           
                //first find farest axle, we got to apply default values
                AxleInfo farestAxle = allAxles[0];
                //calculate start point for checking
                float farestAxleDistantion = ((allAxles[0].leftWheel.transform.localPosition - allAxles[0].rightWheel.transform.localPosition) / 2f).z;
                for (int a = 0; a < allAxles.Count; a++)
                {
                    float theDistance = ((allAxles[a].leftWheel.transform.localPosition - allAxles[a].rightWheel.transform.localPosition) / 2f).z;
                    // if we found axle that farer - save it
                    if (theDistance < farestAxleDistantion)
                    {
                        farestAxleDistantion = theDistance;
                        farestAxle = allAxles[a];
                    }
                }
                float wheelBaseWidth = (Mathf.Abs(leftWheel.transform.localPosition.x) + Mathf.Abs(rightWheel.transform.localPosition.x)) / 2;
                float wheelBaseLength = Mathf.Abs(((farestAxle.leftWheel.transform.localPosition + farestAxle.rightWheel.transform.localPosition) / 2f).z) +
                    Mathf.Abs(((leftWheel.transform.localPosition + rightWheel.transform.localPosition) / 2f).z);

                float angle = maxSteerAngle * input;
                //ackerman implementation
                float turnRadius = Mathf.Abs(wheelBaseLength * Mathf.Tan(Mathf.Deg2Rad * (90 - Mathf.Abs(angle))));
                if (input != 0)
                {   
                    //right wheel
                    if (angle > 0)
                    {//turn right
                        
                        rightAckermanCorrectionAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (turnRadius - wheelBaseWidth / 2f));
                        rightAckermanCorrectionAngle = (rightAckermanCorrectionAngle - Mathf.Abs(angle)) * ackermanSteering + (Mathf.Abs(angle));
                        rightAckermanCorrectionAngle = Mathf.Sign(angle) * rightAckermanCorrectionAngle;
                    }
                    else
                    {//turn left

                        rightAckermanCorrectionAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (turnRadius + wheelBaseWidth / 2f));
                        rightAckermanCorrectionAngle = (rightAckermanCorrectionAngle - Mathf.Abs(angle)) * ackermanSteering + (Mathf.Abs(angle));
                        rightAckermanCorrectionAngle = Mathf.Sign(angle) * rightAckermanCorrectionAngle;
                    }
                    //left wheel
                    if (angle > 0)
                    {//turn right
                        leftAckermanCorrectionAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (turnRadius + wheelBaseWidth / 2f));
                        leftAckermanCorrectionAngle = (leftAckermanCorrectionAngle - Mathf.Abs(angle)) * ackermanSteering + (Mathf.Abs(angle));
                        leftAckermanCorrectionAngle = Mathf.Sign(angle) * leftAckermanCorrectionAngle;
                    }
                    else
                    {//turn left
                        leftAckermanCorrectionAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (turnRadius - wheelBaseWidth / 2f));
                        leftAckermanCorrectionAngle = (leftAckermanCorrectionAngle - Mathf.Abs(angle)) * ackermanSteering + (Mathf.Abs(angle));
                        leftAckermanCorrectionAngle = Mathf.Sign(angle) * leftAckermanCorrectionAngle;
                    }
                }
                else
                {
                    rightAckermanCorrectionAngle = 0f;
                    leftAckermanCorrectionAngle = 0f;
                }
                leftWheel.steerAngle = leftAckermanCorrectionAngle;
                rightWheel.steerAngle = rightAckermanCorrectionAngle;
                Debug.Log(leftAckermanCorrectionAngle + " " + rightAckermanCorrectionAngle);
            }
        }
           



    



}