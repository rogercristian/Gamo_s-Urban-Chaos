using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour,Control.IPlayerActions
{
    public Control control;

    public bool isNight = false;

    public float MaxEnginerForce;
    public float MaxSteeringTorque;
    public GameObject brakeLight;
    public AudioClip[] audioClips;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource mainAudioSource;

    float EnginerForce = 1f;
    float SteeringDirection = 0f;

    private Rigidbody2D rb2D;
    private bool onAccelerate = false;
    private bool onBrake = false;

    private void Awake()
    {
        control = new Control();
        control.Player.SetCallbacks(this);
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(WaitToEngine());
    }
    IEnumerator WaitToEngine()
    {
        yield return new WaitForSeconds(1.5f);
        mainAudioSource.clip = audioClips[0];
        mainAudioSource.Play();
        mainAudioSource.loop = true;
    }
    private void OnEnable()
    {
        control.Player.Enable();
    }

    private void OnDisable()
    {
        control.Player.Disable();
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onAccelerate = true;
            audioSource.clip = audioClips[1];
            audioSource.Play();
            brakeLight.SetActive(false);
           

        }
        if (context.canceled)
        {
            onAccelerate = false;
            audioSource.clip = audioClips[1];
            audioSource.Stop();            

        }

    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onBrake = true;
            brakeLight.SetActive(true);
            audioSource.clip = audioClips[1];
            audioSource.Play();                    

        }
        if (context.canceled)
        {
            onBrake = false;
            brakeLight.SetActive(false);
            audioSource.clip = audioClips[1];
            audioSource.Stop();            

        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        SetSteeringDirection( context.ReadValue<float>());
    }

    private void FixedUpdate()
    {
        if (onAccelerate)
        {
            rb2D.AddForce(transform.up * EnginerForce * MaxEnginerForce, ForceMode2D.Force);           
        }

        if (onBrake)
        {
             rb2D.AddForce(transform.up * -EnginerForce * MaxEnginerForce, ForceMode2D.Force);           
        }
        ApplySteeringForce();
    }

   
    public void ApplySteeringForce()
    {
        rb2D.AddTorque(SteeringDirection * MaxSteeringTorque, ForceMode2D.Force);
    }
    public void SetSteeringDirection(float steeringDirection)
    {
        SteeringDirection = steeringDirection;
    }
}
