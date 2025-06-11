using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DialogueActionController : MonoBehaviour
{
    private float _moveRange = 3f;
    private float _moveDuration = 7.5f;

    private Vector3 _initialPos = Vector3.zero;
    private GameObject jia;

    public void StartAutoMove()
    {
        if(DialogueCharacterController.Instance)
        {
            jia = DialogueCharacterController.Instance.GetCharacterGameObject("Áö¾Æ");
            if(jia !=null)
            {
                if(_initialPos == Vector3.zero)
                    _initialPos = jia.transform.position;

                jia.transform.DOMoveZ(_initialPos.z - _moveRange, _moveDuration)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        jia.transform.DOMoveZ(_initialPos.z, _moveDuration)
                        .SetEase(Ease.InOutSine)
                        .OnComplete(() =>
                        {
                            StartAutoMove();
                        });
                    });
            }
        }
    }

    public void EndAutoMove()
    {
        jia.transform.DOKill();
    }
}
