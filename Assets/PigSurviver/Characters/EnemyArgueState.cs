using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArgueState : StateMachineBehaviour
{
    private Enemy _actor;

    private ALifeEntity _entity;
    private static readonly int Argue = Animator.StringToHash("Argue");

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _entity = GameModel.Instance.MainLifeEntity;
        _actor = animator.GetComponent<Enemy>();
        _actor.Speed += _actor.ArgueSpeedIncrease;
        _actor.Audio.clip = _actor.AngrySound;
        _actor.Audio.Play();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(_actor.transform.position, _entity.transform.position) > _actor.DistanceAggro)
        {
            animator.SetBool(Argue, false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _actor.Speed -= _actor.ArgueSpeedIncrease;
        _actor.Audio.clip = _actor.IdleSound;
        _actor.Audio.Stop();
        _actor.Audio.Play();
    }

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
