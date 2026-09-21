using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Brushes/SplitHouseBrush", fileName = "SplitHouseBrush")]
[CustomGridBrush(false, true, false, "Berry Resource Brush")]
public class SplitHouseBrush : GridBrush
{
	public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
	{
		Tilemap lower = brushTarget.transform.Find("Lower").GetComponent<Tilemap>();
		Tilemap upper = brushTarget.transform.Find("Upper").GetComponent<Tilemap>();

		int height = size.y;
		int width = size.x;
		int halfHeight = height / 2;

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				int index = y * width + x;
				if (index >= cells.Length) continue;

				TileBase tile = cells[index].tile;
				Vector3Int tilePos = position + new Vector3Int(x, y, 0);

				if (y < halfHeight)
					lower.SetTile(tilePos, tile);
				else
					upper.SetTile(tilePos, tile);
			}
		}
		base.Paint(grid, brushTarget, position);
	}
}
