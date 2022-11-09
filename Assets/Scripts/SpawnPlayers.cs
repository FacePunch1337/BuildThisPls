using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Smooth;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using System.Linq;
//using static UnityEditor.Progress;

public class SpawnPlayers : MonoBehaviour
{

    public GameObject player;
    public Transform[] spawnPoints;
    public Transform lookAtTarget;
    
    private PhotonView view;
    private SmoothSyncPUN2 smoothSync;

    private bool isFirstPlayer = true;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        
        /*var center = GameObject.Find("Center");
        if (center)
        {
            foreach (var item in spawnPoints)
            {
                item.LookAt(center.transform);
            }
           
        }*/
        SpawnPlayerOnPoint();


    }

    public void SpawnPlayerOnPoint()
    {
        if (isFirstPlayer)
        {
            isFirstPlayer = false;

           
            Transform randomTransform = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject go = PhotonNetwork.Instantiate(player.name, randomTransform.position, randomTransform.rotation);
            GameObject.Find("CM FreeLook1").GetComponent<FollowCamera>().Attach(go.transform);
           // GameObject.Find("CM vcam2").GetComponent<FreeCamera>().Attach(go.transform);
        

        }
    }

}
