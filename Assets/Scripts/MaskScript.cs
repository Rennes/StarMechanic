using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskScript : MonoBehaviour
{
    public bool IsFull = false;
    public float IncreaseSpeed;
    public float DecreaseSpeed;

    public ParticleSystem BuildingParts;

    public float MaximumScale = 1;
    public float MinimumScale = 0;

    public bool isHoldingMouseButton;

    public Transform maskTransform;

    public Animator animator;

    public LayerMask layerMask;

    public AudioSource terramorphingSound;
    public float terramorphinDefaultVolume = 1;

    // Start is called before the first frame update
    void Start()
    {
        maskTransform = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !animator.GetBool("isMoving"))
        {
            RaycastHit2D hit= Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, layerMask);
            if (hit.transform == this.transform)
            {
                Debug.Log("hit");
                Debug.Log(hit.transform.gameObject.name);
                isHoldingMouseButton = true;
            }
            else if(isHoldingMouseButton)
            {
                FailedHit();
            }
        }
        else if(isHoldingMouseButton)
        {
            FailedHit();
        }

        if (IsFull)
        {
            animator.SetBool("isRepairing", false);

            IEnumerator stopSlowlyTerramorphingSound()
            {
                while(terramorphingSound.volume>=0)
                {
                    yield return new WaitForSeconds(0.05f);
                    terramorphingSound.volume -= 0.05f;
                }

                terramorphingSound.Stop();
            }

            if (terramorphingSound.isPlaying)
            {
                StartCoroutine(stopSlowlyTerramorphingSound());
            }

            if (BuildingParts.isPlaying)
                BuildingParts.Stop();
            return;
        }

        if (isHoldingMouseButton)
        {
            if (animator.GetBool("isMoving"))
            {
                return;
            }

            if(!terramorphingSound.isPlaying)
            {
                terramorphingSound.volume = terramorphinDefaultVolume;
                terramorphingSound.Play();
            }

            if (!BuildingParts.isPlaying)
                BuildingParts.Play();

            if (maskTransform.localScale.x >= MaximumScale)
            {
                transform.parent.GetComponent<BoxCollider2D>().enabled = true;
                IsFull = true;
                return;
            }

            animator.SetBool("isRepairing", true);

            if (maskTransform.localScale.x < MaximumScale)
            {
                maskTransform.localScale = new Vector3(maskTransform.localScale.x + IncreaseSpeed * Time.deltaTime,
                    maskTransform.localScale.y + IncreaseSpeed * Time.deltaTime,
                    maskTransform.localScale.z);
            }
        }

        else
        {
            if (maskTransform.localScale.x > MinimumScale)
            {
                maskTransform.localScale = new Vector3(maskTransform.localScale.x - DecreaseSpeed * Time.deltaTime,
                    maskTransform.localScale.y - DecreaseSpeed * Time.deltaTime,
                    maskTransform.localScale.z);

                terramorphingSound.volume = maskTransform.localScale.x / MaximumScale;
                if(maskTransform.localScale.x<=0)
                {
                    terramorphingSound.Stop();
                }
            }
        }
    }

    private void FailedHit()
    {
        animator.SetBool("isRepairing", false);

        if (BuildingParts.isPlaying)
            BuildingParts.Stop();
        isHoldingMouseButton = false;
    }

    //private void OnMouseDrag()
    //{
    //    isHoldingMouseButton = true;

    //    //Debug.Log("YEP");
    //}

    //private void OnMouseUp()
    //{
    //    animator.SetBool("isRepairing", false);

    //    if (BuildingParts.isPlaying)
    //        BuildingParts.Stop();
    //    isHoldingMouseButton = false;
    //}
}
