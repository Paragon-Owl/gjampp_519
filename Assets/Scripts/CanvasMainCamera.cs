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
            if(Input.GetAxis("Vertical")*10>5)
            {
                activeButtonIndex = activeButtonIndex-1<0?buttons.Count-1:activeButtonIndex-1;
                ev.SetSelectedGameObject(buttons[activeButtonIndex].gameObject);
                lastDelay = 0;
            }
            else if(Input.GetAxis("Vertical")*10<-5)
            {
                activeButtonIndex = (activeButtonIndex+1)%buttons.Count;
                ev.SetSelectedGameObject(buttons[activeButtonIndex].gameObject);
                lastDelay = 0;
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
