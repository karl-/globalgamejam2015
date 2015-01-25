using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	const float MIN_ZOOM = 5f;
	const float MAX_ZOOM = 25f;

	public float speed = 10f;
	private float zoomSpeed = 10f;
	public float rotationSpeed = .5f;
	public float followDistance = 1.5f;

	public Character ship;
	public GameObject compass;
	private Transform t;
	private Camera cam;

	public delegate void OnCameraMoveEvent(Bounds frustumBounds);
	public event OnCameraMoveEvent OnCameraMove;

	void Awake()
	{
		cam = gameObject.GetComponent<Camera>();
		t = transform;
	}

	void Start()
	{
		if(OnCameraMove != null)
			OnCameraMove( CameraFrustumBounds() );
	}

	bool approx(Vector2 lhs, Vector2 rhs)
	{
		return 	Mathf.Abs(lhs.x - rhs.x) < .1f && 
				Mathf.Abs(lhs.y - rhs.y) < .1f;
	}

	bool approx(float lhs, float rhs)
	{
		return Mathf.Abs(lhs - rhs) < .001f;
	}

	float xVelocity, yVelocity;
	public float smooth = .5f;

	float dot = 0f, dotperp = 0f;

	void FixedUpdate()
	{
		Vector2 target = ship.transform.position + (ship.transform.TransformDirection(Vector2.up) * -followDistance);

		float xpos = Mathf.SmoothDamp(t.position.x, target.x, ref xVelocity, smooth);		
		float ypos = Mathf.SmoothDamp(t.position.y, target.y, ref yVelocity, smooth);		

		t.position = new Vector3(xpos, ypos, t.position.z);

		// Camera rotation
		Vector3 rotation = t.localRotation.eulerAngles;

		Vector2 camDir = t.transform.TransformDirection(Vector2.up);
		Vector2 camDirPerp = t.transform.TransformDirection(Vector2.right);
		Vector2 shipDir = ship.transform.TransformDirection(Vector2.up);

		dot = Vector2.Dot(camDir, shipDir);
		dotperp = Vector2.Dot(camDirPerp, shipDir);

		if( dot < .999f )// && ship.CurrentSpeed().magnitude > 1f )
		{
			rotation.z += (dotperp < 0f ? rotationSpeed : -rotationSpeed) * (Mathf.Abs(dotperp) * 3f);

			t.localRotation = Quaternion.Euler(rotation);
		}

		// update the camera
		if(OnCameraMove != null)
		{
			OnCameraMove( CameraFrustumBounds() );
		}

		float scroll = Input.GetAxis("Mouse ScrollWheel");

		if( !approx(scroll, 0f) )
		{
			cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * (cam.orthographicSize / (MAX_ZOOM-MIN_ZOOM) * zoomSpeed), MIN_ZOOM, MAX_ZOOM);

			if( OnCameraMove != null)
				OnCameraMove( CameraFrustumBounds() );
		}
	}

	// void OnGUI()
	// {
	// 	GUILayout.Label(string.Format("Ship: {0}\nPosition: {1}\nRotation: {2}",
	// 	                Grid.GridPosition(ship.transform.position),
	// 	                ship.transform.position,
	// 	                ship.transform.localRotation.eulerAngles));

	// 	GUILayout.Label("Dot: " + dot);
	// 	GUILayout.Label("Prep: " + dotperp);

	// }

	void Update()
	{
		compass.transform.localRotation = t.localRotation; // Quaternion.Inverse(t.localRotation);
	}

	Bounds CameraFrustumBounds()
	{
		Vector3 bottomLeft = camera.ViewportToWorldPoint(Vector3.zero);
		Vector3 topRight = camera.ViewportToWorldPoint(Vector3.one);

		Vector3 size = topRight - bottomLeft;
		return new Bounds(size / 2f + bottomLeft, size);
	}
}
