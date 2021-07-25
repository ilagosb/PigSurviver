using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class EnemyPatrolState : StateMachineBehaviour
{

    private ALifeEntity _target;
    private Enemy _actor;
    private static readonly int Argue = Animator.StringToHash("Argue");


    private void OnGotDirty(Animator animator)
    {
        animator.SetBool(Dirty, true);
    }
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _target = GameModel.Instance.MainLifeEntity;
        _actor = animator.GetComponent<Enemy>();
        _actor.GotDirty += () =>
        {
            OnGotDirty(animator);
        };
        _actor.SetProvider(new ArgueZoneProvider(_actor.DistanceAggro, _actor.transform, LayerMask.GetMask("Player")));
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(_target.transform.position, _actor.transform.position) <= _actor.DistanceAggro)
            animator.SetBool(Argue, true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
