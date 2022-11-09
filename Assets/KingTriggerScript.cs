using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KingTriggerScript : MonoBehaviourPun
{
    public TMP_Text kingName;
    private TMP_Text _lastKingName;
    public TMP_Text lastKingName { get { return _lastKingName; } set { _lastKingName = value; } }

    
    private void OnTriggerEnter(Collider other)
    {
       /* if (other.gameObject.CompareTag("Car"))
        {
            other.gameObject.TryGetComponent(out CarController car);
            kingName.text = car.nickname.text;
            

        }
        else return;*/

    }
    private void OnTriggerStay(Collider other)
    {


        if (other.gameObject.CompareTag("Car"))
        {
            other.gameObject.TryGetComponent(out CarController car);
            kingName.text = car.nickname.text;
            _lastKingName.text = car.nickname.text;


        }
        else return;


    }



    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            kingName.text = string.Empty;


        }
        else return;

        //  textReadyCount.SetActive(false);
        /*if (!other.gameObject.TryGetComponent(out CustomCar customCar)) return;
        else if (base.photonView.IsMine) customCar.SendCustomCarDate();*/


        // Debug.Log("leave trigger");


    }


}
