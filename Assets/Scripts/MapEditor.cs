﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapEditor : MonoBehaviour
{
    public Color[] colors;
    public MapGrid hexGrid;
    private Color activeColor;

    private void Awake()
    {
        SelectColor(0);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)&&!EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(inputRay,out hit))
        {
            //hexGrid.ColorCell(hit.point, activeColor);
        }
    }

    public void SelectColor(int index)
    {
        activeColor = colors[index];
    }
}
