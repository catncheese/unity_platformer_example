using UnityEngine;

public class CoinController : MonoBehaviour
{
    public const string PLAYER_TAG = "Player";

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.gameObject.tag == PLAYER_TAG)
        {
            CharacterController controller = otherCollider.gameObject.GetComponent<CharacterController>();
            controller?.IncrementScore();

            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        // Do Nothing
    }

    private void OnTriggerStay2D(Collider2D otherCollider)
    {
        // Do Nothing
    }
}
