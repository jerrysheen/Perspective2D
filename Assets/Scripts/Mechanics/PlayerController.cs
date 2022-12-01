using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        protected float _currentGravity = 0;
        public ParticleSystem m_particle;
        

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 45;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            m_particle = this.transform.Find("DustParticles")?.GetComponent<ParticleSystem>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_particle.gameObject.SetActive(false);
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");
                if (Input.GetButtonDown("Fire1"))
                {
                    animator.SetBool("Attack",true);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    animator.SetBool("Attack", false);
                }

                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            // 这个计算貌似不太好， 替掉
            // 首先起跳应该是个瞬时行为吧
            // 如果在地上，起跳
            //Debug.Log(IsGrounded);
            if (jumpState == JumpState.Landed || jumpState == JumpState.Grounded)
            {
                animator.SetBool("Jump", false);
            }
            else
            {
                animator.SetBool("Jump", true);
                m_particle.gameObject.SetActive(false);
            }

            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else
            {
                    //Debug.Log(model.gForce * gravityModifier);
                    //velocity.y = velocity.y * model.jumpDeceleration;
                    //velocity.y -= 0.5f * model.gForce * gravityModifier* Time.deltaTime * Time.deltaTime;
                    //velocity.y -= gravityModifier * Time.deltaTime;
                    // 这个地方应该用个重力模型吧？
                    _currentGravity = model.Gravity;
                    if (velocity.y > 0)
                    {
                        _currentGravity = _currentGravity / model.AscentMultiplier;
                    }
                    if (velocity.y < 0)
                    {
                        _currentGravity = _currentGravity * model.FallMultiplier;
                    }
                    
                    //if (_gravityActive)
                    {
                        //velocity.y += (_currentGravity + _movingPlatformCurrentGravity) * Time.deltaTime;
                        velocity.y += (_currentGravity + 0.0f) * Time.deltaTime;
                    }
                    
                    
                //}
            }




            //animator.SetBool("grounded", IsGrounded);
            //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            if (move == Vector2.zero)
            {
                animator.SetBool("Walk", false);
                m_particle.gameObject.SetActive(false);

            }
            else
            {
                animator.SetBool("Walk", true);
                m_particle.gameObject.SetActive(true);
            }
            
            // flip
            if (move.x > 0.01f)
            {
                spriteRenderer.flipX = false;
                m_particle.gameObject.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
            }
            else if (move.x < -0.01f)
            {
                spriteRenderer.flipX = true;
                m_particle.gameObject.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }
            
            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            
//            Debug.Log("Trigger : " + other.gameObject.name);
        }
    }
}