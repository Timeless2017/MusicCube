using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LitJson;

public class SimpleScrollFlow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float imageWidth;
    public float imageHeight;

    public List<SimpleScrollFlowItem> itemList = new List<SimpleScrollFlowItem>();
    public int ItemCount
    {
        get { return itemList.Count; }
    }

    public int currentIndex;
    public int targetIndex;
    [HideInInspector]

    private bool isDrag = false;
    public bool IsDrag
    {
        get { return IsDrag; }
    }
    //拖拽导致切换图片的距离
    public float minDragToMove;

    private bool useNetTexture;
    public bool UseNetTexture
    {
        get { return useNetTexture; }
    }

    public bool Loop;
    public bool isAutoScroll;

    public float imageMoveTime = 0.5f;
    public float autoScrollIntervalTime;
    private float autoScrollTime = 0;
    [HideInInspector]
    public float currentItemOnDragPos;
    [HideInInspector]
    public float currentItemOffsetPos;  

    #region 拖拽

    [HideInInspector]
    public Vector2 startDragPoint, dragRelativeOffset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        currentItemOnDragPos = itemList[currentIndex].Root.transform.localPosition.x;
        startDragPoint = eventData.position;
        foreach (SimpleScrollFlowItem item in itemList)
        {
            item.posXOnDragStart = item.Root.transform.localPosition.x;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        dragRelativeOffset = eventData.position - startDragPoint;
        float targetX = dragRelativeOffset.x;
        foreach (SimpleScrollFlowItem item in itemList)
            item.SetDragPosition(targetX);
        targetIndex = (int)Mathf.Repeat(targetIndex, ItemCount);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        if (dragRelativeOffset.x > minDragToMove)
            ChangeTargetIndex(false);
        else if (dragRelativeOffset.x < -minDragToMove)
            ChangeTargetIndex(true);
    }

    #endregion

    private void ChangeTargetIndex(bool add)
    {
        if (add)
        {
            if (Loop || targetIndex != itemList.Count - 1)
                targetIndex++;
        }
        else
        {
            if (Loop || targetIndex != 0)
                targetIndex--;
        }
        targetIndex = (int)Mathf.Repeat(targetIndex, ItemCount);
    }

    private void ChangeCurrentIndex(bool add)
    {
        if (add)
        {
            if (Loop || currentIndex != itemList.Count - 1)
                currentIndex++;
        }
        else
        {
            if (Loop || currentIndex != 0)
                currentIndex--;
        }
        currentIndex = (int)Mathf.Repeat(currentIndex, ItemCount);
    }

    void Start()
    {
        currentIndex = 0;
        targetIndex = 0;
        ReadJsonAndInit();
        SetItemsActive();
        SetItemsInitPos();
    }


    void Update()
    {
        if (itemList.Count == 0)
            return;
        Loop = (Loop && itemList.Count >= 3);
        currentItemOffsetPos = itemList[currentIndex].Root.transform.localPosition.x;
        SetItemsActive();
        SetItemsScale();
        if (!isDrag)
        {
            SetItemsPos();
            UpdateAutoScroll();
            if (itemList[currentIndex].Root.transform.localPosition.x > imageWidth / 2)
                ChangeCurrentIndex(false);
            else if(itemList[currentIndex].Root.transform.localPosition.x < -imageWidth / 2)
                ChangeCurrentIndex(true);
        }
    }

    /// <summary>
    /// 每隔一段时间自动切换Image
    /// </summary>
    private void UpdateAutoScroll()
    {
        isAutoScroll = (Loop && isAutoScroll);
        if (!isAutoScroll)
            return;
        autoScrollTime += Time.deltaTime;
        if(autoScrollTime >= autoScrollIntervalTime)
        {
            ChangeTargetIndex(true);
            autoScrollTime = 0;
        }
    }

    /// <summary>
    /// 解析Json配置文件并初始化
    /// </summary>
    private void ReadJsonAndInit()
    {
        //设置宽高
        imageWidth = Screen.width;
        imageHeight = Screen.height / 5;

        string jsonText = Resources.Load<TextAsset>("Datas/scrollItem").text;
        JsonData jsonData = JsonMapper.ToObject(jsonText);
        minDragToMove = imageWidth / 4;
        //配置属性(还有什么需要配置的写在Json文件里然后改这里就可以)
        Loop = bool.Parse((string)jsonData["loop"]);
        isAutoScroll = bool.Parse((string)jsonData["isAutoScroll"]);
        imageMoveTime = (float)(double)jsonData["imageMoveTime"];
        autoScrollIntervalTime = (float)(double)jsonData["autoScrollIntervalTime"];
        useNetTexture = bool.Parse((string)jsonData["useNetTexture"]);

        if (useNetTexture)
            return;
        //解析spritePathArray
        JsonData spritePathArrayData = jsonData["spritePathArray"];
        if (spritePathArrayData.IsArray)
        {
            int index = 0;
            foreach (JsonData spritePathData in spritePathArrayData)
            {
                string spritePath = (string)spritePathData;
                Sprite sprite = Resources.Load<Sprite>(spritePath);
                CreateItem(sprite, index);
                index++;
            }
        }
    }

    public void CreateItem(Sprite sprite, int index)
    {
        //创建物体，添加Image组件，设置sprite，设置父物体，设置位置，添加到itemList里
        GameObject item = new GameObject("ScrollFlowItem");
        Image image = item.AddComponent<Image>();
        RectTransform rect = item.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(imageWidth, imageHeight);
        image.sprite = sprite;
        item.transform.SetParent(gameObject.transform, false);
        itemList.Add(new SimpleScrollFlowItem(item, this, index));
    }

    /// <summary>
    /// 设置Items的显示与隐藏
    /// </summary>
    private void SetItemsActive()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].SetActive();
        }
    }

    /// <summary>
    /// 设置Items的初始位置，开始时调用
    /// </summary>
    private void SetItemsInitPos()
    {
        foreach (SimpleScrollFlowItem item in itemList)
        {
            item.SetPositionImmediately();
        }
    }

    private void SetItemsPos()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (!itemList[i].Root.activeSelf)
                continue;
            itemList[i].SetPosition();
        }
    }

    public void SetTargetIndex(int target)
    {
        if (target >= ItemCount || target < 0)
        {
            Debug.LogError("target超出索引");
            return;
        }
        this.targetIndex = target;
    }

    public void SetItemsScale()
    {
        foreach (SimpleScrollFlowItem item in itemList)
        {
            if (!item.Root.activeSelf)
                continue;
            item.rect.sizeDelta = new Vector2(imageWidth, imageHeight);
        }
    }
}
