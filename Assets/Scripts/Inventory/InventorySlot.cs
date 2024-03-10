using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image m_icon;

    [SerializeField] private TextMeshProUGUI m_label;

    [SerializeField] private GameObject m_stackObj;

    [SerializeField] private TextMeshProUGUI m_stackLabel;
    // Start is called before the first frame update
    public void Set(InventoryItem item)
    {
        m_icon.sprite = item.data.icon;
        m_label.text = item.data.displayName;
        if (item.stackSize <= 1)
        {
            m_stackObj.SetActive(false);
            return;
        }

        m_stackLabel.text = item.stackSize.ToString();
    }
    void Awake()
    {
        m_icon = gameObject.transform.GetChild(0).GetComponent<Image>();
        m_label = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        m_stackObj = gameObject.transform.GetChild(2).gameObject;
        m_stackLabel = m_stackObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
