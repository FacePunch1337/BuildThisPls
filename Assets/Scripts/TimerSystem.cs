using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Photon.Pun;

public class TimerSystem : MonoBehaviourPun
{
    public TMP_Text _timer_txt;
    public TMP_Text kingName;
    public GameObject leaveButton;
    public GameObject winnerBoard;
    
    public float _second;
    public int _minute;
    public bool _countdown = false;
    private bool timerStop = false;


    void Start()
    {
        _timer_txt.text = _second.ToString();
        winnerBoard.SetActive(false);
        leaveButton.SetActive(false);
    } 
    
    public void Update()
    {
        if (_countdown)
        {
            if (_second < 1 && _minute < 1)
            {
                _timer_txt.text = Convert.ToString($"0{_minute}:00");



                //if (photonView.gameObject.GetComponent<PhotonView>().Owner.IsMasterClient && photonView.gameObject.GetComponent<PhotonView>().AmOwner)
                //{
                //Debug.Log("Im Here suka");

                if (!timerStop)
                {


                    Finish();

                    timerStop = true;
                    
                }
                    
                    
                //}
                //else return;
                    


                return;
            }
            else
            {
                
                _second -= Time.deltaTime;
                _timer_txt.text = Convert.ToString($"0{_minute}:0{Mathf.Round(_second)}");

                if (Convert.ToInt32(_second) == 0)
                {
                    _minute--;
                    _second = 59;
                }
                else if (Convert.ToInt32(_second) >= 10)
                    _timer_txt.text = Convert.ToString($"0{_minute}:{Mathf.Round(_second)}");
                else if (Convert.ToInt32(_second) < 10)
                    _timer_txt.text = Convert.ToString($"0{_minute}:0{Mathf.Round(_second)}");
            }
        }
        else
        {
            _second += Time.deltaTime;
            _timer_txt.text = Convert.ToString($"0{_minute}:0{Mathf.Round(_second)}");
            
            if (_second > 59)
            {
                _minute++;
                _second = 1;
            }
            else if (Convert.ToInt32(_second) >= 10)
                _timer_txt.text = Convert.ToString($"0{_minute}:{Mathf.Round(_second)}");
            else if (Convert.ToInt32(_second) < 10)
                _timer_txt.text = Convert.ToString($"0{_minute}:0{Mathf.Round(_second)}");
        }
    }

    public void Finish()
    {
            
            GameObject.Find("KingTrigger").TryGetComponent(out KingTriggerScript kingTrigger);
            if(kingTrigger.kingName.text != string.Empty)
            {
                kingName.text = $"{kingTrigger.kingName.text}" + " " + "Winner!";
                winnerBoard.SetActive(true);
                
                    leaveButton.SetActive(true);
                    Cursor.visible = true;
                
                
            }
            else if (kingTrigger.kingName.text == string.Empty)
            {
                kingName.text = $"{kingTrigger.lastKingName.text}" + " " + "Winner!";
                winnerBoard.SetActive(true);
                
                    leaveButton.SetActive(true);
                    Cursor.visible = true;
                
               
            }
           
            
            return;


    }
}
