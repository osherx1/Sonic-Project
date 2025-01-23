using System;
using UnityEngine;

public class CameraSizeAdjustment : MonoBehaviour
{
    private const float PixelsToWorldUnitFactor = 200f;

    [SerializeField] private Vector2 referenceResolution = new Vector2(1920, 1080);
    [SerializeField] private Camera mainCamera;
    private float _currentRatio;


    private float TargetRatio => referenceResolution.x / referenceResolution.y;


    private void Start()
    {
        _currentRatio = (float)Screen.width / Screen.height;
        FitToScreenSize();
    }

    private void Update()
    {
        var prevRatio = _currentRatio;
        var newRatio = (float)Screen.width / Screen.height;
        if (Math.Abs(prevRatio - newRatio) > 0.01f) 
            FitToScreenSize();

        _currentRatio = newRatio;
    }

    private void FitToScreenSize()
    {
        var scaledHeight = referenceResolution.x / _currentRatio;
        
        // Screen became wider than we want
        if (_currentRatio >= TargetRatio)
        {
            mainCamera.orthographicSize = referenceResolution.y / PixelsToWorldUnitFactor;
        }
        
        // Screen became taller than we want
        else
        {
            mainCamera.orthographicSize = scaledHeight / PixelsToWorldUnitFactor;
        }
    }
}