using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private MovementConfig config;
    public MovementConfig Config { get { return config; } private set { config = value; } }


    private void Update()
    {
        this.OnUpdate();
    }
    protected virtual void OnUpdate() { }
}
