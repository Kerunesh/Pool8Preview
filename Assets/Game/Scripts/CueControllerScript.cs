using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CueControllerScript : MonoBehaviour
{
    public Rigidbody cueBallRigidbody;
    public GameObject cueGraphics;

    [SerializeField] float strikeForce = 10f;
    
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
        if (cueGraphics.activeSelf)
        {
            Aim();

            if (Input.GetMouseButtonDown(0))
            {
                Strike();
            }
        }
                
    }

    void Aim()
    {
        Vector3 cueBallScreenPos = Camera.main.WorldToScreenPoint(cueBallRigidbody.transform.position);
        Vector3 mousePos = Input.mousePosition;
        Vector2 direction = new Vector2 (mousePos.x - cueBallScreenPos.x, mousePos.y - cueBallScreenPos.y);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
    }

        void Strike()
    {
        SetCueVisibility(false);

        var strikeDirection = -transform.right;

        cueBallRigidbody.AddForce(strikeDirection * strikeForce, ForceMode.Impulse);

        StartCoroutine(CheckIfBallStoppedRoutine());
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
                    if (ballRB.velocity.magnitude > 0.01f || ballRB.angularVelocity.magnitude > 0.01f)
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

        Debug.Log("Все шары остановились. Можно бить.");
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
