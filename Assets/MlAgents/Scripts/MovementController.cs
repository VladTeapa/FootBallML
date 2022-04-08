using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float speed = 2;
    [SerializeField]
    private float jumpPower = 5;
    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private float runTime = 2;
    [SerializeField]
    private float runTimeCoolDown = 5;
    [SerializeField]
    private float powerShotCoolDown = 5;
    private Vector3 moveDir;
    private Vector3 lookDir;
    private Vector3 initialPosition;
    private Rigidbody rigidbody;
    private ParticleSystem particle;
    private bool shoot = false;
    private bool canRun = true;
    private float initialSpeed;
    private bool powerShotEnabled=false;
    private bool canUsePowerShot = true;
    private Coroutine runCoroutine = null, powershotCoroutine = null;
    public bool GetPowerShotEnabled()
    {
        return powerShotEnabled;
    }
    public void SetMoveDir(Vector3 moveDir)
    {
        this.moveDir = moveDir;
    }
    public void SetLookDir(Vector3 lookDir)
    {
        this.lookDir = lookDir;
    }
    public void Jump()
    {
        Ray ray = new Ray(transform.position+Vector3.down*0.45f, Vector3.down);
        
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 0.15f, ground))
        {
             rigidbody.AddForce(Vector3.up * jumpPower,ForceMode.VelocityChange);
        }
    }
    public void Shoot(bool shoot)
    {
        this.shoot = shoot;
    }
    private IEnumerator RunCountDownPowerUp()
    {
        yield return new WaitForSeconds(powerShotCoolDown);
        canUsePowerShot = true;
    }
    private IEnumerator RunCountDown()
    {
        yield return new WaitForSeconds(runTime);
        speed = initialSpeed;
        yield return new WaitForSeconds(runTimeCoolDown-runTime);
        canRun = true;
    }
    public void Run()
    {
        if (canRun == false)
            return;
        runCoroutine = StartCoroutine(RunCountDown());
        speed *= 2;
        canRun = false;
    }
    public void UsePowerShot()
    {
        if(canUsePowerShot)
            powerShotEnabled = !powerShotEnabled;
        particle.enableEmission = powerShotEnabled;

    }
    public void ResetObject()
    {
        transform.rotation = Quaternion.identity;
        transform.position = initialPosition;
        if (runCoroutine != null)
            StopCoroutine(runCoroutine);
        if (powershotCoroutine != null)
            StopCoroutine(powershotCoroutine);
        shoot = false;
        canRun = true;
        powerShotEnabled = false;
        canUsePowerShot = true;
        speed = initialSpeed;
        moveDir = Vector3.zero;
        lookDir = Vector3.zero;
        rigidbody.velocity = Vector3.zero;
        particle.enableEmission = false;

    }
    private void Start()
    {
        initialPosition = transform.position;
        rigidbody = GetComponent<Rigidbody>();
        moveDir = Vector3.zero;
        lookDir = Vector3.zero;
        initialSpeed = speed;
        particle = GetComponent<ParticleSystem>();
        particle.enableEmission = false;

    }
    private void LateUpdate()
    {
        float y = rigidbody.velocity.y; 
        rigidbody.velocity = new Vector3((moveDir * speed).x,y,(moveDir * speed).z); 
        if(moveDir.magnitude <= 1e-5)
        {
            return;
        }
        transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag.Equals("Ball"))
        {
            if(shoot)
            {
               collision.collider.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 7, ForceMode.VelocityChange);
            }
            if(powerShotEnabled)
            {
                collision.collider.gameObject.GetComponent<Rigidbody>().velocity *= 2;
                powerShotEnabled=false;
                canUsePowerShot=false;
                particle.enableEmission = false;
                powershotCoroutine = StartCoroutine(RunCountDownPowerUp());
            }
        }
    }
}
