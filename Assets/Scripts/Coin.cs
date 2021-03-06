﻿using UnityEngine;

public class Coin : MonoBehaviour
{
    public enum CoinState { Idle, Selected, Accepted, Rejected }

    private bool judged = false;
    private Rigidbody2D rigidBody;

    public CoinConfig Config;
    public CoinState coinState = CoinState.Idle;
    public float MoveSpeed = 50f;
    public float MoveClamp = 60f;
    public float CoinDropSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        ApplyConfig();
        rigidBody = GetComponent<Rigidbody2D>();
        gameObject.SendMessageUpwards("CoinAdded");
    }

    // Update is called once per frame
    void Update()
    {
        // this is kinda ugly
        switch(coinState) {
            case CoinState.Selected:
                if(!judged) {
                    var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    var targetDirection = targetPos - transform.position;
                    var force = Vector2.ClampMagnitude(targetDirection * MoveSpeed, MoveClamp);
                    rigidBody.velocity = force;
                }
                else {
                    rigidBody.velocity = Vector2.zero;
                }
                break;
            case CoinState.Accepted:
                transform.Translate(new Vector2(CoinDropSpeed, 0) * Time.deltaTime, Space.World);
                break;
            case CoinState.Rejected:
                transform.Translate(new Vector2(-CoinDropSpeed, 0) * Time.deltaTime, Space.World);
                break;
            default:
                break;
        }
    }

    public bool Select() {
        bool selected = false;
        if(coinState == CoinState.Idle) {
            coinState = CoinState.Selected;
            selected = true;
        }
        return selected;
    }

    public void Drop() {
        if(coinState == CoinState.Selected) {
            coinState = CoinState.Idle;
        }
    }

    public void ApplyConfig() {
        var renderer = GetComponent<SpriteRenderer>();
        renderer.sprite =  Config.Sprite;
        var collider = GetComponent<CircleCollider2D>();
        var extents = renderer.sprite.bounds.extents;
        collider.radius = extents.x;
    }

    private void OnTriggerExit2D() {
        if(judged) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(!judged && coinState == CoinState.Idle) {
            judged = true;
            if(collision.gameObject.name == "AcceptZone") {
                coinState = CoinState.Accepted;
                gameObject.SendMessageUpwards("CoinAccepted", Config);
            }
            else {
                coinState = CoinState.Rejected;
                gameObject.SendMessageUpwards("CoinRejected", Config);
            }
        }
    }
}
