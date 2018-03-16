using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FloatingMessageController : MonoBehaviour {

    [SerializeField] Vector3 move;
    [SerializeField] float time;
    [SerializeField] Text text; 

    public void Init(string message, Color color)
    {
        text.text = message;
        text.color = color;
        StartCoroutine(MessageRoutine());
    }

    IEnumerator MessageRoutine()
    {
        Color start = text.color;
        Color end = start;
		end.a = 0f;

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + move;

        float clock = time;

        while (clock > 0f)
        {
            clock -= Time.deltaTime;
            float t = 1f - (clock / time);
            text.color = Color.Lerp(start, end, t);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        Destroy(gameObject);
    }

}
