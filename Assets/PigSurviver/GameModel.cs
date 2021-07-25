using System;
using UnityEngine;
using UnityEngine.Events;

public class GameModel : MonoBehaviour
{
    private int _scores = 0;
    public int Score
    {
        get => _scores;
        set
        {
            _scores = value;
            EventScoreChanged();
        }
    }
    
    [SerializeField]
    private TMPro.TextMeshProUGUI _scoreText;
    public TMPro.TextMeshProUGUI ScoreText => _scoreText;

    [SerializeField] 
    private GameOverScreen _gameOverScreen;
    public GameOverScreen GameOverScreen => _gameOverScreen;

    

    private event UnityAction EventScoreChanged;
    public event UnityAction ScoreChanged
    {
        add => EventScoreChanged += value;
        remove => EventScoreChanged += value;
    }

    [SerializeField]
    private ALifeEntity _mainLifeEntity;
    public ALifeEntity MainLifeEntity => _mainLifeEntity;

    [SerializeField]
    private GeneratorFood _generatorFood;

    public GeneratorFood GeneratorFood => _generatorFood;
    
    [SerializeField]
    private GeneratorEnemy _generatorEnemy;

    public GeneratorEnemy GeneratorEnemy => _generatorEnemy;

    [SerializeField] 
    private Joystick _joystick;
    public Joystick Joystick => _joystick;

    [SerializeField] 
    private GameArea _gameArea;
    public GameArea GameArea => _gameArea;

    [SerializeField] 
    private AudioSource _backgroundMusic;

    private static GameModel _instance;
    public static GameModel Instance => _instance;

    public AudioSource BackgroundMusic => _backgroundMusic;
    
    [SerializeField]
    private Pathfinder _pathfinder;

    public Pathfinder Pathfinder => _pathfinder;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
}