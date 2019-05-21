using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonA : MonoBehaviour
{

    static public bool pressed;
    private SphereCollider _collider;
    private List<int> touches = new List<int>();
    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (!pressed)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touchInfo = Input.GetTouch(i);
                Ray r = Camera.main.ScreenPointToRay(touchInfo.position);
                RaycastHit hitInfo;
                if (_collider.Raycast(r, out hitInfo, 1000000))
                {
                    touches.Add(touchInfo.fingerId);
                    pressed = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (touches.Contains(Input.GetTouch(i).fingerId) && Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    touches.Remove(Input.GetTouch(i).fingerId);
                    pressed = false;
                }
            }
        }
    }
}
