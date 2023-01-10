using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXLight_m1 : MonoBehaviour
{
    public GameObject HideGlobalObgect = null; // для сокрытия рендера
    public GameObject target = null;
    public LineRenderer LR = null;
    public float arcLength = 2.0f;
    public float arcVariation = 2.0f;
    public float inaccuracy = 1.0f;
    public float TimeEnam = 0.5f;
    [SerializeField]private float TimeEnamTek = 0;
    public float TimeDisable = 3.5f;
    [SerializeField] private float TimeDisableTek = 0;

    // Start is called before the first frame update
    void Start()
    {
        HideRenderSprite();
    }

    // Update is called once per frame
    void Update()
    {
        TimerEnamDisableObstacle();
        RENDERS();
    }
    //скрывает рендеры объектов
    void HideRenderSprite()
    {
        if (HideGlobalObgect != null)
            HideGlobalObgect.GetComponent<SpriteRenderer>().enabled = false;
        if (target != null)
            target.GetComponent<SpriteRenderer>().enabled = false;
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().enabled = false;
    }
    void RENDERS()
    {
        Vector3 lastPoint = transform.position;
        int i = 1;
        LR.SetPosition(0, transform.position);//make the origin of the LR the same as the transform
        while (Vector3.Distance(target.transform.position, lastPoint) > .5)
        {//was the last arc not touching the target?
            LR.SetVertexCount(i + 1);//then we need a new vertex in our line renderer
            var fwd = target.transform.position - lastPoint;//gives the direction to our target from the end of the last arc
            fwd.Normalize();//makes the direction to scale
            fwd = Randomize(fwd, inaccuracy);//we don't want a straight line to the target though
            fwd *= Random.Range(arcLength * arcVariation, arcLength);//nature is never too uniform
            fwd += lastPoint;//point + distance * direction = new point. this is where our new arc ends
            LR.SetPosition(i, fwd);//this tells the line renderer where to draw to
            i++;
            lastPoint = fwd;//so we know where we are starting from for the next arc
        }

    }
    Vector3 Randomize(Vector3 v3, float inaccuracy2)
    {
        v3 += new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * inaccuracy2;
        v3.Normalize();
        return v3;
    }

    void TimerEnamDisableObstacle()
    {
        if(TimeEnamTek<TimeEnam)
        {
            TimeEnamTek += Time.deltaTime * 1f;
            if (!LR.enabled)
                LR.enabled = true;
            if (!HideGlobalObgect.GetComponent<BoxCollider2D>().enabled)
                HideGlobalObgect.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            if(TimeDisableTek<TimeDisable)
            {
                TimeDisableTek += Time.deltaTime * 1f;
                if (LR.enabled)
                    LR.enabled = false;
                if (HideGlobalObgect.GetComponent<BoxCollider2D>().enabled)
                    HideGlobalObgect.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                TimeEnamTek = 0;
                TimeDisableTek = 0;
            }
        }
    }
}