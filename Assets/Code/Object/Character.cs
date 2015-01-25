using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Character : MonoBehaviour
{
	public float maxVolume = .7f;
	public float speed = 5f;
	public float speedBoost = 2f;
	public float rotationSpeed = 100f;

	public UnityEngine.UI.Text coordinateDisplay;

	new Rigidbody2D rigidbody;

	Transform t;

	void Start()
	{
		t = GetComponent<Transform>();
		rigidbody = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		audio.volume = Mathf.Lerp(audio.volume, acceleration.y * maxVolume, Time.deltaTime);
	}

	void FixedUpdate()
	{
		Move();

		if(coordinateDisplay)
			coordinateDisplay.text = string.Format("GSC({0,-5:G}, {1,-5:G})", transform.position.x.ToString("F2"), transform.position.y.ToString("F2"));
	}

	/**
	 * WASD
	 */
	Vector2 acceleration = Vector2.zero;
	void Move()
	{
		acceleration.y = CurrentAcceleration();
		float angle = -Input.GetAxisRaw("Horizontal");

		Vector3 rotation = t.localRotation.eulerAngles;
		rotation.z += angle * rotationSpeed * Time.deltaTime;
		t.localRotation = Quaternion.Euler(rotation);

		rigidbody.AddRelativeForce(acceleration * speed);
	}

	public float CurrentAcceleration()
	{
		return Mathf.Clamp( Input.GetAxisRaw("Vertical") * (Input.GetKey(KeyCode.LeftShift) ? speedBoost : 1f), 0f, speed + speedBoost);
	}

	public Vector2 CurrentSpeed()
	{
		return rigidbody.velocity;
	}
}
