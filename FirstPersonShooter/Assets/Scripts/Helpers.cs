using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class Helpers
{
    public static async Task Cooldown(int millisecondsDelay)
    {
        await Task.Delay(millisecondsDelay);
    }
    public static Vector3 RayCastFromCamera(Transform camera, float maxDistance, Action onClick) {
        Vector3 destination = camera.position + camera.forward * maxDistance;
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, maxDistance))
        {
            destination = hit.point;
            onClick();
        }
        return destination;
    }
}
