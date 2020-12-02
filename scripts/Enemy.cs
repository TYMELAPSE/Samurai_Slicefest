using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Transform _playerPosition;
    private Collider2D _collider2D;
    [SerializeField] private float movementSpeed;
    [SerializeField] private SpawnManager _spawnManager;

    public bool isActive;
    public bool isInDangerZone;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerPosition = GameObject.Find("Player").GetComponent<Transform>();
        _collider2D = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            Vector3 directionTowardsPlayer =
                (_playerPosition.transform.position - _rigidbody.transform.position).normalized;
            _rigidbody.MovePosition(_rigidbody.transform.position + directionTowardsPlayer * movementSpeed * Time.fixedDeltaTime);

            Vector2 directionToLook = (Vector2) _playerPosition.transform.position - _rigidbody.position;
            float angleToLook = Mathf.Atan2(directionToLook.y, directionToLook.x) * Mathf.Rad2Deg - 90f;
            _rigidbody.rotation = angleToLook;
        }
    }

    public void TriggerDeath()
    {
        if (!GlobalGameSettings.hideBlood)
        {
            _spawnManager.SpawnBlood(this.transform, Quaternion.identity);   
        }
        
        _spawnManager.AddEnemyToPool(this);
    }

    public void ToggleCollider(bool active)
    {
        _collider2D.enabled = active;
    }
}
