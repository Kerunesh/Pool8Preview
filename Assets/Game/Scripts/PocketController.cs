using System.Collections;
using UnityEngine;

public class PocketController : MonoBehaviour
{
    [SerializeField] private float sinkDuration = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            StartCoroutine(SinkBallRoutine(other.gameObject));
        }
    }

    private IEnumerator SinkBallRoutine(GameObject ball)
    {
        if (ball.GetComponent<Rigidbody>() != null)
        {
            ball.GetComponent<Collider>().enabled = false;
        }

        if (ball.GetComponent<Rigidbody>() != null)
        {
            Destroy(ball.GetComponent<Rigidbody>());
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
