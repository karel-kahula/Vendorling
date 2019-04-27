using UnityEngine;

public class Coin : MonoBehaviour
{
    private enum CoinState { Active, Accepted, Rejected }

    private Vector2 offset;
    private bool judged = false;
    private CoinState coinState = CoinState.Active;

    public float moveSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var exit = new Vector2(5f, transform.position.y);

        switch(coinState) {
            case CoinState.Accepted:
                transform.Translate(exit * Time.deltaTime);
                break;
            case CoinState.Rejected:
            default:
                break;
        }
        
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

    private void OnTriggerExit2D() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        judged = true;
        if(collision.gameObject.name == "AcceptZone") {
            coinState = CoinState.Accepted;
        }
        else {
            coinState = CoinState.Rejected;
        }
    }
}
