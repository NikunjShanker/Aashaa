using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
	public static CharacterController2D instance;

	[SerializeField] private float m_JumpForce;									// Amount of force added when the player jumps.
	[SerializeField] private float knockbackForce;								// Amount of force added when the player hits an enemy/projectile/trap.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing;			// How much to smooth out the movement.
	[SerializeField] private bool m_AirControl = true;                          // Whether or not a player can steer while jumping.
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character.
	[SerializeField] private LayerMask m_WhatIsWall;							// A mask determining what is ground to the character.
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_WallCheck;								// A position marking where to check if the player is grounded.

	float k_GroundedRadius = .2f;												// Radius of the overlap circle to determine if grounded
	private bool m_Grounded;                                                    // Whether or not the player is grounded.
	private bool nearWall;
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;											// For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	// Double jump variables
	public int jumpNum;
	private bool jumpCooldown;
	private bool dashCooldown;

	// Abilities
	public bool canMove;
	public bool canDoubleJump;
	public bool canDash;
	public bool canWallJump;
	public bool speedIncrease;
	public bool jumpIncrease;

	private void Awake()
	{
		if (instance == null) instance = this;

		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		canDoubleJump = UniversalScript.instance.canDoubleJump;
		canDash = UniversalScript.instance.canDash;
		canWallJump = UniversalScript.instance.canWallJump;
		speedIncrease = UniversalScript.instance.speedBoost;
		jumpIncrease = UniversalScript.instance.jumpBoost;
	}

    private void Start()
    {
		m_MovementSmoothing = 0.05f;
		m_JumpForce = 575f;
		knockbackForce = 13f;
		nearWall = false;

		canMove = true;
	}

    private void FixedUpdate()
	{
		m_Grounded = false;
		nearWall = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject.layer == 9)
			{
				m_Grounded = true;

				if(canDoubleJump) jumpNum = 2;
				else jumpNum = 1;

				jumpCooldown = false;
				StopCoroutine(jumpCountdown());
				PlayerAnimationController.instance.setJump(false);
			}
		}

		Collider2D[] wallColliders = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_WhatIsWall);
		for (int i = 0; i < wallColliders.Length; i++)
		{
			if (wallColliders[i].gameObject.layer == 9 && canWallJump)
			{
				nearWall = true;
				if (canDoubleJump) jumpNum = 2;
				else jumpNum = 1;
			}
		}
	}

    public void Move(float move, bool jump)
	{
		if(canMove)
        {
			//only control the player if grounded or airControl is turned on
			if (m_Grounded || m_AirControl)
			{
				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(move * 9.5f, m_Rigidbody2D.velocity.y);
				if(speedIncrease) targetVelocity = new Vector2(move * 11f, m_Rigidbody2D.velocity.y);

				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}
			}

			// If the player should jump...
			if (jump && jumpNum != 0 && !jumpCooldown)
			{
				if (!dashCooldown) m_Rigidbody2D.velocity = Vector2.zero;

				// Add a vertical force to the player.
				PlayerAnimationController.instance.setJump(true);

				if (jumpIncrease) m_JumpForce = 720f;

				if (nearWall && !m_Grounded)
				{
					Flip();
					m_Rigidbody2D.AddForce(new Vector2(1000f * transform.localScale.x, m_JumpForce));
				}
				else m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));

				AudioManagerScript.instance.sounds[4].pitch = Random.Range(1.2f, 1.4f);
				AudioManagerScript.instance.Play("jump");

				jumpNum--;
				m_Grounded = false;
				jumpCooldown = true;
				createDust();
				StartCoroutine(jumpCountdown());
			}

			if(m_Grounded && move != 0)
            {
				AudioManagerScript.instance.Play("run");
            }
			else
            {
				AudioManagerScript.instance.Stop("run");
			}
		}
	}

	public void Dash()
    {
		if(canDash && canMove)
        {
			// Move the character by finding the target velocity
			Vector3 targetVelocity;
			if (Application.isEditor)
            {
				targetVelocity = new Vector2(transform.localScale.x * 550f, m_Rigidbody2D.velocity.y);
			}
			else
            {
				targetVelocity = new Vector2(transform.localScale.x * 250f, m_Rigidbody2D.velocity.y);
			}

			AudioManagerScript.instance.Play("dash");

			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
			StartCoroutine(dashCountdown());
			createDust();
		}
	}

	public void PushBack()
    {
		m_Rigidbody2D.velocity = Vector2.zero;
		m_Rigidbody2D.velocity = new Vector2(-transform.localScale.x * knockbackForce, knockbackForce/1.3f);
		CameraShakeScript.shake.shakeCamera(2.5f, 0.1f);
		StartCoroutine(disableMoveCountdown());
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void createDust()
    {
		this.transform.Find("Dust PS").GetComponentInChildren<ParticleSystem>().Play();
    }

	IEnumerator disableMoveCountdown()
    {
		canMove = false;
		yield return new WaitForSeconds(0.1f);
		canMove = true;
	}

	IEnumerator jumpCountdown()
    {
		yield return new WaitForSeconds(0.5f);
		jumpCooldown = false;
    }

	IEnumerator dashCountdown()
    {
		yield return new WaitForSeconds(0.2f);
		dashCooldown = false;
	}
}
