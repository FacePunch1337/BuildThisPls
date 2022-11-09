using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Photon.Pun;

using Photon.Realtime;
using System.Reflection;
using ExitGames.Client.Photon;

public class CustomCar : MonoBehaviourPun
{
    //RiseEvent

    /*private const byte COLOR_CHANGE_EVENT = 0;
   
    void Start()
    {
      
    }

    private void Update()
    {
        if (photonView.Owner.IsMasterClient && Input.GetKey(KeyCode.Q))
        {
            
            ChangeColor();
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventRecived;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventRecived;
    }

    private void NetworkingClient_EventRecived(EventData obj)
    {
        if(obj.Code == COLOR_CHANGE_EVENT)
        {
            object[] datas = (object[])obj.CustomData;
            float r = (float)datas[0];
            float g = (float)datas[1];
            float b = (float)datas[2];

            foreach (Material item in GameObject.Find("Car(Clone)").GetComponent<MeshRenderer>().materials)
            {
                item.color = new Color(r, g, b, 1f);
            }
        }
    }

    private void ChangeColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        foreach (Material item in GameObject.Find("Car(Clone)").GetComponent<MeshRenderer>().materials)
        {
            item.color = new Color(r, g, b, 1f);
        }

        object[] datas = new object[] {r, g, b };

        PhotonNetwork.RaiseEvent(COLOR_CHANGE_EVENT, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
       
    }*/



    //RPC

  //  public Trigger trigger;
    public void SendCustomCarDate()
    {
         photonView.RPC("ChangeColor", RpcTarget.AllBuffered);
        

    }

    [PunRPC]
    public void ChangeColor()
    {
        var carMaterialList = GameObject.Find("Car(Clone)").GetComponent<MeshRenderer>().materials;
        foreach (Material item in carMaterialList)
        {
            item.color = Color.green;
        }
       

    }
    
}
