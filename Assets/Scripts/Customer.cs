using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public Item item;
    public int myPos;
    public float remainTime;
    private float cdTime;
    // Start is called before the first frame update
    void Start()
    {
        cdTime = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        cdTime -= Time.deltaTime;
        if (cdTime < 0) {
            // rechoose
            GameObject.FindWithTag("GameController").GetComponent<GameController>().ChooseOneType(this);
            cdTime = 10f;
        }
        remainTime -= Time.deltaTime;
        if (remainTime <= 0) {
            GameObject.FindWithTag("GameController").GetComponent<GameController>().RemoveCustomer(this);
        }
    }

    public void ClearWanted()
    {
        transform.GetChild(0).GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        transform.GetChild(0).GetComponent<Image>().sprite = null;
        GameObject.FindWithTag("GameController").GetComponent<GameController>().dislink(this);
        item = null;
    }
}
