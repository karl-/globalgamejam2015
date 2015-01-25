using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour
{
	float Gravity = 1f;
	float m_Gravity = 1f;
	CircleCollider2D cir;

	float max(Vector3 v)
	{
		return (v.x > v.y && v.x > v.z) ? v.x : ((v.y > v.x && v.y > v.z) ? v.y : v.z);
	}

	void Start()
	{
		cir = gameObject.AddComponent<CircleCollider2D>();
		cir.radius = max(GetComponent<MeshRenderer>().bounds.size);
		cir.isTrigger = true;

		m_Gravity = cir.radius * .05f * Gravity;
	}

	Vector2 force = Vector2.zero;
	void OnTriggerStay2D(Collider2D col)
	{
		Rigidbody2D rigid = col.attachedRigidbody;

		if(rigid != null)
		{
			float dist = Vector2.Distance(col.transform.position, transform.position);
			force = (-m_Gravity * (Vector2)(col.transform.position - transform.position).normalized) * (1f-dist/cir.radius);
			rigid.AddForce( force );
		}
	}
}
