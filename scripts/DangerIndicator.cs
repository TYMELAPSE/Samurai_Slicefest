using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerIndicator : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Collider2D _collider;

    private int fadeInId;
    private int fadeOutId;

    [SerializeField] private float _fadeTime;

    private string _name;
    
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy") && other.gameObject.GetComponent<Enemy>().isInDangerZone)
        {
            LeanTween.cancel(fadeOutId);
            fadeInId = LeanTween.alpha(_sprite.gameObject, 1f, _fadeTime * Time.timeScale).id;
            _name = other.gameObject.name;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == _name)
        {
            LeanTween.cancel(fadeInId);
            HideDangerIndicator();
        }
    }

    public void HideDangerIndicator()
    {
        LeanTween.cancel(fadeInId);   
        fadeOutId = LeanTween.alpha(_sprite.gameObject, 0f, _fadeTime * Time.timeScale).id;
    }

    public void ToggleCollider(bool enabled)
    {
        _collider.enabled = enabled;
    }
}
