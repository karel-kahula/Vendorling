using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Coin;

public class MouseController : MonoBehaviour {
    private Coin selectedCoin;
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
            if(hit && hit.transform.tag == "coin") {
                selectedCoin = hit.transform.gameObject.GetComponent<Coin>();
                selectedCoin = selectedCoin.Select() ? selectedCoin : null;
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            if(selectedCoin != null && selectedCoin.coinState == CoinState.Selected) {
                selectedCoin.Drop();
                selectedCoin = null;
            }
        }
    }
}