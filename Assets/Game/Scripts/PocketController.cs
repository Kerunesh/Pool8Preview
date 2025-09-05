using System.Collections;
using UnityEngine;

public class PocketController : MonoBehaviour
{
    [SerializeField] private float sinkDuration = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            UIController.Instance.AddScore(1);

            StartCoroutine(SinkBallRoutine(other.gameObject));
        }
    }

    private IEnumerator SinkBallRoutine(GameObject ball)
    {
        Collider ballCollider = ball.GetComponent<Collider>();
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();

        if (ballCollider != null)
        {
           ballCollider.enabled = false;
        }

        if (ballRigidbody != null)
        {
            Destroy(ballRigidbody);
        }
        
        

        Vector3 initialScale = ball.transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime < sinkDuration)
        {
            ball.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / sinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(ball);
    }

}
