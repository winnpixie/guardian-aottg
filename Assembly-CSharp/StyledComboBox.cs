using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class StyledComboBox : StyledItem
{
    public delegate void SelectionChangedHandler(StyledItem item);

    public SelectionChangedHandler OnSelectionChanged;
    public StyledComboBoxPrefab containerPrefab;
    public StyledItem itemPrefab;
    public StyledItem itemMenuPrefab;
    [HideInInspector]
    [SerializeField]
    private StyledComboBoxPrefab root;
    [HideInInspector]
    [SerializeField]
    private List<StyledItem> items = new List<StyledItem>();
    [SerializeField]
    private int selectedIndex;
    private bool isToggled;

    public int SelectedIndex
    {
        get
        {
            return selectedIndex;
        }
        set
        {
            if (value >= 0 && value <= items.Count)
            {
                selectedIndex = value;
                CreateMenuButton(items[selectedIndex].GetText().text);
            }
        }
    }

    public StyledItem SelectedItem
    {
        get
        {
            if (selectedIndex >= 0 && selectedIndex <= items.Count)
            {
                return items[selectedIndex];
            }
            return null;
        }
    }

    private void Awake()
    {
        InitControl();
    }

    private void AddItem(object data)
    {
        if (itemPrefab != null)
        {
            Vector3[] array = new Vector3[4];
            itemPrefab.GetComponent<RectTransform>().GetLocalCorners(array);
            Vector3 position = array[0];
            float num = position.y - array[2].y;
            position.y = (float)items.Count * num;
            StyledItem styledItem = Object.Instantiate(itemPrefab, position, root.itemRoot.rotation) as StyledItem;
            RectTransform component = styledItem.GetComponent<RectTransform>();
            styledItem.Populate(data);
            component.SetParent(root.itemRoot.transform, worldPositionStays: false);
            component.pivot = new Vector2(0f, 1f);
            component.anchorMin = new Vector2(0f, 1f);
            component.anchorMax = Vector2.one;
            component.anchoredPosition = new Vector2(0f, position.y);
            items.Add(styledItem);
            component.offsetMin = new Vector2(0f, position.y + num);
            component.offsetMax = new Vector2(0f, position.y);
            RectTransform itemRoot = root.itemRoot;
            Vector2 offsetMin = root.itemRoot.offsetMin;
            itemRoot.offsetMin = new Vector2(offsetMin.x, (float)(items.Count + 2) * num);
            Button button = styledItem.GetButton();
            int curIndex = items.Count - 1;
            if (button != null)
            {
                button.onClick.AddListener(delegate
                {
                    OnItemClicked(styledItem, curIndex);
                });
            }
        }
    }

    public void OnItemClicked(StyledItem item, int index)
    {
        SelectedIndex = index;
        TogglePanelState();
        if (OnSelectionChanged != null)
        {
            OnSelectionChanged(item);
        }
    }

    public void ClearItems()
    {
        for (int num = items.Count - 1; num >= 0; num--)
        {
            Object.DestroyObject(items[num].gameObject);
        }
    }

    public void AddItems(params object[] list)
    {
        ClearItems();
        for (int i = 0; i < list.Length; i++)
        {
            AddItem(list[i]);
        }
        SelectedIndex = 0;
    }

    public void InitControl()
    {
        if (root != null)
        {
            Object.DestroyImmediate(root.gameObject);
        }
        if (containerPrefab != null)
        {
            RectTransform component = GetComponent<RectTransform>();
            root = (Object.Instantiate(containerPrefab, component.position, component.rotation) as StyledComboBoxPrefab);
            root.transform.SetParent(base.transform, worldPositionStays: false);
            RectTransform component2 = root.GetComponent<RectTransform>();
            component2.pivot = new Vector2(0.5f, 0.5f);
            component2.anchorMin = Vector2.zero;
            component2.anchorMax = Vector2.one;
            component2.offsetMax = Vector2.zero;
            component2.offsetMin = Vector2.zero;
            root.gameObject.hideFlags = HideFlags.HideInHierarchy;
            root.itemPanel.gameObject.SetActive(isToggled);
        }
    }

    private void CreateMenuButton(object data)
    {
        if (root.menuItem.transform.childCount > 0)
        {
            for (int num = root.menuItem.transform.childCount - 1; num >= 0; num--)
            {
                Object.DestroyObject(root.menuItem.transform.GetChild(num).gameObject);
            }
        }
        if (itemMenuPrefab != null && root.menuItem != null)
        {
            StyledItem styledItem = Object.Instantiate(itemMenuPrefab) as StyledItem;
            styledItem.Populate(data);
            styledItem.transform.SetParent(root.menuItem.transform, worldPositionStays: false);
            RectTransform component = styledItem.GetComponent<RectTransform>();
            component.pivot = new Vector2(0.5f, 0.5f);
            component.anchorMin = Vector2.zero;
            component.anchorMax = Vector2.one;
            component.offsetMin = Vector2.zero;
            component.offsetMax = Vector2.zero;
            root.gameObject.hideFlags = HideFlags.HideInHierarchy;
            Button button = styledItem.GetButton();
            if (button != null)
            {
                button.onClick.AddListener(TogglePanelState);
            }
        }
    }

    public void TogglePanelState()
    {
        isToggled = !isToggled;
        root.itemPanel.gameObject.SetActive(isToggled);
    }
}
