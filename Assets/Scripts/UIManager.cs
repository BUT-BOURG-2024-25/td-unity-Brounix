using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get {return _instance;}}
    private static UIManager _instance = null;

    [SerializeField]
    private Joystick joystick = null;

    public Joystick Joystick {get {return joystick;}}

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        } else 
        {
            Destroy(gameObject);
        }
    }

}
