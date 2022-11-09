using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Smooth;
using System;
using TMPro;

public class GameManager : MonoBehaviourPun
{


   
    private new PhotonView photonView;

    public GameObject menuPanel;
 
    public bool first_press = true;
    public bool menu = false;
 
   
    
    

    void Start()
    {
        Cursor.visible = false;
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
                menuPanel.SetActive(true);
                Cursor.visible = true;
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
           
            }
            
        
    }
    

    

    public void EndGameMode()
    {
       
        PhotonNetwork.LoadLevel(1);

    }

    public void QuitButton()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
        Cursor.visible = true;
    }

    public void ResumeButton()
    {
        menuPanel.SetActive(false);
        Cursor.visible = false;
    }

    public void InputButton()
    {
        menuPanel.SetActive(false);
        
        Cursor.visible = true;
    }



}
