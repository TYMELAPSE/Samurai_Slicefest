using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public const float DEFAULT_ORTHOGRAPHIC_SIZE = 17.46f;
    public int tweenId;
    public void ZoomIn(){
        tweenId = LeanTween.value(Camera.main.gameObject, Camera.main.orthographicSize, 10f, 5f).setOnUpdate((float flt) =>   {
            Camera.main.orthographicSize = flt;
        }).id;
    }

    public void ResetZoom()
    {
        Camera.main.orthographicSize = DEFAULT_ORTHOGRAPHIC_SIZE;
    }
}
