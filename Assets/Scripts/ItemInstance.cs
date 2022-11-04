using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInstance : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Item item;
    public int valueNow;
    public int posInStall;
    public float quality;
    public ArrayList wantedCustomers;
    private ArrayList collisions;
    private bool isDown;
    private Vector3 dist;
    private float posX;
    private float posY;
    // Start is called before the first frame update
    void Start()
    {
        collisions = new ArrayList();
        this.gameObject.GetComponent<Image>().sprite = item.itemImg;
        isDown = false;
        wantedCustomers = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDown) {
            Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y -  posY, dist.z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
            this.transform.position = worldPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Customer")) { return; }
        Debug.Log("enter" + collision);
        collisions.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Customer")) { return; }
        Debug.Log("exit" + collision);
        collisions.Remove(collision);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        dist = Camera.main.WorldToScreenPoint(this.transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
        if (collisions.Count == 0)
        {
            this.transform.position = Camera.main.ScreenToWorldPoint(dist);
            return;
        }
        GameObject touchCustomer;
        int nearlistIndex = -1;
        float minDistance = float.MaxValue;
        if (collisions.Count > 1) {
            // find nearest collision
            for (int i = 0; i < collisions.Count; i++) {
                Transform tCol = ((Collider2D)collisions[0]).transform;
                float thisDistance = Vector2.Distance(new Vector2(tCol.position.x, tCol.position.y), new Vector2(transform.position.x, transform.position.y));
                if (thisDistance < minDistance) {
                    minDistance = thisDistance;
                    nearlistIndex = i;
                }
            }
            touchCustomer = ((Collider2D)collisions[nearlistIndex]).gameObject;
        } else {
            touchCustomer = ((Collider2D)collisions[0]).gameObject;
        }
        if (wantedCustomers.Contains(touchCustomer.GetComponent<Customer>()))
        {
            for (int i = 0; i < collisions.Count; i++)
            {
                if (((Collider2D)collisions[0]).GetComponent<Customer>() != touchCustomer.GetComponent<Customer>())
                {
                    ((Collider2D)collisions[0]).GetComponent<Customer>().ClearWanted();
                }
                else {
                    GameObject.FindWithTag("GameController").GetComponent<GameController>().RemoveCustomer(touchCustomer.GetComponent<Customer>());
                    GameObject.FindWithTag("GameController").GetComponent<GameController>().money += this.valueNow;
                }
            }
            GameObject.FindWithTag("GameController").GetComponent<GameController>().RemoveItemInStall(this);
        }
        else {
            this.transform.position = Camera.main.ScreenToWorldPoint(dist);
        }
    }
}
