using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Flasher : MonoBehaviour
{
    private float _alpha = 0f;

    [SerializeField]
    private float _fadespeed = 1f;

    [SerializeField]
    private Image fadeimage;

    [SerializeField]
    private TextMeshProUGUI fadePlus;

    public void Fade()
    {
        StartCoroutine(fade());
    }

    private IEnumerator fade()
    {
        _fadespeed = 0.5f;
        while (_alpha < 1f)
        {
            yield return 0;
            ActualFade();
        }
        yield return new WaitForSeconds(3f);
        _fadespeed = -0.5f;
        while(_alpha > 0f)
        {
            yield return 0;
            ActualFade();
        }

        GameObject.Destroy(fadeimage.gameObject);
        GameObject.Destroy(fadePlus.gameObject);
        GameObject.Destroy(transform.gameObject);
    }

    void ActualFade()
    {
        _alpha += _fadespeed * Time.deltaTime;
        _alpha = Mathf.Clamp01(_alpha);

        var newcolor = Color.white;
        newcolor.a = _alpha; 

        fadeimage.color = newcolor;
        fadePlus.color = newcolor;
    }
}
