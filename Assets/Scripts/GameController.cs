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
    public Transform newItemPos;
    public GameObject itemPrefab;
    public GameObject customerPrefab;
    public GameObject newItemPrefab;
    public Item[] items;
    // <Item, ArrayList<Customer>>
    public Hashtable wantedCusomer;

    /* test */
    public Item testItem;
    /* test */

    public GameObject[] itemInstanceNow;
    public GameObject[] customerNow;
    public bool isNewItemExisted;

    // Start is called before the first frame update
    void Start()
    {
        itemInstanceNow = new GameObject[9];
        customerNow = new GameObject[10];
        wantedCusomer = new Hashtable();
        isNewItemExisted = false;
        for (int i = 0; i < items.Length; i++) {
            wantedCusomer.Add(items[i], new ArrayList());
        }
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
        // if there is no this type of item
        // notify all cm
        if (!GetAllItemCategoty().Contains(ii.item)) {
            for (int i = 0; i < ((ArrayList)wantedCusomer[ii.item]).Count; i++) {
                ((Customer)((ArrayList)wantedCusomer[ii.item])[i]).ClearWanted();
                i--;
            }
        }
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
        GameObject thisInstance = Instantiate(customerPrefab, customerPos[thisCount].transform);
        thisInstance.transform.SetParent(background.transform);
        Customer cm = thisInstance.GetComponent<Customer>();
        cm.myPos = thisCount;
        cm.remainTime = Random.Range(20, 30);
        cm.ClearWanted();
        ChooseOneType(cm);
        customerNow[thisCount] = thisInstance;
        return true;
    }

    public void RemoveCustomer(Customer cm)
    {
        if (cm.item != null) {
            ((ArrayList)wantedCusomer[cm.item]).Remove(cm);
        }
        customerNow[cm.myPos] = null;
        Destroy(cm.gameObject);
    }

    public void dislink(Customer cm)
    {
        if (cm.item != null)
        {
            ((ArrayList)wantedCusomer[cm.item]).Remove(cm);
        }
    }

    public bool AddNewItem()
    {
        if (isNewItemExisted) { return false; }
        isNewItemExisted = true;
        int choosedItemIndex = Random.Range(0, items.Length);
        GameObject thisInstance = Instantiate(newItemPrefab, newItemPos);
        thisInstance.GetComponent<NewItem>().item = items[choosedItemIndex];
        thisInstance.transform.GetComponent<Image>().sprite = items[choosedItemIndex].itemImg;
        return true;
    }

    public ArrayList GetAllItemCategoty()
    {
        ArrayList ret = new ArrayList();
        for (int i = 0; i < itemInstanceNow.Length; i++) {
            if (itemInstanceNow[i] != null) {
                Item itemNow = itemInstanceNow[i].GetComponent<ItemInstance>().item;
                if (!ret.Contains(itemNow)) {
                    ret.Add(itemNow);
                }
            }
        }
        return ret;
    }

    public void ChooseOneType(Customer cm)
    {
        // find how many items we have
        ArrayList itemsNow = GetAllItemCategoty();
        Debug.Log(itemsNow.Count);
        if (itemsNow.Count == 0) { return; }
        // rand choose one item
        int wantedItemIndex = Random.Range(0, itemsNow.Count);
        // generate customer
        cm.item = ((Item)itemsNow[wantedItemIndex]);
        // create link bewteen customer and item
        cm.transform.GetChild(0).GetComponent<Image>().sprite = ((Item)itemsNow[wantedItemIndex]).itemImg;
        cm.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        ((ArrayList)wantedCusomer[((Item)itemsNow[wantedItemIndex])]).Add(cm);
    }
}
