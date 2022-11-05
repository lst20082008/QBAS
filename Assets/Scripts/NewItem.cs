using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewItem : MonoBehaviour, IPointerDownHandler
{
    public Item item;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameObject.FindWithTag("GameController").GetComponent<GameController>().AddItemToStall(item)) {
            GameObject.FindWithTag("GameController").GetComponent<GameController>().isNewItemExisted = false;
            Destroy(this.gameObject);
        }
    }
}
