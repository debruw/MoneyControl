using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    Vector3 move;
    public CharacterController controller;

    private Touch touch;
    public SplineFollower splineFollower;
    public float xSpeed = 15, zSpeed = 10;
    public Animator playerAnimator;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            splineFollower.followSpeed = zSpeed;
            Debug.Log(playerAnimator.GetBool("isrunning"));
            playerAnimator.SetBool("isRunning", true);
            Debug.Log(playerAnimator.GetBool("isRunning"));
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 move = controller.transform.right * Input.GetAxis("Mouse X") * xSpeed;

            controller.Move(move * Time.deltaTime);
            controller.transform.localPosition = new Vector3(Mathf.Clamp(controller.transform.localPosition.x, -3.5f, 3.5f), 0, 0);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            splineFollower.followSpeed = 0;
            playerAnimator.SetBool("isRunning", false);
        }

#elif UNITY_IOS || UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                splineFollower.followSpeed = xSpeed;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                move += controller.transform.right * Input.GetAxis("Mouse X") * xSpeed;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                splineFollower.followSpeed = 0;
            }
            controller.Move(move * Time.deltaTime);
            controller.transform.localPosition = new Vector3(Mathf.Clamp(controller.transform.localPosition.x, -3.5f, 3.5f), 0, 0);
        }
#endif
    }
}
