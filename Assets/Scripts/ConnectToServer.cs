using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
//using UnityEditor.PackageManager.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{

    public Canvas loadingCanvas;
    public Canvas menuCanvas;
    public InputField inputFieldRoom;
    public InputField inputFieldNickname;
   


    void Start()
    {
        menuCanvas.enabled = false;
        
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        loadingCanvas.enabled = false;
        menuCanvas.enabled = true;
    }

    public void SaveName()
    {
        PlayerPrefs.SetString("name", inputFieldNickname.text);
        PhotonNetwork.NickName = inputFieldNickname.text;
    }
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 5;
        PhotonNetwork.CreateRoom(inputFieldRoom.text);
    }

    public void JoinRoom()
    {
        
        PhotonNetwork.JoinRoom(inputFieldRoom.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("TestRoom");
    }

    public void Exit()
    {

        Application.Quit();
    }

}
