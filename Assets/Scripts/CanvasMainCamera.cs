using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CanvasMainCamera : MonoBehaviour
{
    public EventSystem ev;
    public List<Button> buttons;
    public int activeButtonIndex = 0;
    public float delay = 0.25f;
    public float lastDelay = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        ev = Camera.main.GetComponent<EventSystem>();
    }

    private void Update()
    {
        lastDelay += Time.deltaTime;

        if(lastDelay >= delay)
        {

            float value = Input.GetAxis("Vertical");

            if(value > 0)
            {
                if(activeButtonIndex != 0)
                {
                    activeButtonIndex--;
                }
            }
            else if(value < 0)
            {
                if(activeButtonIndex != 2)
                    activeButtonIndex++;
            }

            ev.SetSelectedGameObject(buttons[activeButtonIndex].gameObject);
            lastDelay = 0;
        }

        if(ButtonA.pressed)
        {
            switch(activeButtonIndex)
            {
                case 0:
                    PlayGame();
                    break;
                case 1:
                    PlayCredit();
                    break;
                case 2:
                    Quit();
                    break;
            }
        }
    }

    public void PlayGame()
    {
        GameManager.instance.PlayGame();
    }

    public void PlayCredit()
    {
        //GameManager.instance.PlayCredit();
        Debug.Log("PIPI");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
