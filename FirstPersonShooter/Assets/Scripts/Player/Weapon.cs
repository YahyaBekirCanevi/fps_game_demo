using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private LineRenderer lineRenderer;
    
    [Header("Variables")]
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private bool shoot = false;
    [SerializeField] private float maxDistance = 100;
    [SerializeField] private int cooldownAfterShot = 400;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !shoot) {
            ShootRay(cooldownAfterShot);
        }
    }
    private void LateUpdate() {
        if(shoot)
        {
            Vector3 destination = Helpers.RayCastFromCamera(Camera.main.transform, maxDistance, () => {});
            lineRenderer.SetPosition(0, barrelTransform.position);
            lineRenderer.SetPosition(1, destination);
        }
    }
    private async void ShootRay(int millisecondsDelay){
        shoot = lineRenderer.enabled = true;

        await Helpers.Cooldown(millisecondsDelay);

        shoot = lineRenderer.enabled = false;
    }
    
}
