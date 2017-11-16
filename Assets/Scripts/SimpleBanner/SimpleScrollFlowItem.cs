using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleScrollFlowItem
{

    private GameObject _root;
    public GameObject Root
    {
        get { return _root; }
    }
    public Image image;
    public RectTransform rect;

    public int index;
    public float posXOnDragStart;


    private SimpleScrollFlow controller;

    public Sprite Sprite
    {
        get { return image.sprite; }
        set { image.sprite = value; }
    }

    public SimpleScrollFlowItem(GameObject root, SimpleScrollFlow controller, int index)
    {
        this._root = root;
        image = _root.GetComponent<Image>();
        this.controller = controller;
        this.index = index;
        rect = _root.GetComponent<RectTransform>();
    }


    /// <summary>
    /// 根据距离currentIndex多少显示或隐藏
    /// </summary>
    public void SetActive()
    {
        //这里需要根据多个条件判断，写的不是很好但想不出怎么优化
        if (controller.Loop)
        {
            if (controller.currentItemOffsetPos > 0)
            {
                if ((index == Mathf.Repeat(controller.currentIndex - 1, controller.ItemCount) || index == controller.currentIndex) 
                    && !_root.activeSelf)
                {
                    _root.SetActive(true);
                    SetPositionImmediately();
                }
                else if (index != Mathf.Repeat(controller.currentIndex - 1, controller.ItemCount) && index != controller.currentIndex
                    && _root.activeSelf)
                    _root.SetActive(false);
            }
            else if (controller.currentItemOffsetPos < 0)
            {
                if ((index == Mathf.Repeat(controller.currentIndex + 1, controller.ItemCount) || index == controller.currentIndex) 
                    && !_root.activeSelf)
                {
                    _root.SetActive(true);
                    SetPositionImmediately();
                }
                else if(index != Mathf.Repeat(controller.currentIndex + 1, controller.ItemCount) && index != controller.currentIndex
                    && _root.activeSelf)
                    _root.SetActive(false);
            }
            else
            {
                if (index == controller.currentIndex && !_root.activeSelf)
                {
                    _root.SetActive(true);
                    SetPositionImmediately();
                }
                else if (index != controller.currentIndex && _root.activeSelf)
                    _root.SetActive(false);
            }
        }
        else
        {
            if (controller.currentItemOffsetPos > 0)
            {
                if ((index == controller.currentIndex - 1 || index == controller.currentIndex) && !_root.activeSelf)
                {
                    _root.SetActive(true);
                    SetPositionImmediately();
                }
                else if (index != controller.currentIndex - 1 && index != controller.currentIndex && _root.activeSelf)
                    _root.SetActive(false);
            }
            else if (controller.currentItemOffsetPos < 0)
            {
                if ((index == controller.currentIndex + 1 || index == controller.currentIndex) && !_root.activeSelf)
                {
                    _root.SetActive(true);
                    SetPositionImmediately();
                }
                else if(index != controller.currentIndex + 1 && index != controller.currentIndex && _root.activeSelf)
                    _root.SetActive(false);
            }
            else
            {
                if (index == controller.currentIndex && !_root.activeSelf)
                {
                    _root.SetActive(true);
                    SetPositionImmediately();
                }
                else if (index != controller.currentIndex && _root.activeSelf)
                    _root.SetActive(false);
            }
        }
    }

    #region 三种设置位置方式

    /// <summary>
    /// 根据index求应在的位置并缓动过去
    /// </summary>
    public void SetPosition()
    {
        if (!_root.activeSelf)
            return;
        float targetPosX = (index - controller.targetIndex) * controller.imageWidth;
        targetPosX = CalMinDistancePos(targetPosX);
        float moveDistancePerSecond = controller.imageWidth / controller.imageMoveTime;
        if (Mathf.Abs(_root.transform.localPosition.x - targetPosX) > moveDistancePerSecond * Time.deltaTime)
        {
            if (_root.transform.localPosition.x - targetPosX > 0)
                _root.transform.Translate(-moveDistancePerSecond * Time.deltaTime, 0, 0);
            else
                _root.transform.Translate(moveDistancePerSecond * Time.deltaTime, 0, 0);
        }
        else
            _root.transform.localPosition = new Vector3(targetPosX, -controller.imageHeight / 2, 0);
    }

    /// <summary>
    /// 立即移动到目标位置
    /// </summary>
    /// <param name="targetPosX"></param>
    public void SetPositionImmediately()
    {
        if (!_root.activeSelf)
            return;
        float targetPosX = controller.currentItemOffsetPos + (index - controller.currentIndex) * controller.imageWidth;

        targetPosX = CalMinDistancePos(targetPosX);
        _root.transform.localPosition = new Vector3(targetPosX, -controller.imageHeight / 2, 0);
        this.posXOnDragStart = targetPosX - controller.dragRelativeOffset.x;
    }



    /// <summary>
    /// 设置拖拽时的位置
    /// </summary>
    /// <param name="xAxisDragRelativeOffset"></param>
    public void SetDragPosition(float xAxisDragRelativeOffset)
    {
        if (!_root.activeSelf)
            return;
        float targetPosX = xAxisDragRelativeOffset + posXOnDragStart;
        targetPosX = CalMinDistancePos(targetPosX);
        _root.transform.localPosition = new Vector3(targetPosX, -controller.imageHeight / 2, 0);
        if (Mathf.Abs(_root.transform.localPosition.x) < controller.imageWidth / 2)
            controller.currentIndex = index;
    }

    #endregion

    /// <summary>
    /// 计算并返回距离currentImage最近的位置
    /// </summary>
    /// <param name="targetPosX"></param>
    /// <returns></returns>
    private float CalMinDistancePos(float targetPosX)
    {
        if (targetPosX > (Screen.width + controller.imageWidth))
            targetPosX -= controller.imageWidth * controller.ItemCount;
        if (targetPosX < -(Screen.width + controller.imageWidth))
            targetPosX += controller.imageWidth * controller.ItemCount;
        return targetPosX;
    }


}
