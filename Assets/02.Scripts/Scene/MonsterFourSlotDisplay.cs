using UnityEngine;
using UnityEngine.UI;

public class MonsterFourSlotDisplay : MonoBehaviour
{
    public Image slot1;
    public Image slot2;
    public Image slot3;
    public Image slot4;  

    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4; 

    private GameObject ui1;
    private GameObject ui2;
    private GameObject ui3;
    private GameObject ui4;   

    void Start()
    {
        CreateSlot(slot1, prefab1, ref ui1);
        CreateSlot(slot2, prefab2, ref ui2);
        CreateSlot(slot3, prefab3, ref ui3);
        CreateSlot(slot4, prefab4, ref ui4); 
    }

    void CreateSlot(Image slot, GameObject prefab, ref GameObject uiObj)
    {
        if (slot == null || prefab == null) return;

        if (uiObj != null)
            Destroy(uiObj);

        SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogWarning(prefab.name + " 에 SpriteRenderer가 없습니다!");
            return;
        }

        uiObj = new GameObject(prefab.name + "_UI", typeof(RectTransform), typeof(Image));
        uiObj.transform.SetParent(slot.transform, false);

        Image img = uiObj.GetComponent<Image>();
        img.sprite = sr.sprite;
        img.preserveAspect = true;

        RectTransform rt = uiObj.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(120, 120);
    }
}
