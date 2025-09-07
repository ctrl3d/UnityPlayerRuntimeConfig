using System.Collections.Generic;
using UnityEngine;
using work.ctrl3d;

public class GameManager : MonoBehaviour
{
    private List<DisplayInfo> _displayInfoList;

    private void Start()
    {
        UnityPlayerRuntimeConfig.Apply();
        
        _displayInfoList = new List<DisplayInfo>();
        Screen.GetDisplayLayout(_displayInfoList);

        foreach (var displayInfo in _displayInfoList)
        {
            Debug.Log(displayInfo.name);
            Debug.Log(displayInfo.width);
            Debug.Log(displayInfo.height);
            Debug.Log(displayInfo.refreshRate);
            Debug.Log(displayInfo.workArea);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("!!");
            Screen.MoveMainWindowTo(_displayInfoList[0], _displayInfoList[0].workArea.position);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("@@");
            Screen.MoveMainWindowTo(_displayInfoList[1], _displayInfoList[1].workArea.position);
        }
    }
}