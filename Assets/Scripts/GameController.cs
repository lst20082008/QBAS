using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int days;
    public int money;
    public GameObject background;
    public Transform[] itemPos;
    public Transform[] customerPos;
    public GameObject itemPrefab;
    public GameObject customerPrefab;

    /* test */
    public Item testItem;
    /* test */

    public GameObject[] itemInstanceNow;
    public GameObject[] customerNow;

    // Start is called before the first frame update
    void Start()
    {
        itemInstanceNow = new GameObject[9];
        customerNow = new GameObject[10];
    }

    // Update is called once per frame
    void Update()
    {
        /* test */
        if (Input.GetKeyUp(KeyCode.Q)) {
            int i = 9;
            while (i > 0) {
                AddItemToStall(testItem);
                i--;
            }
            
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            int i = 8;
            while (i >= 0)
            {
                if (itemInstanceNow[i] == null) { return; }
                RemoveItemInStall(itemInstanceNow[i].GetComponent<ItemInstance>());
                i--;
            }

        }

        if (Input.GetKeyUp(KeyCode.R)) {
            AddCustomer();
        }
        /* test */
    }

    public bool AddItemToStall(Item item)
    {
        int thisCount = -1;
        for (int i = 0; i < 9; i++) {
            if (itemInstanceNow[i] == null) {
                thisCount = i;
                break;
            }
        }
        if (thisCount == -1) {
            return false;
        }
        GameObject thisInstance = Instantiate(itemPrefab, itemPos[thisCount].transform);
        thisInstance.transform.SetParent(background.transform);
        ItemInstance ii = thisInstance.GetComponent<ItemInstance>();
        ii.item = item;
        ii.posInStall = thisCount;
        ii.valueNow = item.baseValue;
        ii.quality = Random.Range(0.5f, 2f);
        itemInstanceNow[thisCount] = thisInstance;
        return true;
    }

    public void RemoveItemInStall(ItemInstance ii)
    {
        if (ii == null) {
            return;
        }
        itemInstanceNow[ii.posInStall] = null;
        Destroy(ii.gameObject);
    }

    public bool AddCustomer()
    {
        // find weather there is a space for new customer
        int thisCount = -1;
        for (int i = 0; i < 10; i++) {
            if (customerNow[i] == null) {
                thisCount = i;
                break;
            }
        }
        if (thisCount == -1) {
            return false;
        }
        // find how many items we have
        int itemNum = 0;
        for (int i = 0; i < 9; i++) {
            if (itemInstanceNow[i] == null) {
                continue;
            }
            itemNum++;
        }
        if (itemNum == 0) {
            return false;
        }
        // rand choose one item
        int wantedItem = Random.Range(1, itemNum);
        int wantedItemIndex = -1;
        for (int i = 0; i < 9; i++)
        {
            if (itemInstanceNow[i] == null)
            {
                continue;
            }
            wantedItem--;
            if (wantedItem == 0) {
                wantedItemIndex = i;
            }
        }
        // generate customer
        GameObject thisInstance = Instantiate(customerPrefab, customerPos[thisCount].transform);
        thisInstance.transform.SetParent(background.transform);
        Customer cm = thisInstance.GetComponent<Customer>();
        cm.myPos = thisCount;
        cm.remainTime = Random.Range(20, 30);
        // create link bewteen customer and item
        cm.wantedItemPos = wantedItemIndex;
        cm.transform.GetChild(0).GetComponent<Image>().sprite = itemInstanceNow[wantedItemIndex].GetComponent<ItemInstance>().item.itemImg;
        itemInstanceNow[wantedItemIndex].GetComponent<ItemInstance>().wantedCustomers.Add(cm);
        customerNow[thisCount] = thisInstance;
        return true;
    }

    public void RemoveCustomer(Customer cm)
    {
        if (cm.wantedItemPos != -1) {
            if (itemInstanceNow[cm.wantedItemPos]) {
                itemInstanceNow[cm.wantedItemPos].GetComponent<ItemInstance>().wantedCustomers.Remove(cm);
            }
        }
        customerNow[cm.myPos] = null;
        Destroy(cm.gameObject);
    }
}
