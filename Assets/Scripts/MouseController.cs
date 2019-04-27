using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
    private GameObject selectedCoin;
    private Vector2 mouseDown;
    private Vector2 mouseUp;
    private DateTime mouseDownTime;
    private DateTime mouseUpTime;

    [Range(0,1)]
    public float swipeDistanceMin = 0.3f;
    [Range(0,2)]
    public float swipeTimeMax = 1f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        var cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) {
            mouseDown = new Vector2(cursorPos.x, cursorPos.y);
            mouseDownTime = DateTime.UtcNow;
            var hit = Physics2D.Raycast(mouseDown, Vector2.zero, 10f);
            selectedCoin = hit ? hit.transform.gameObject : null;
        }
        if (Input.GetMouseButtonUp(0)) {
            if(selectedCoin) {
                mouseUp = new Vector2(cursorPos.x, cursorPos.y);
                mouseUpTime = DateTime.UtcNow;
                var swipeDeltaX = mouseUp.x - mouseDown.x;
                var swipeDeltaTime = (float)mouseUpTime.Subtract(mouseDownTime).TotalSeconds;   

                if(swipeTimeMax > swipeDeltaTime && swipeDistanceMin < Mathf.Abs(swipeDeltaX)) {
                    if(swipeDeltaX > 0) {
                        Debug.Log($"right {swipeDeltaX}");
                    }
                    else {
                        Debug.Log($"left {swipeDeltaX}");
                    }
                    selectedCoin = null;
                }
            }
        }
    }
}
