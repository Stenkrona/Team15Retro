using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Playerinput")]
public class PlayerInput : ScriptableObject
{
    public KeyCode upThruster;
    public KeyCode downThruster;
    public KeyCode leftThurster;
    public KeyCode rightThurster;
    public string horizontalAxis;
    public KeyCode exitGame;
}
