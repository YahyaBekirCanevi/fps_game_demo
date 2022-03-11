using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager instance = null;
    public static Manager Instance {
        get => instance;
    }
    [SerializeField] private float gravity = 12;
    [Range(1, 4)][SerializeField] private float mouseSensitivity;

    private void Awake() {
        instance = this;
    }

    public float Gravity {
        get => gravity;
    }
    public float MouseSensitivity {
        get => mouseSensitivity;
    }
}
