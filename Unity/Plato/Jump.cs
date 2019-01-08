using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ATF.Tools;

public abstract class Jump : MonoBehaviour {

    public enum JumpMode {
        SimpleJump, MultiJump, ForceJump, ForceMultiJump
    }
    
    [Header("Jump")]
    [SerializeField][Tooltip("Сила при прыжке")][Range(0, 30)] private float jumpImpulse = 15;
    [SerializeField][Tooltip("Режим Force")][ConditionalField("jumpMode", new object[] { JumpMode.ForceJump, JumpMode.ForceMultiJump })] private float jumpForce = 70;
    [Header("What is ground")]
    [SerializeField] [Tooltip("Радиус сферы, проверяющей платформу под персонажем")] private float radiusCheckSphere = 1f;
    [SerializeField] [Tooltip("Растояние между положением ног и центра персонажа")] private float feetsPosition = -0.56f;
    [SerializeField] [Tooltip("Цвет сферы")] private ColorChoice colorSphere;
    private Vector3 posFeets;
    [SerializeField] [Tooltip("Layer ground")] private LayerMask layerGround;
 

    [Header("Jump mode")]
    [SerializeField] [Tooltip("Режим прыжка")] private JumpMode jumpMode = JumpMode.SimpleJump;

    //Multijump
    [SerializeField] [ConditionalField("jumpMode", new object[] { JumpMode.MultiJump, JumpMode.ForceMultiJump })]
    [Tooltip("Кол-во прыжков в режиме MultyJump")] [Range(2, 5)] private int countMultyJump = 2;
    [SerializeField] [ConditionalField("jumpMode", new object[] { JumpMode.MultiJump, JumpMode.ForceMultiJump })]
    [Tooltip("Задержка между прыжкаме в ремиже MultyJump")] [Range(0, 1)] private float delayMultyJump = 0.5f;
    private int currentMultyJump = 0; //Кол-во прыжков до приземления
    private float currentTimeMultyJump = 0; //Время после прыжка в режиме MultyJump
    private bool multiJump = false; //Отслеживает второй прыжок

    //Forcejump
    [SerializeField][ConditionalField("jumpMode", new object[] { JumpMode.ForceJump, JumpMode.ForceMultiJump})] private float lerpImpulse = 0.15f; //Cкорость уменьшения импульса
    private float defImpulse = 4;

    //ForceMultiJump

    [Header("Draw")]
    [SerializeField] [Tooltip("Обозначать место прыжка и приземления")] private bool draw = true;


    //private bool canJump = true;

    [Header("Actions")]
    [SerializeField] private UnityEvent LandingEvent;

    private bool downJump = false; //ButtonDown не срабатывавет когда нажимаешь на в воздухе
    private bool buttonDownJump;

    private bool isLanding = false; //Приземление
    private bool isJump = false; //Прыжок
                                 
    private Rigidbody2D body;
    private Animator anim;
    private Access access;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        access = GetComponent<Access>();
    }

    private void Start() {
        defImpulse = jumpForce;
    }

    public void CollStay2D(bool buttonDownJump) {
        if ((jumpMode == JumpMode.MultiJump && currentMultyJump == 0) || (jumpMode != JumpMode.MultiJump && jumpMode != JumpMode.ForceMultiJump) || ((jumpMode == JumpMode.ForceMultiJump && currentMultyJump == 0))) {
            access.CanJump = true;
        }
    }
    public void JumpUpdate(bool buttonDown) {
        buttonDownJump = buttonDown;
        if (downJump && !buttonDownJump) {
            downJump = false;
        }
    }

    public IEnumerator DoJump() {
        if (access.CanJump && !downJump && IsGround() && !multiJump) {

            downJump = true;
            access.CanJump = false;
            SetAnimations(true);
            
            body.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);

            if (jumpMode == JumpMode.ForceJump || jumpMode == JumpMode.ForceMultiJump) {
                defImpulse = jumpForce;
                while (downJump && defImpulse > 0.02f) {
                    body.AddForce(Vector2.up * defImpulse, ForceMode2D.Force);
                    defImpulse = Mathf.LerpUnclamped(defImpulse, 0f, lerpImpulse);

                    yield return null;
                }
            } 
            if (jumpMode == JumpMode.MultiJump || jumpMode == JumpMode.ForceMultiJump) {

                multiJump = true;
                currentMultyJump++;
                
                while (currentMultyJump < countMultyJump && !isLanding) {

                    while (!access.CanJump && currentTimeMultyJump < delayMultyJump) {

                        currentTimeMultyJump += Time.deltaTime;

                        yield return null;
                    }

                    currentTimeMultyJump = 0;
                    access.CanJump = true;

                    if (buttonDownJump && !downJump && !isLanding) {
                        downJump = true;
                        access.CanJump = false;

                        body.velocity = new Vector2(body.velocity.x, 0);
                        anim.SetTrigger("doubleJump");

                        if (jumpMode == JumpMode.MultiJump) {
                            body.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
                        } else if (jumpMode == JumpMode.ForceMultiJump) {
                            defImpulse = jumpImpulse;
                            while (downJump && defImpulse > 0.02f) {
                                body.AddForce(Vector2.up * defImpulse, ForceMode2D.Impulse);
                                defImpulse = Mathf.LerpUnclamped(defImpulse, 0f, lerpImpulse);

                                yield return null;
                            }
                        }
                        
                        currentMultyJump++;
                    }

                    yield return null;
                }
            }


        }
    }
    
    public void Landing() {
        if (!isLanding && IsGround()) {

            access.CanJump = true;
            downJump = buttonDownJump;
            SetAnimations(false);

            if (LandingEvent != null)
                LandingEvent.Invoke();

            if (JumpMode.MultiJump == jumpMode || jumpMode == JumpMode.ForceMultiJump) {
                currentMultyJump = 0;
                multiJump = false;
            }
        }        
    }
    private void SetAnimations(bool condition) {
        isJump = condition;
        anim.SetBool("isJump", isJump);
        isLanding = !condition;
        anim.SetBool("isLanding", isLanding);
    }
    private bool IsGround() {
        return Physics2D.OverlapCircle(transform.position + new Vector3(0, feetsPosition, 0), radiusCheckSphere, layerGround);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
    }

    private void OnCollisionEnter2D() {
        Landing();
    }

    protected abstract bool DoAction();

    private void Update() {
        if (DoAction()) {
            StartCoroutine(DoJump());
        }

        JumpUpdate(DoAction());
    }

    private void OnDrawGizmosSelected() {
        if (draw) {
            Gizmos.color = colorSphere.CurrentColor;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, feetsPosition, 0), radiusCheckSphere);
        }
    }
}
