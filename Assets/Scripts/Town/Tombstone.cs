using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tombstone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] TownsPerson person;
    [SerializeField] Text title;
    [SerializeField] Text epitaph;

    [SerializeField] new Collider2D collider;

    public void OnPointerExit(PointerEventData eventData)
    {
        title.enabled = false;
        epitaph.enabled = false;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        title.enabled = true;
        epitaph.enabled = true;
    }

    // Use this for initialization
    void Start () {
        title.text = person.name;
        epitaph.text = person.epitaph;

	}
}
