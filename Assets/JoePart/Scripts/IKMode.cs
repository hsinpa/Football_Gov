using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKMode : MonoBehaviour {

    
    protected Animator animator;
   
    public bool ikActive = false;
    
    public Transform rightFootObj = null;

    public Transform LeftFootObj = null;
    void Start()
    {
        
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void OnAnimatorIK()
    {
        if (ikActive)
        {

            //weight = 1.0 for the right hand means position and rotation will be at the IK goal (the place the character wants to grab)
            
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0.3f);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0.3f);

            //set the position and the rotation of the right hand where the external object is
            if (rightFootObj != null)
            {
                
                animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootObj.position);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootObj.rotation);

                animator.SetIKPosition(AvatarIKGoal.LeftFoot, LeftFootObj.position);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, LeftFootObj.rotation);
            }

        }
    }
}
