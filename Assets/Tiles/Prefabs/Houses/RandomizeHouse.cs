using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum HouseColor
{
	none,
	Green,
	Blue,
	Red
}

[System.Serializable]
public class HouseVarient
{
	public Sprite green;
	public Sprite blue;
	public Sprite red;
	public Vector2 imageOffset;
	public Vector2 houseSize;
	public float xOffset;
}

[ExecuteInEditMode]
[RequireComponent(typeof(HouseCollider))]
public class RandomizeHouse : MonoBehaviour
{
	public List<HouseVarient> houseVarients = new();
	public int selectedIndex = -1;
	public HouseColor selectedColor = HouseColor.none;

	private SpriteRenderer _image;
	private SpriteRenderer Image
	{
		get
		{
			if (_image == null)
			{
				_image = transform.Find("Image").GetComponent<SpriteRenderer>();
			}
			return _image;
		}
	}
	private HouseCollider _houseCollider;
	private HouseCollider HouseCollider
	{
		get
		{
			if (_houseCollider == null)
			{
				_houseCollider = GetComponent<HouseCollider>();
			}
			return _houseCollider;
		}
	}

	private void Awake()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
		{
			// If this is a prefab asset, do NOT randomize
			if (PrefabUtility.IsPartOfPrefabAsset(this))
				return;
		}
#endif

		if (selectedIndex == -1)
			selectedIndex = Random.Range(0, houseVarients.Count);
		if (selectedColor == HouseColor.none)
			selectedColor = (HouseColor)Random.Range(1, 4);

		HouseVarient selectedVarient = houseVarients[selectedIndex];
		if (HouseCollider)
		{
			HouseCollider.houseSize = selectedVarient.houseSize;
			HouseCollider.imageOffset = selectedVarient.imageOffset;
			HouseCollider.xOffset = selectedVarient.xOffset;
			HouseCollider.OnValidate();
		}

		if (Image == null) return;
		switch (selectedColor)
		{
			case HouseColor.Green:
				Image.sprite = selectedVarient.green;
				break;
			case HouseColor.Blue:
				Image.sprite = selectedVarient.blue;
				break;
			case HouseColor.Red:
				Image.sprite = selectedVarient.red;
				break;
		}
	}
}
