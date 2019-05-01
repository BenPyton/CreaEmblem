using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[DefaultExecutionOrder(-1)]
public class AnimatedSprite : MonoBehaviour {


    [SerializeField] public SpriteSheet sheet;
    [SerializeField] public float animationSpeed = 5.0f;
    [SerializeField] public float row = 0;
    [SerializeField] public bool pingPong = false;
    protected bool animate = true;
    protected int animIndex = 0;
    protected bool reverse = false;


    protected SpriteRenderer sr;
    
    public bool flipX
    {
        get { return sr != null ? sr.flipX : false; }
        set { if (sr != null) sr.flipX = value; }
    }

    public bool flipY
    {
        get { return sr != null ? sr.flipY : false; }
        set { if(sr != null) sr.flipY = value; }
    }


    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Animate());
	}
	
	// Update is called once per frame
	void Update () {
        sr.sprite = sheet[(int)row, animIndex];
	}

    protected IEnumerator Animate()
    {
        animIndex = 0;
        while (animate)
        {
            if(pingPong)
            {
                if (reverse && animIndex == 0)
                    reverse = false;
                else if (!reverse && animIndex == sheet.columnCount - 1)
                    reverse = true;
            }

            animIndex = (animIndex + (reverse ? -1 : 1)) % sheet.columnCount;
            yield return new WaitForSeconds(1.0f / animationSpeed);
        }
    }
}
