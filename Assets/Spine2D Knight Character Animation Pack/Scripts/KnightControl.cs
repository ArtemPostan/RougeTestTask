using UnityEngine;
using System;
using Spine;
using Spine.Unity;
using UnityEngine.U2D;

public class KnightControl : MonoBehaviour
{
    #region Inspector
    // [SpineAnimation] attribute allows an Inspector dropdown of Spine animation names coming form SkeletonAnimation.
    [SpineAnimation]
    public string runAnimationName;

    [SpineAnimation]
    public string idleAnimationName;

    [SpineAnimation]
    public string walkAnimationName;

    [SpineAnimation]
    public string atkAnimationName_1;

    [SpineAnimation]
    public string atkAnimationName_2;

    [SpineAnimation]
    public string jumpAnimationName;

    [SpineAnimation]
    public string hitAnimationName;

    [SpineAnimation]
    public string deathAnimationName;

    [SpineAnimation]
    public string stunAnimationName;

    [SpineAnimation]
    public string skillAnimationName_1;
    [SpineAnimation]
    public string skillAnimationName_2;
    [SpineAnimation]
    public string skillAnimationName_3;
    #endregion
    private bool _isDead = false;

    SkeletonAnimation skeletonAnimation;

    // Spine.AnimationState and Spine.Skeleton are not Unity-serialized objects. You will not see them as fields in the inspector.
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;
    // Start is called before the first frame update
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
    }

    public void running()
    {
        if (_isDead) return;
        spineAnimationState.SetAnimation(0, runAnimationName, true);
    }
    public void walking()
    {
        if (_isDead) return;
        spineAnimationState.SetAnimation(0, walkAnimationName, true);
    }
    public void idle()
    {
        if (_isDead) return;
        spineAnimationState.SetAnimation(0, idleAnimationName, true);
    }
    public void jump()
    {
        if (_isDead) return;
        spineAnimationState.SetAnimation(0, jumpAnimationName, true);
    }
    public void getHit()
    {
        if (_isDead) return;
        var current = spineAnimationState.GetCurrent(0);

        if (current != null && current.Animation.Name == atkAnimationName_1)
        {            
            return;
        }

        spineAnimationState.SetAnimation(0, hitAnimationName, false);
        spineAnimationState.AddAnimation(0, idleAnimationName, true, 0f);
    }
    public void death()
    {
        if (_isDead) return; 

        _isDead = true;
        spineAnimationState.SetAnimation(0, deathAnimationName, false);
    }
    public void stun()
    {
        if (_isDead) return;
        spineAnimationState.SetAnimation(0, stunAnimationName, true);
    }
    public void attack_1()
    {
        if (_isDead) return;
        
        spineAnimationState.SetAnimation(0, atkAnimationName_1, false);
        
        spineAnimationState.AddAnimation(0, idleAnimationName, true, 0);
    }
    public void attack_2()
    {
        if (_isDead) return;
        spineAnimationState.SetAnimation(0, atkAnimationName_2, false);
        spineAnimationState.AddAnimation(0, idleAnimationName, true, 0);
    }
    public void skill_1()
    {
        if (_isDead) return;
        spineAnimationState.SetAnimation(0, skillAnimationName_1, true);
    }
    public void skill_2()
    {
        if (_isDead) return;
        spineAnimationState.SetAnimation(0, skillAnimationName_2, true);
    }
    public void skill_3()
    {
        if (_isDead) return;
        spineAnimationState.SetAnimation(0, skillAnimationName_3, true);
    }   

}
