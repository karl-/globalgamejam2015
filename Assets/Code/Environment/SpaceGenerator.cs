using UnityEngine;
using System.Collections;

public class SpaceGenerator : MonoBehaviour
{
	const int TEX_SIZE = 256;

	/**
	 * Return a gradient noise texture at block coordinates.
	 */
	public static Texture2D Create(GridUnit InGridUnit)
	{
		Color[] pix = new Color[TEX_SIZE * TEX_SIZE];
		Texture2D tex = new Texture2D(TEX_SIZE, TEX_SIZE);

		for (int y = 0; y < TEX_SIZE; y++)
		{
			for (int x = 0; x < TEX_SIZE; x++)
			{
				float xCoord = InGridUnit.x + ( (float)x / TEX_SIZE );
				float yCoord = InGridUnit.y + ( (float)y / TEX_SIZE );

				float sample = Mathf.PerlinNoise(xCoord, yCoord);

				pix[y * TEX_SIZE + x] = new Color(sample, sample, sample, 1f);
			}
		}
		
		tex.SetPixels(pix);
		tex.Apply();

		tex.wrapMode = TextureWrapMode.Clamp;

		return tex;
	}
}
