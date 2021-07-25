using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;

public class GamePlayingState : StateMachineBehaviour
{
    private GameModel _model;
    private int _enemyCount = 1;
    private static readonly int GameOver = Animator.StringToHash("GameOver");

    private void OnLifeChanged(Animator animator, int oldValue, int newValue)
    {
        if (newValue < 1)
        {
            animator.SetTrigger(GameOver);
        }
    }

    private void OnFoodEaten()
    {
        _model.Score += 50;
        CreateFood();
    }

    private void OnEnemyDeath()
    {
        CreateEnemy();
    }
    
    private void OnScoreChanged()
    {
        _model.ScoreText.text = _model.Score.ToString();
        if (_enemyCount - 1 < _model.Score / 100)
        {
            _enemyCount++;
            CreateEnemy();
        }
    }

    private void CreateEnemy()
    {
        Enemy enemy = _model.GeneratorEnemy.Generate();
        //enemy.Died += OnEnemyDeath;
    }

    

    private void CreateFood()
    {
        Food food  = _model.GeneratorFood.Generate();
        food.Eaten += OnFoodEaten;
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model = animator.GetComponent<GameModel>();
        OnScoreChanged();
        _model.ScoreChanged += OnScoreChanged;
        _model.MainLifeEntity.LifeCountChanged += (v1, v2) =>
        {
            OnLifeChanged(animator,v1, v2);
        };
        
        CreateFood();
        CreateEnemy();
    }




}