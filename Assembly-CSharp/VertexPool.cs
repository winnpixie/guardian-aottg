using UnityEngine;

public class VertexPool
{
	public class VertexSegment
	{
		public int VertStart;

		public int IndexStart;

		public int VertCount;

		public int IndexCount;

		public VertexPool Pool;

		public VertexSegment(int start, int count, int istart, int icount, VertexPool pool)
		{
			VertStart = start;
			VertCount = count;
			IndexCount = icount;
			IndexStart = istart;
			Pool = pool;
		}
	}

	public const int BlockSize = 36;

	public Vector3[] Vertices;

	public int[] Indices;

	public Vector2[] UVs;

	public Color[] Colors;

	public bool IndiceChanged;

	public bool ColorChanged;

	public bool UVChanged;

	public bool VertChanged;

	public Mesh Mesh;

	public Material Material;

	protected int VertexTotal;

	protected int VertexUsed;

	protected int IndexTotal;

	protected int IndexUsed;

	protected bool FirstUpdate = true;

	protected bool VertCountChanged;

	public float BoundsScheduleTime = 1f;

	public float ElapsedTime;

	public VertexPool(Mesh mesh, Material material)
	{
		VertexTotal = (VertexUsed = 0);
		VertCountChanged = false;
		Mesh = mesh;
		Material = material;
		InitArrays();
		Vertices = Mesh.vertices;
		Indices = Mesh.triangles;
		Colors = Mesh.colors;
		UVs = Mesh.uv;
		IndiceChanged = (ColorChanged = (UVChanged = (VertChanged = true)));
	}

	public void RecalculateBounds()
	{
		Mesh.RecalculateBounds();
	}

	public Sprite AddSprite(float width, float height, STYPE type, ORIPOINT ori, Camera cam, int uvStretch, float maxFps)
	{
		VertexSegment vertices = GetVertices(4, 6);
		return new Sprite(vertices, width, height, type, ori, cam, uvStretch, maxFps);
	}

	public RibbonTrail AddRibbonTrail(float width, int maxelemnt, float len, Vector3 pos, int stretchType, float maxFps)
	{
		VertexSegment vertices = GetVertices(maxelemnt * 2, (maxelemnt - 1) * 6);
		return new RibbonTrail(vertices, width, maxelemnt, len, pos, stretchType, maxFps);
	}

	public Material GetMaterial()
	{
		return Material;
	}

	public VertexSegment GetVertices(int vcount, int icount)
	{
		int num = 0;
		int num2 = 0;
		if (VertexUsed + vcount >= VertexTotal)
		{
			num = (vcount / 36 + 1) * 36;
		}
		if (IndexUsed + icount >= IndexTotal)
		{
			num2 = (icount / 36 + 1) * 36;
		}
		VertexUsed += vcount;
		IndexUsed += icount;
		if (num != 0 || num2 != 0)
		{
			EnlargeArrays(num, num2);
			VertexTotal += num;
			IndexTotal += num2;
		}
		return new VertexSegment(VertexUsed - vcount, vcount, IndexUsed - icount, icount, this);
	}

	protected void InitArrays()
	{
		Vertices = new Vector3[4];
		UVs = new Vector2[4];
		Colors = new Color[4];
		Indices = new int[6];
		VertexTotal = 4;
		IndexTotal = 6;
	}

	public void EnlargeArrays(int count, int icount)
	{
		Vector3[] vertices = Vertices;
		Vertices = new Vector3[Vertices.Length + count];
		vertices.CopyTo(Vertices, 0);
		Vector2[] uVs = UVs;
		UVs = new Vector2[UVs.Length + count];
		uVs.CopyTo(UVs, 0);
		Color[] colors = Colors;
		Colors = new Color[Colors.Length + count];
		colors.CopyTo(Colors, 0);
		int[] indices = Indices;
		Indices = new int[Indices.Length + icount];
		indices.CopyTo(Indices, 0);
		VertCountChanged = true;
		IndiceChanged = true;
		ColorChanged = true;
		UVChanged = true;
		VertChanged = true;
	}

	public void LateUpdate()
	{
		if (VertCountChanged)
		{
			Mesh.Clear();
		}
		Mesh.vertices = Vertices;
		if (UVChanged)
		{
			Mesh.uv = UVs;
		}
		if (ColorChanged)
		{
			Mesh.colors = Colors;
		}
		if (IndiceChanged)
		{
			Mesh.triangles = Indices;
		}
		ElapsedTime += Time.deltaTime;
		if (ElapsedTime > BoundsScheduleTime || FirstUpdate)
		{
			RecalculateBounds();
			ElapsedTime = 0f;
		}
		if (ElapsedTime > BoundsScheduleTime)
		{
			FirstUpdate = false;
		}
		VertCountChanged = false;
		IndiceChanged = false;
		ColorChanged = false;
		UVChanged = false;
		VertChanged = false;
	}
}
