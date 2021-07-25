using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameOverState : StateMachineBehaviour
{
    private GameModel _model;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model = animator.GetComponent<GameModel>();
        Pig pig = (Pig)_model.MainLifeEntity;
        Instantiate(pig.Meat, pig.transform.position, Quaternion.identity);
        _model.GameOverScreen.ScoreText.text = _model.Score.ToString();
        _model.GameOverScreen.ShowScreen();
        _model.Joystick.enabled = false;
        _model.BackgroundMusic.DOFade(0, .5f).SetEase(Ease.Flash);
        Destroy(_model.MainLifeEntity.gameObject);
    }
}
