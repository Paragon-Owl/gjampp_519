using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonB : MonoBehaviour
{

    static public bool pressed = false;
    static bool inside = false;

    private void FixedUpdate() {
        pressed = false;
    }


    private void OnMouseEnter() {
        inside = true;
    }

    private void OnMouseExit() {
        inside = false;
    }

    private void OnMouseDown() {
        if(inside)
            pressed = true;
    }

    private void OnMouseUpAsButton() {
        pressed = true;
    }

}
