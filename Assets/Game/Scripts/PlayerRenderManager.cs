using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRenderManager : MonoBehaviour
{
    public class ViewDirectionConst
    {
        public const bool LEFT = false;
        public const bool RIGHT = true;
        public const int L_QUATERNION_Y = 270;
        public const int R_QUATERNION_Y = 90;
        public const int F_QUATERNION_Y = 180;
    }
    private bool isViewingRight;
    public bool ViewDirection
    {
        get
        {
            return isViewingRight;
        }
        set
        {
            isViewingRight = value;
        }
    }
    public bool ViewFront
    {
        get; set;
    }
    public RuntimeAnimatorController animatorController;
    private Animator animator;

    void Awake()
    {
        ViewDirection = ViewDirectionConst.LEFT;
        ResetAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        int target_y = ViewDirectionConst.L_QUATERNION_Y;
        if (ViewDirection == ViewDirectionConst.RIGHT)
        {
            target_y = ViewDirectionConst.R_QUATERNION_Y;
        }
        if (ViewFront)
        {
            target_y = ViewDirectionConst.F_QUATERNION_Y;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, target_y, 0), 5.0f * Time.deltaTime);
    }
    public void FlipDirection()
    {
        ViewDirection = !ViewDirection;
    }
    public void SetAnimatorFloat(string name, float value)
    {
        if (animator == null)
        {
            ResetAnimator();
        }
        animator.SetFloat(name, value);
    }
    public void SetAnimatorBool(string name, bool value)
    {
        if (animator == null)
        {
            ResetAnimator();
        }
        animator.SetBool(name, value);
    }
    private void ResetAnimator()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Cannot find animator");
            return;
        }
        animator.runtimeAnimatorController = animatorController;
    }
}
