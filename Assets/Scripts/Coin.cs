using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Vector2 offset;
    private bool judged = false;

    public float moveSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() {
        if(!judged) {
            var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseDrag() {
        if(!judged) {
            var screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(screenPos.x, screenPos.y) + offset;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        judged = true;
        if(collision.gameObject.name == "AcceptZone") {
            StartCoroutine("AcceptCoin");
        }
        else {
            //StartCoroutine("RejectCoin");
        }
    }

    private IEnumerator AcceptCoin() {
        Debug.Log("AcceptCoin");
        bool exited = false;
        var exit = new Vector2(5f, transform.position.y);

        while (!exited) {
            transform.position = Vector2.MoveTowards(transform.position, exit, moveSpeed);
            exited = Vector2.Distance(transform.position, exit) == 0;
            yield return new WaitForSeconds(0.1f);
        }

        if(exited) {
            Destroy(gameObject);
        }
    }
}
