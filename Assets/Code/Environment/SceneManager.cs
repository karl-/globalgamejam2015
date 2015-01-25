using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneManager : MonoBehaviour
{
	public Material spaceMaterial;
	public Character ship;

	public CameraFollow cam;
	Dictionary<GridUnit, GameObject> loadedTiles = new Dictionary<GridUnit, GameObject>();
	GridUnit bottomLeftGrid = new GridUnit(-1, -1);
	GridUnit topRightGrid = new GridUnit(1, 1);

	private Transform SpaceParent;

	void Awake()
	{
		cam.OnCameraMove += this.OnCameraMove;

		SpaceParent = new GameObject().transform;
		SpaceParent.name = "Space";
	}

	void OnCameraMove(Bounds bounds)
	{
		List<GridUnit> visible = new List<GridUnit>();

		bottomLeftGrid = Grid.GridPosition(bounds.center - bounds.extents);
		topRightGrid = Grid.GridPosition(bounds.center + bounds.extents);

		for(int y = bottomLeftGrid.y-2; y <= topRightGrid.y+2; y++)
		{
			for(int x = bottomLeftGrid.x-2; x <= topRightGrid.x+2; x++)
			{
				visible.Add( new GridUnit(x,y) );
			}
		}

		// now that we konw what units are in culling range, generate / destroy old tiles
		StartCoroutine( LoadTiles(visible) );
	}

	IEnumerator LoadTiles(List<GridUnit> visible)
	{
		foreach(GridUnit gu in visible)
		{
			if(!loadedTiles.ContainsKey(gu))
			{
				loadedTiles.Add(gu, CreateTile(gu.x, gu.y));
			}

			yield return null;
		}

		// unload culled tiles
		UnloadCulledTiles();
	}

	void UnloadCulledTiles()
	{
		List<GridUnit> destroy = new List<GridUnit>();

		foreach(KeyValuePair<GridUnit, GameObject> kvp in loadedTiles)
		{
			if( !InRetainBounds(kvp.Key) )
			{
				GameObject.Destroy( kvp.Value.GetComponent<MeshRenderer>().sharedMaterial.mainTexture );
				GameObject.Destroy( kvp.Value );

				destroy.Add(kvp.Key);
			}
		}

		foreach(GridUnit g in destroy)
		{
			loadedTiles.Remove(g);
		}
	}

	GameObject CreateTile(int x, int y)
	{
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
		go.transform.parent = SpaceParent;

		go.transform.localScale = Vector3.one * Grid.UnitSize;
		go.transform.position = new Vector3(x * Grid.UnitSize, y * Grid.UnitSize, 1f);

		Material mat = (Material)GameObject.Instantiate(spaceMaterial);

		mat.mainTexture = SpaceGenerator.Create( new GridUnit(x+Grid.UnitSize, y+Grid.UnitSize) );

		go.GetComponent<MeshRenderer>().sharedMaterial = mat;

		// float noise = Mathf.PerlinNoise( x + x/(float)Grid.UnitSize, y + y/(float)Grid.UnitSize );

		// Debug.Log(string.Format("noise: {0}, {1}, {2}", x, y, noise));

		for(int i = 0; i < Random.Range(0, 2); i++)
		{
			Vector3 pos = new Vector3( Random.Range(-Grid.UnitSize/2f, Grid.UnitSize/2f), Random.Range(-Grid.UnitSize/2f, Grid.UnitSize/2f), -0.02f);
			GameObject asteroid = AsteroidGenerator.Create( Random.Range(1f, 24f), pos);

			asteroid.transform.position = pos + go.transform.position;
			asteroid.transform.parent = go.transform;

			if(asteroid.GetComponent<PolygonCollider2D>().OverlapPoint(ship.transform.position))
				Destroy(asteroid);
		}
			
		return go;
	}

	bool InRetainBounds(GridUnit g)
	{
		return  g.x > bottomLeftGrid.x - 6 &&
				g.x < topRightGrid.x + 6 &&
				g.y > bottomLeftGrid.y - 6 &&
				g.y < topRightGrid.y + 6;
	}
}
