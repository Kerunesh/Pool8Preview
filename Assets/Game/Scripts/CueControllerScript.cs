using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Security;

public class CueControllerScript : MonoBehaviour
{
    public Rigidbody cueBallRigidbody;
    public GameObject cueGraphics;

    [SerializeField] float minStrikeForce = 1.0f;
    [SerializeField] float maxStrikeForce = 10f;
    [SerializeField] float forceChangeSpeed = 5f;
    [SerializeField] float maxPullBackDistance = 4f;
    [SerializeField] private float ballStopThreshold = 0.01f;

    private float currentStrikeForce;
    private Coroutine chargeShotCoroutine;
    private bool isAiming = true;


    private List<Rigidbody> allBallsRigidbodies;

    // Start is called before the first frame update
    void Start()
    {
       allBallsRigidbodies = FindObjectsOfType<Rigidbody>().ToList();
        SetCueVisibility(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAiming)
        {
            Aim();

            if (Input.GetMouseButtonDown(0))
            {
                chargeShotCoroutine = StartCoroutine(ChargeShotCoroutine());
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (chargeShotCoroutine != null)
                {
                    StopCoroutine(chargeShotCoroutine);
                    Strike();
                }
            }
        }
                
    }

    void Aim()
    {
        Vector3 cueBallScreenPos = Camera.main.WorldToScreenPoint(cueBallRigidbody.transform.position);
        Vector3 mousePos = Input.mousePosition;
        Vector2 direction = new Vector2(mousePos.x - cueBallScreenPos.x, mousePos.y - cueBallScreenPos.y);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
    }
    

        void Strike()
    {
        isAiming = false;
        SetCueVisibility(false);

        cueGraphics.transform.localPosition = Vector3.zero;

        var strikeDirection = -transform.right;
        cueBallRigidbody.AddForce(strikeDirection * currentStrikeForce, ForceMode.Impulse);

        StartCoroutine(CheckIfBallStoppedRoutine());
    }

    private IEnumerator ChargeShotCoroutine()
    {
        float chargeTime = 0f;

        while (true)
        {
            float pingPongValue = Mathf.PingPong(chargeTime * forceChangeSpeed, 1f);

            currentStrikeForce = Mathf.Lerp(minStrikeForce, maxStrikeForce, pingPongValue);

            float pullBackValue = Mathf.Lerp(0, maxPullBackDistance, pingPongValue);

            Vector3 cuePosition = cueGraphics.transform.localPosition;
            cuePosition.x = pullBackValue;
            cueGraphics.transform.localPosition = cuePosition;

            chargeTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator CheckIfBallStoppedRoutine()
    {
        yield return new WaitForFixedUpdate();

        while (true)
        {
            bool allStopped = true;
            foreach (Rigidbody ballRB in allBallsRigidbodies)
            {
                if (ballRB != null)
                {
                    if (ballRB.velocity.magnitude > ballStopThreshold || ballRB.angularVelocity.magnitude > ballStopThreshold)
                    {
                        allStopped = false;
                        break;
                    }
                }

            }

            if (allStopped)
            {
                break;
            }

            yield return new WaitForFixedUpdate();
        }

        Debug.Log("All balls are stopped. You can strike now.");
        isAiming = true;
        SetCueVisibility(true);

    }

    void SetCueVisibility(bool isVisible)
    {
        if (cueGraphics != null)
        {
            cueGraphics.SetActive(isVisible);
        }
        
    } 
}
