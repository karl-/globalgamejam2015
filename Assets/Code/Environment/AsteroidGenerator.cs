using UnityEngine;
using System.Collections;

public class AsteroidGenerator
{
	const float EDGE_WIDTH = .2f;
	const int VERTEX_COUNT = 32;

	public static Material AsteroidMaterial { get { return (Material)Resources.Load("AsteroidMaterial"); } }
	public static Material AsteroidOutline { get { return (Material)Resources.Load("AsteroidOutline"); } }

	public static GameObject Create(float size, Vector2 seed)
	{
		Vector3 scale = new Vector3(Random.Range(.5f, 1.5f), Random.Range(.5f, 1.5f), 1f) * size;

		Vector3[] vertices = new Vector3[VERTEX_COUNT * 2 + 1];
		Vector2[] uvs = new Vector2[VERTEX_COUNT * 2 + 1];
		int[] tris_asteroid = new int[(VERTEX_COUNT+1) * 3];
		int[] tris_outline = new int[(VERTEX_COUNT+1) * 3];

		vertices[0] = Vector3.zero;
		uvs[0] = vertices[0];

		float wobbleRange = (1f/(VERTEX_COUNT+1)) / 2f;
		
		for(int i = 1; i < VERTEX_COUNT; i++)
		{
			vertices[i] = new Vector3(	Mathf.Cos( (((i/(float)VERTEX_COUNT) * 360f) + Random.Range(-wobbleRange, wobbleRange)) * Mathf.Deg2Rad ),
										Mathf.Sin( (((i/(float)VERTEX_COUNT) * 360f) + Random.Range(-wobbleRange, wobbleRange)) * Mathf.Deg2Rad ),
										0f);

			float noise = Random.Range(1f, 1.1f);

			vertices[i] = (vertices[i]-Vector3.zero) * noise;
			vertices[i] = Vector3.Scale(vertices[i], scale);

			vertices[i+VERTEX_COUNT] = vertices[i] + (vertices[i].normalized * EDGE_WIDTH);
			vertices[i+VERTEX_COUNT].z = .01f;

			uvs[i] = vertices[i];
			uvs[i+VERTEX_COUNT] = vertices[i];

			// min = Vector2.Min(min, uvs[i]);
			// max = Vector2.Max(max, uvs[i]);
		}

		// Vector2 v = max-min;
		// float mx = v.x > v.y ? v.x : v.y;

		// for(int i = 0; i < VERTEX_COUNT + 1; i++)
		// {
		// 	uvs[i] = (uvs[i] + (Vector2.zero-min)) * (1f/mx);
		// }

		int n = 0;
		for(int i = 1; i < VERTEX_COUNT-1; i++)
		{
			tris_asteroid[n+0] = 0;
			tris_asteroid[n+1] = i+1;
			tris_asteroid[n+2] = i;
			
			tris_outline[n+0] = 0;
			tris_outline[n+1] = i+VERTEX_COUNT+1;
			tris_outline[n+2] = i+VERTEX_COUNT;			

			n += 3;
		}

		tris_outline[n+0] 	= 0;
		tris_asteroid[n+0] 	= 0;

		tris_outline[n+1] 	= (VERTEX_COUNT+1);
		tris_asteroid[n+1] 	= 1;

		tris_outline[n+2] 	= VERTEX_COUNT+(VERTEX_COUNT-1);
		tris_asteroid[n+2] 	= VERTEX_COUNT-1;

		Mesh m = new Mesh();
		m.subMeshCount = 2;

		m.vertices = vertices;
		m.SetTriangles(tris_asteroid, 0);
		m.SetTriangles(tris_outline, 1);
		m.uv = uvs;

		GameObject go = new GameObject();
		go.AddComponent<MeshFilter>().sharedMesh = m;
		go.AddComponent<MeshRenderer>().sharedMaterials = new Material[] { AsteroidMaterial, AsteroidOutline };


		Vector2[] collisions = new Vector2[VERTEX_COUNT-1];
		for(int i = 0; i < VERTEX_COUNT-1; i++)
			collisions[i] = vertices[i + VERTEX_COUNT + 1];

		go.AddComponent<PolygonCollider2D>().SetPath(0, collisions);
		go.AddComponent<AsteroidController>();

		return go;
	}
}
