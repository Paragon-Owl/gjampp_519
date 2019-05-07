using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceMenu : MonoBehaviour
{
    static private ChoiceMenu instance = null;

    public UpgradeCard[] cards = new UpgradeCard[3];

    private int activeCardIndex;

    public float cardSpeed = 1;
    private bool moving = false;
    private void Awake() {
        if(instance == null) instance = this; else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        activeCardIndex = 1;
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
            return;

        float value = Input.GetAxis("Horizontal");

        if(value > 0 || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(activeCardIndex!=0)
            {
                activeCardIndex--;

                foreach (UpgradeCard card in cards)
                {
                    card.targetPosition = card.transform.position - (Vector3.right * 5f);
                }
                StartCoroutine("MoveCards");
                moving = true;
            }
        }
        else if(value < 0  || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(activeCardIndex!=2)
            {
                activeCardIndex++;
                foreach (UpgradeCard card in cards)
                {
                    card.targetPosition = card.transform.position + (Vector3.right * 5f);
                }
                StartCoroutine("MoveCards");

                moving = true;
            }
        }

        if(Input.GetButtonDown("Fire1"))
        {
            Time.timeScale = 1;
        }
    }

    IEnumerator MoveCards()
    {
        while(cards[0].gameObject.transform.position != cards[0].targetPosition)
        {
            foreach (UpgradeCard card in cards)
            {
                card.gameObject.transform.position = Vector3.MoveTowards(card.transform.position, card.targetPosition, cardSpeed);
            }
            yield return null;
        }
        moving =false;
        Time.timeScale = 1;
    }

    static public void SetUpgradeCards(Stat.Type card1, Stat.Type card2, Stat.Type card3)
    {
        instance.gameObject.SetActive(true);
        instance.cards[0].Set(card1);
        instance.cards[1].Set(card2);
        instance.cards[2].Set(card3);
    }

    static public void SetUpgradeCards(AMJ.GunUpgrade gunCard, AMJ.SwordUpgrade swordCard, AMJ.OtherUpgrade otherCard)
    {
        instance.gameObject.SetActive(true);
        instance.cards[0].Set(gunCard);
        instance.cards[1].Set(swordCard);
        instance.cards[2].Set(otherCard);

    }
}
