using UnityEngine;

public class HouseCollider : MonoBehaviour
{
	public Vector2 houseSize = new(2.25f, 1.07f);
	public Vector2 imageOffset = new(0f, 0.5f);
	public float xOffset = 0f;

	private readonly float step_height = 0.31f;
	private readonly float step_width = 1.12f;
	private Vector2 HouseOffset => new(xOffset, houseSize.y / 2f);
	private Vector2 StepSize => new(step_width, step_height);
	private Vector2 StepOffset => new(0f,  -step_height / 2f);

	BoxCollider2D step;
	BoxCollider2D house;
	Transform image;

	void Reset()
	{
		image = transform.Find("Image");
		if (image == null) throw new System.Exception("Image child not found. Please create a child GameObject named 'Image'.");

		while (GetComponents<BoxCollider2D>().Length < 2)
			gameObject.AddComponent<BoxCollider2D>();

		FindColliders();
	}
	void FindColliders()
	{
		BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
		step = colliders[0];
		house = colliders[1];
	}

	public void OnValidate()
	{
		if (step == null || house == null)
			FindColliders();
		if (image == null)
			image = transform.Find("Image");

		step.size = StepSize;
		step.offset = StepOffset;
		house.size = houseSize;
		house.offset = HouseOffset;
		image.localPosition = imageOffset;
	}
}
