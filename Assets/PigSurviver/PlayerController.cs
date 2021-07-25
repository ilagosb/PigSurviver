using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Pig _pig;

    [SerializeField]
    private Joystick _joystick;

    [SerializeField]
    private SpellButtonAnimationController _spellButtonAnimationBomb;
    
    [SerializeField]
    private SpellButtonAnimationController _spellButtonAnimationShit;

    private void FixedUpdate() {
        Vector2 direction = _joystick.Horizontal != 0 ? Vector2.right * _joystick.Horizontal : Vector2.up * _joystick.Vertical;
        _pig.Move(direction);
    }

    public void MakeShit()
    {
        if (_canMakeShit)
        {
            _canMakeShit = false;
            var sequence = _spellButtonAnimationShit.Hide();
            _pig.CreateShit();
            sequence.OnComplete(() =>
            {
                StartCoroutine(RechargeSpell(3,
                    () => _spellButtonAnimationShit.Show(3),
                    () => _canMakeShit = true));
            });

        }
    }

    private bool _canMakeShit = true;

    public void MakeBomb()
    {
        if (_canMakeBomb)
        {
            _canMakeBomb = false;
            var sequence = _spellButtonAnimationBomb.Hide();
            _pig.CreateBomb();
            sequence.OnComplete(() =>
            {
                StartCoroutine(RechargeSpell(5,
                    () => _spellButtonAnimationBomb.Show(5),
                    () => _canMakeBomb = true));
            });
        }
        
    }

    private bool _canMakeBomb = true;

    private IEnumerator RechargeSpell(float time, UnityAction beforeCharge, UnityAction afterCharge)
    {
        beforeCharge.Invoke();
        yield return new WaitForSecondsRealtime(time);
        afterCharge.Invoke();
    }
}
