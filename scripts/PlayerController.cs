using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isDragging;

    private LineRenderer _lineRenderer;
    private Rigidbody2D _rigidbody;
    private Rigidbody2D _dangerIndicatorRigidbody;
    private PlayerScoreManager _playerScoreManager;
    [SerializeField] private TimescaleManager _timescaleManager;
    [SerializeField] private CameraZoom _cameraZoom;
    [SerializeField] private PostProcessingManager _postProcessingManager;
    [SerializeField] private SpriteRenderer _vignette;
    [SerializeField] private StateManager _stateManager;
    [SerializeField] private DangerIndicator _dangerIndicator;

    private Vector2 _positionToMoveTowards;
    private CameraShakeInstance _chargingShake;

    private int _vignetteId;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerScoreManager = GetComponent<PlayerScoreManager>();
        _dangerIndicatorRigidbody = GameObject.Find("DangerIndicator").GetComponent<Rigidbody2D>();
    }
    
    private void OnMouseDown()
    {
        if (isDragging || _stateManager.currentPlayerState == StateManager.PlayerState.Dead)
        {
            _lineRenderer.enabled = false;
            return;
        }
        
        _dangerIndicator.ToggleCollider(true);
        _timescaleManager.SlowdownWhenCharging();
        
        isDragging = true;
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, transform.position);
        
        _postProcessingManager.DisableMotionBlur(false);

        _chargingShake = CameraShaker.Instance.StartShake(0.1f, 0.2f, 0f);
        _vignetteId = LeanTween.alpha(_vignette.gameObject, 1f, 0.5f).id;
        
        _cameraZoom.ZoomIn();
    }

    private void OnMouseUp()
    {
        if (_stateManager.currentPlayerState == StateManager.PlayerState.Dead || isDragging == false)
        {
            _lineRenderer.enabled = false;
            return;
        }
        
        _dangerIndicator.HideDangerIndicator();
        _dangerIndicator.ToggleCollider(false);
        isDragging = false;
        _timescaleManager.ResetTimescale();
        _postProcessingManager.EnableMotionBlur(true);
        
        _chargingShake.StartFadeOut(0f);

        _positionToMoveTowards = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        //TODO switch to LineCast
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position, Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position));

        int enemiesHit = 0;
        
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag.Equals("Enemy"))
            {
                enemiesHit++;
                hit.collider.gameObject.GetComponent<Enemy>().TriggerDeath();
            }
        }
        
        _playerScoreManager.CalculateKills(enemiesHit);
        _lineRenderer.enabled = false;
        
        LeanTween.cancel(_cameraZoom.tweenId);
        LeanTween.cancel(_vignetteId);
        _vignette.color = new Color(1f, 1f, 1f, 0f);

        _cameraZoom.ResetZoom();
        
        _rigidbody.position = _positionToMoveTowards;
    }

    void Update()
    {
        if (isDragging)
        {
            _lineRenderer.SetPosition(1, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f));
            _dangerIndicator.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);
        }
    }

    void FixedUpdate()
    {
        if (isDragging && _stateManager.currentPlayerState != StateManager.PlayerState.Dead)
        {
            Vector2 directionToLook = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - _rigidbody.position;
            float angleToLook = Mathf.Atan2(directionToLook.y, directionToLook.x) * Mathf.Rad2Deg - 90f;
            _rigidbody.rotation = angleToLook;
            _dangerIndicatorRigidbody.rotation = angleToLook;
        }
    }

    public void CancelShake()
    {
        try
        {
            _chargingShake.StartFadeOut(0f);
        }
        catch (NullReferenceException error)
        {
            Debug.Log(error);
            Debug.Log("_chargingShake was null.");
        }
        
    }

    public void ResetVignette()
    {
        LeanTween.cancel(_vignetteId);
        _vignette.color = new Color(1f, 1f, 1f, 0f);
    }
}
