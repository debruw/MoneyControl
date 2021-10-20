using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Vector3 move;
    public CharacterController controller;

    private Touch touch;
    public SplineFollower splineFollower;
    public float xSpeed = 15, zSpeed = 10;
    public Animator playerAnimator;
    public GameObject moneyParticle;
    float count;
    bool isGameOver;

    void Update()
    {
        if (isGameOver)
        {
            splineFollower.followSpeed = 0;
            playerAnimator.SetBool("isRunning", false);
            return;
        }
        count += Time.deltaTime;
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            splineFollower.followSpeed = zSpeed;
            playerAnimator.SetBool("isRunning", true);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 move = controller.transform.right * Input.GetAxis("Mouse X") * xSpeed;

            if (move.magnitude != 0 && count > 1 && CollisionManager.Instance.MoneyPiles.Count > 0)
            {
                CreateParticle(CollisionManager.Instance.MoneyPiles[CollisionManager.Instance.MoneyPiles.Count - 1].transform.position);
                count = 0;
            }

            controller.Move(move * Time.deltaTime);
            controller.transform.localPosition = new Vector3(Mathf.Clamp(controller.transform.localPosition.x, -3.4f, 3.4f), 0, 0);
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
                splineFollower.followSpeed = zSpeed/1.5f;
                playerAnimator.SetBool("isRunning", true);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                move = controller.transform.right * Input.GetTouch(0).deltaPosition * xSpeed / 15;

                if (move.magnitude != 0 && count > 1)
                {
                    CreateParticle(CollisionManager.Instance.MoneyPiles[CollisionManager.Instance.MoneyPiles.Count - 1].transform.position);
                    count = 0;
                }

                controller.Move(move * Time.deltaTime);
                controller.transform.localPosition = new Vector3(Mathf.Clamp(controller.transform.localPosition.x, -3.4f, 3.4f), 0, 0);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                splineFollower.followSpeed = 0;
                playerAnimator.SetBool("isRunning", false);
            }

        }
#endif
    }

    public void CreateParticle(Vector3 pos)
    {
        Instantiate(moneyParticle, pos, Quaternion.identity);
    }

    public GameObject RestartButton;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            RestartButton.SetActive(true);
            isGameOver = true;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
