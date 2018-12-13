using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour {

    [Header("Velocity")]
    [SerializeField] [Tooltip("Max movement speed")] private float maxSpeed = 5f;
    [SerializeField] [Tooltip("Rate of chenge of speed")] private float acceleration = 1000f;
    
    private Rigidbody2D body;
    private Animator anim;
    private Access access;

    //private bool isWalk = false;
    private bool isFlip = true;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        access = GetComponent<Access>();
    }
    

    private void Move(float axis) {
        if (access.CanMove && axis != 0) {
            body.AddForce(new Vector2(axis * maxSpeed - body.velocity.x, 0) * acceleration * Time.deltaTime, ForceMode2D.Force);
        }

        //isWalk = Mathf.Abs(axis) > 0 && Mathf.Abs(body.velocity.x) > 0.05f ? true : false;
    }

    private void Update() {
        SetAnimations();
        ScalePlayer(Mathf.Abs(body.velocity.x) > 0.05f ? body.velocity.x  : 0);
    }

    private void FixedUpdate() {
        Move(SetAxis());
    }

    public abstract float SetAxis();

    public void SetAnimations() {
        //anim.SetFloat("X", body.velocity.x);
        anim.SetFloat("Y", body.velocity.y);
        anim.SetInteger("velocityX", Mathf.RoundToInt(Mathf.Abs(body.velocity.x)));
        anim.SetInteger("velocityY", Mathf.RoundToInt(Mathf.Abs(body.velocity.y)));
        //anim.SetBool("isWalk", isWalk);
    }

    public void ScalePlayer(float axis) {
        if (isFlip && axis < 0 || !isFlip && axis > 0) {
            isFlip = !isFlip;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
    
    
}
