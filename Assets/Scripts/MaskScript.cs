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
            if (hit)
            {
                Debug.Log("hit");
                Debug.Log(hit.transform.gameObject.name);
                isHoldingMouseButton = true;
            }
            else
            {
                FailedHit();
            }
        }
        else
        {
            FailedHit();
        }

        if (IsFull)
        {
            animator.SetBool("isRepairing", false);

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

            if (!BuildingParts.isPlaying)
                BuildingParts.Play();

            if (maskTransform.localScale.x >= MaximumScale)
            {
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
