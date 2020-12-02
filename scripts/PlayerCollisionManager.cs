using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
    private PlayerController _playerController;
    public StateManager stateManager;
    
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    { 
        if (other.gameObject.tag.Equals("Enemy"))
        {
            stateManager.SwitchState(StateManager.PlayerState.Dead);
        }
    }
}
