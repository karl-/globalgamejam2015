using UnityEngine;

public struct GridUnit
{
	public int x, y;

	public GridUnit(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override string ToString()
	{
		return "(" + x + ", "+ y + ")";
	}

	public static bool operator == (GridUnit lhs, GridUnit rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y;
	}

	public static bool operator != (GridUnit lhs, GridUnit rhs)
	{
		return lhs.x != rhs.x || lhs.y != rhs.y;
	}

	public override bool Equals(System.Object obj)
	{
		if( obj is GridUnit )
		{
			GridUnit rhs = (GridUnit)obj;
			return this.x == rhs.x && this.y == rhs.y;
		}
		else
		{
			return false;
		}
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}

public static class Grid
{
	public const int UnitSize = 50;

	/**
	 * Convert world position to grid unit.
	 */
	public static GridUnit GridPosition(Vector3 v)
	{
		return new GridUnit( (int)Mathf.Round(v.x/UnitSize), (int)Mathf.Round(v.y/UnitSize) );
	}

	public static Vector2 WorldPosition(GridUnit u)
	{
		return new Vector2(u.x * 10f, u.y * 10f);
	}
}