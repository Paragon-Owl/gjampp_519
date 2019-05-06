using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo_anim : MonoBehaviour
{

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    public Canvas canvas;

    public float startDelay = 0.5f;
    public float fadeTime = 0.5f;
    private float fadingTime;

    public float timeToGlitchOut = 0.5f;

    public Color start;
    public Color middle;

    bool fadinIn;
    bool isGlitchIn;
    bool isGlitchOut;
    private float timeStart;

    Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        fadingTime = 0;
        spriteRenderer.color = start;

        fadinIn = true;
        isGlitchIn = false;
        isGlitchOut = false;
        sprite = spriteRenderer.sprite;
        timeStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timeStart<= startDelay)
            return;

        if(fadinIn)
        {
            fadingTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(start, middle, fadingTime/fadeTime);

            if(fadingTime >= fadeTime)
            {
                fadinIn = false;
                fadingTime = 0;
            }
        }
        else
        {
            if(!isGlitchIn)
            {
                isGlitchIn = true;
                anim.Play("GlitchIn");
            }
            else if (!isGlitchOut)
            {
                fadingTime += Time.deltaTime;
                if( fadingTime >= 0.4f )
                {
                    anim.StopPlayback();
                    spriteRenderer.sprite = sprite;
                }
                if(fadingTime >= timeToGlitchOut+0.4f)
                {
                    isGlitchOut = true;
                    fadingTime = 0;
                    anim.Play("GlitchOut");
                }
            }

            if(isGlitchOut)
            {
                fadingTime += Time.deltaTime;
                if( fadingTime >= 0.4f && fadingTime <= 0.41f)
                {
                    anim.StopPlayback();
                    spriteRenderer.sprite = sprite;
                }
                else if(fadingTime > 0.41f)
                {
                    fadingTime += Time.deltaTime;
                    spriteRenderer.color = Color.Lerp(middle, start, (fadingTime-0.41f)/fadeTime);

                    if(fadingTime >= 1f+fadeTime)
                    {
                        StartCoroutine("StartCanvas");
                        //gameObject.SetActive(false);
                    }
                }

            }
        }
    }

    IEnumerator StartCanvas()
    {
        yield return new WaitForSeconds(1f);
        canvas.gameObject.SetActive(true);
        Destroy(gameObject, 3f);
    }

}
