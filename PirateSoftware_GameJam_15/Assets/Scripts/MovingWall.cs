using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
//using XInputDotNetPure;

public class MovingWall : MonoBehaviour
{
    [Header("Travis' Code & James :(")]
    // Travis coded this





    /* PlayerIndex playerIndex;
     GamePadState state;
     GamePadState prevState;*/


    [SerializeField] private GameObject _groundvfx;
    [SerializeField] private GameObject _VFXlocation;
    [SerializeField] private bool IsMovable = true;
    [SerializeField] private bool IsPressed = false;
    [SerializeField] private Transform startMarker;
    [SerializeField] private Transform endMarker;
    [SerializeField] private Transform thirdMarker;

    // Movement speed in units per second.
    [SerializeField] public float moveUpSpeed = 1.0F;
    [SerializeField] public float moveBackSpeed = 1.0F;
    [SerializeField] float totalMoveTime = 1.0f;
    [SerializeField] float lerpPercent = 0.0f;
    [SerializeField] float lastCurLerpTime = 4.0f;
    [SerializeField] public int waitBeforeMovingBack = 0;
    [SerializeField] public bool movesBackAuto;
    [SerializeField] public bool movesBackPress;
    [SerializeField] public bool IsStatue = false;
    [SerializeField] public GameObject Handprints;
    [SerializeField] public float rumbleAmmount = 0.5f;
    [SerializeField] private CinemachineImpulseSource _impulse;


    [SerializeField] private GameObject _secondWall;
    [SerializeField] private GameObject _thirdWall;
    public GameObject LanternDetectionLight;
    public GameObject EnviroCam;

    public UnityEvent ResetButton;




    // [SerializeField] public PlayerIndex player;

    // Time when the movement started.
    [SerializeField] private float startTime;

    // if last rotation is true, move to 0 instead of 2
    [SerializeField] private bool lastRotation;

    // Total distance between the markers.
    private float journeyLength;

