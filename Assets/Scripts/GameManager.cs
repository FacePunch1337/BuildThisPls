using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

using Smooth;
using System;
using TMPro;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class GameManager : MonoBehaviourPun
{

    private new PhotonView photonView;

    public GameObject menuPanel;
    

    public bool first_press = true;
    public bool menu = false;
    


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuPanel.SetActive(false);
        photonView = GetComponent<PhotonView>();
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    void Update()
    {
        // Cursor.visible = false;
        // CountOfPlayer();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menu)
            {
                menuPanel.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                menu = true;
            }
            else
            {
               
                menuPanel.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                menu = false;
            }
           
            
            
        }
        


        /*if (Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene(1);
            
        }*/
    }


    public void StartMode()
    {
       
        GameObject.Find("Trigger").TryGetComponent(out Trigger trigger);
        if (trigger.readyCount == PhotonNetwork.PlayerList.Length)
        {
           
            PhotonNetwork.LoadLevel(2);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            
        }


    }


    public void SendEndGameMode()
    {
        photonView.RPC("EndGameMode", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void EndGameMode()
    {

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


    }

    public void QuitButton()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeButton()
    {
        menuPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void InputButton()
    {
        menuPanel.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }



}
