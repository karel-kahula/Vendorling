using UnityEngine;

public class Coin : MonoBehaviour
{
    private enum CoinState { Active, Accepted, Rejected }

    private Vector2 offset;
    private bool judged = false;
    private CoinState coinState = CoinState.Active;

    public CoinConfig Config;
    public float moveSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        ApplyConfig();
        gameObject.SendMessageUpwards("CoinAdded");
    }

    // Update is called once per frame
    void Update()
    {
        // this is kinda ugly
        switch(coinState) {
            case CoinState.Accepted:
                transform.Translate(new Vector2(5f, transform.position.y) * Time.deltaTime);
                break;
            case CoinState.Rejected:
                transform.Translate(new Vector2(-5f, transform.position.y) * Time.deltaTime);
                break;
            default:
                break;
        }
        
    }

    public void ApplyConfig() {
        var renderer = GetComponent<SpriteRenderer>();
        renderer.sprite =  Config.Sprite;
        var collider = GetComponent<CircleCollider2D>();
        var extents = renderer.sprite.bounds.extents;
        collider.radius = extents.x;
    }

    private void OnMouseDown() {
        if(!judged) {
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseDrag() {
        if(!judged) {
            var screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(screenPos.x, screenPos.y) + offset;
        }
    }

    private void OnTriggerExit2D() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
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
