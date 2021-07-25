using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] 
    private TMPro.TextMeshProUGUI _labelTapForRepeat;

    [SerializeField] 
    private TMPro.TextMeshProUGUI _scoreText;

    public TextMeshProUGUI ScoreText => _scoreText;

    public void ShowScreen()
    {
        gameObject.SetActive(true);
        transform.DOMoveY(0, 1).SetEase(Ease.OutQuad);
        GetComponent<Image>().DOFade(.95f, 2).SetEase(Ease.OutQuad);
        _labelTapForRepeat.DOFade(0, 1).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene("GameplayScene");
    }
}
