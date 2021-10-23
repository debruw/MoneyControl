using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    bool ismouseDowned;
    public Transform finishLine;
    public Slider slider;
    public Image sliderImage;
    float maxDistance;

    private void Start()
    {
        maxDistance = Vector3.Distance(transform.position, finishLine.transform.position);
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            splineFollower.followSpeed = 0;
            playerAnimator.SetBool("isRunning", false);
            return;
        }
        count += Time.deltaTime;
        //#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            ismouseDowned = true;
            splineFollower.followSpeed = zSpeed;
            playerAnimator.SetBool("isRunning", true);
        }
        if (Input.GetMouseButton(0) && ismouseDowned)
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
        if (Input.GetMouseButtonUp(0) && ismouseDowned)
        {
            ismouseDowned = false;
            splineFollower.followSpeed = 0;
            playerAnimator.SetBool("isRunning", false);
        }

        slider.value = 1 - Vector3.Distance(transform.position, finishLine.transform.position) / maxDistance;
        sliderImage.fillAmount = 1 - Vector3.Distance(transform.position, finishLine.transform.position) / maxDistance;
        //elif UNITY_IOS || UNITY_ANDROID

        //if (Input.touchCount > 0)
        //{
        //    Debug.Log(Input.GetAxis("Mouse X"));
        //    touch = Input.GetTouch(0);
        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        splineFollower.followSpeed = zSpeed/1.5f;
        //        playerAnimator.SetBool("isRunning", true);
        //    }
        //    if (touch.phase == TouchPhase.Moved)
        //    {
        //        move = controller.transform.right * Input.GetAxis("Mouse X") * xSpeed;

        //        if (move.magnitude != 0 && count > 1)
        //        {
        //            CreateParticle(CollisionManager.Instance.MoneyPiles[CollisionManager.Instance.MoneyPiles.Count - 1].transform.position);
        //            count = 0;
        //        }

        //        controller.Move(move * Time.deltaTime);
        //        controller.transform.localPosition = new Vector3(Mathf.Clamp(controller.transform.localPosition.x, -3.4f, 3.4f), 0, 0);
        //    }
        //    if (touch.phase == TouchPhase.Ended)
        //    {
        //        splineFollower.followSpeed = 0;
        //        playerAnimator.SetBool("isRunning", false);
        //    }

        //}
        //#endif
    }

    public void CreateParticle(Vector3 pos)
    {
        Instantiate(moneyParticle, pos, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            splineFollower.followSpeed = 0;
            playerAnimator.SetBool("isRunning", false);
            GameManager.Instance.isGameOver = true;
            StartCoroutine(CollisionManager.Instance.MoveMoneysToPiggy());
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
