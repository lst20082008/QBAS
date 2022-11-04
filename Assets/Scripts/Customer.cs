using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public int wantedItemPos;
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
        }
        remainTime -= Time.deltaTime;
        if (remainTime <= 0) {
            GameObject.FindWithTag("GameController").GetComponent<GameController>().RemoveCustomer(this);
        }
    }

    public void ClearWanted()
    {
        wantedItemPos = -1;
    }
}