    private bool doOnce = true;
    private bool atTop = true;
    // bool playerIndexSet = false;
    private Transform CurrentPosition;
    public Transform EndPosition;
    void Start()
    {

        transform.rotation = startMarker.rotation;
        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);



    }

    private void Update()
    {/*
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }*/
    }
    public void MoveForward()
    {
        if (!IsMovable) { }
        else
          if (doOnce&&atTop)
        {
            doOnce = false;
            IsPressed = true;
            StartCoroutine(Move());
            if (Handprints != null) { (Handprints).SetActive(false); }
        }
        else

          if (movesBackPress&&!atTop)
        {
            doOnce = false;
            IsPressed = false;
            StartCoroutine(MoveBack());
            if (Handprints != null) { (Handprints).SetActive(false); }
        }

    }

    public void Moveback()
    {
        IsPressed = false;
        StartCoroutine(MoveBack());
    }
    public void TruePressed()
    {
        IsPressed = true;
    }
    public void MakeLanternReady()
    {
        IsMovable = true;
    }
    public void MakeLanternNotReady()
    {
        IsMovable = false;
    }
    public void FalsePressed()
    {
        IsPressed = false;
    }
    public void Rotate()
    {

        if (IsStatue)
        {
            if (transform.rotation == startMarker.rotation && lastRotation)
            {
                EndPosition = endMarker;
                if (_secondWall != null && _thirdWall != null)
                {
                    _secondWall.SetActive(true);
                    _thirdWall.SetActive(false);
                }

            }
            else if(transform.rotation == startMarker.rotation && !lastRotation)
            {
                EndPosition = thirdMarker;
                if (_secondWall != null && _thirdWall != null)
                {
                    _secondWall.SetActive(false);
                    _thirdWall.SetActive(true);
                }
                
            }
            else if (transform.rotation == thirdMarker.rotation)
            {
                EndPosition = startMarker;
                lastRotation = true;
                if (_secondWall != null && _thirdWall != null)
                {
                    _secondWall.SetActive(false);
                    _thirdWall.SetActive(false);
                }
            }
            else if (transform.rotation == endMarker.rotation)
            {
                EndPosition = startMarker;
                lastRotation = false;
                if (_secondWall != null && _thirdWall != null)
                {
                    _secondWall.SetActive(false);
                    _thirdWall.SetActive(false);
                }
                
            }

            if (doOnce)
            {
                doOnce = false;
                StartCoroutine(Spin());
                if (Handprints != null) { (Handprints).SetActive(false); }

            }
        }

        //If its not a statue. going to assume its pressureplate. 
        else
        {
            if (IsPressed)
            {
                EndPosition = endMarker;
            }
            else
            {
                EndPosition = startMarker;
            }

            {
                StartCoroutine(Spin());
            }
        }


    }
    /*
    private void RumbleOn()
    {
        GamePad.SetVibration(playerIndex, rumbleAmmount, rumbleAmmount);
    }

    private void RumbleOff()
    {
        GamePad.SetVibration(playerIndex, 0, 0);
    }*/



    private IEnumerator Move()
    {
        yield return new WaitForSeconds(waitBeforeMovingBack);
        StartSFX();
        startTime = Time.time;
        
        float curLerpTime = 0.0f;
        if (lastCurLerpTime < totalMoveTime)
        {
            curLerpTime = Mathf.Abs(lastCurLerpTime - totalMoveTime);
        }
        while (transform.position != endMarker.position && IsPressed)
        {

            curLerpTime += Time.deltaTime;
            
            if(curLerpTime > totalMoveTime)
            {
                curLerpTime = totalMoveTime;
            }
            
            lerpPercent = curLerpTime / totalMoveTime;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, lerpPercent);

            // RumbleOn();
            // Distance moved equals elapsed time times speed..
            //float distCovered = (Time.time - startTime) * moveUpSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            //float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            //transform.position = Vector3.Lerp(transform.position, endMarker.position, fractionOfJourney);

            if (movesBackAuto && transform.position == endMarker.position)
            {
                StartCoroutine(MoveBack());
                // RumbleOff();
                IsPressed=false;
            }
            else

            if (movesBackPress && transform.position == endMarker.position)
            {
                if (Handprints != null) { (Handprints).SetActive(true); }

                doOnce = true;
                atTop = false;
                //  RumbleOff();
            }
            yield return null;

        }
        doOnce = true;
        lastCurLerpTime = curLerpTime;
        StopSFX();
        if (LanternDetectionLight != null) { LanternDetectionLight.SetActive(true); }
        if (EnviroCam != null) { EnviroCam.SetActive(false); }

        yield return null;
    }
    private IEnumerator MoveBack()
    {
        yield return new WaitForSeconds(waitBeforeMovingBack);
        // RumbleOn();
        StartSFX();
        float curLerpTime = 0.0f;
        if(lastCurLerpTime < totalMoveTime)
        {
            curLerpTime = Mathf.Abs(lastCurLerpTime - totalMoveTime);
        }

        startTime = Time.time;
        while (transform.position != startMarker.position && !IsPressed)
        {   
            curLerpTime += Time.deltaTime;

            if (curLerpTime > totalMoveTime)
            {
                curLerpTime = totalMoveTime;
            }

            lerpPercent = curLerpTime / totalMoveTime;

            transform.position = Vector3.Lerp(endMarker.position, startMarker.position, lerpPercent);
            // Distance moved equals elapsed time times speed..
            //float distCovered = (Time.time - startTime) * moveBackSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            //float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            //transform.position = Vector3.Lerp(transform.position, startMarker.position, fractionOfJourney);

            if (transform.position == startMarker.position)
            {
                doOnce = true;
                if (Handprints != null) { (Handprints).SetActive(true); }
                atTop = true;
                // RumbleOff();
            }
            yield return null;

        }
        doOnce = true;
        atTop = true;  
        StopSFX();
        ActivateButton();
        lastCurLerpTime = curLerpTime;
        yield return null;

    }


    private IEnumerator Spin()
    {

        StartSFX();
        startTime = Time.time;
        while (transform.rotation != EndPosition.rotation)
        {

            // RumbleOn();
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * moveUpSpeed;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, EndPosition.rotation, moveUpSpeed * Time.deltaTime);


            if (movesBackPress && transform.rotation == EndPosition.rotation)
            {
                doOnce = true;
                if (Handprints != null) { (Handprints).SetActive(true); }

                //  RumbleOff();
            }
            else
            if (movesBackAuto && transform.rotation == EndPosition.rotation)
            {
                // yield return new WaitForSeconds(waitBeforeMovingBack);
                EndPosition = startMarker;
                StartCoroutine(SpinBack());
                // RumbleOff();
            }
            yield return null;

        }
        StopSFX();

    }

    private IEnumerator SpinBack()
    {
        yield return new WaitForSeconds(waitBeforeMovingBack);
        StartSFX();
        while (transform.rotation != EndPosition.rotation)
        {

            // RumbleOn();
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * moveUpSpeed;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, EndPosition.rotation, moveUpSpeed * Time.deltaTime);
            yield return null;

        }
        StopSFX();

    }
    private void StartSFX()
    {

       
    }
    private void StopSFX()
    {

    }
    private void ActivateButton()
    {
        ResetButton.Invoke();
    }
}
