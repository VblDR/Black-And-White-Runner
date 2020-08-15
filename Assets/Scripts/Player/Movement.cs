using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    //player components
    public Transform myTransform;
    public SpriteRenderer mySprite;
    public CircleCollider2D collider;

    //orher preferences
    private bool isWhite = true;
    private Touch touch;
    public float timeToChange = 1f, timeToTap = 0.3f;
    private float coolDown;
    private Coroutine changeColor, changeScale, enablePhysics;
    private Color color;
    private Vector3 scaleTo;

    public ParticleSystem trail;

    public void Update()
    {
        CheckTap();
        coolDown -= Time.deltaTime;
    }

    private void CheckTap()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Ended)
            {
                ChangeSide();
            }
        }
#endif
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
        {
            ChangeSide();
        }
#endif
    }

    private void ChangeSide()
    {
        if (coolDown <= 0)
        {
            //stop all enabled coroutines 
            if (changeColor != null) StopCoroutine(changeColor);
            if (changeScale != null) StopCoroutine(changeScale);
            if (enablePhysics != null)
            {

                StopCoroutine(enablePhysics);
                if (Physics2D.gravity == new Vector2(0, 0.5f) || Physics2D.gravity == new Vector2(0, -0.5f))
                    Physics2D.gravity *= -2;
            }

            // check on which side player is
            if (isWhite)
            {
                scaleTo = new Vector3(0.5f, -0.5f, 0.5f);
                color = Color.black;
                trail.startColor = Color.black;
            }
            else
            {
                color = Color.white;
                scaleTo = new Vector3(0.5f, 0.5f, 0.5f);
                trail.startColor = Color.white;
            }
            //switch side
            isWhite = !isWhite;

            //decrease physics
            Physics2D.gravity *= 0.5f;
            collider.enabled = false;

            //coroutines
            changeColor = StartCoroutine(ChangeColor(color, timeToChange));
            changeScale = StartCoroutine(ChangeScale(scaleTo, timeToChange));
            enablePhysics = StartCoroutine(EnablePhysics(timeToChange));
            coolDown = timeToTap;
        }
    }

    //change color to black or white
    private IEnumerator ChangeColor(Color endColor, float timeToChangeColor )
    {
        float curTime = 0f;
        do
        {
            mySprite.color = Color.Lerp(mySprite.color, endColor, curTime / timeToChangeColor);
            curTime += Time.deltaTime;
            yield return null;
        }
        while (curTime <= timeToChange);
    }

    //scale to run on black or white side
    private IEnumerator ChangeScale(Vector3 scale, float timeToChangeScale)
    {
        float curTime = 0f;
        do
        {
            myTransform.localScale = Vector3.Lerp(myTransform.localScale, scale, curTime/ timeToChangeScale);
            curTime += Time.deltaTime;
            yield return null;
        }
        while (curTime <= timeToChange);
    }

    //return physics after some time
    private IEnumerator EnablePhysics(float timeToEnable)
    {
        float curTime = 0f;
        do
        {
            curTime += Time.deltaTime;
            yield return null;
        }
        while (curTime <= timeToEnable/2);
        Physics2D.gravity *= -2;
        collider.enabled = true;
    }
}
