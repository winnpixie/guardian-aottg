using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
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

			public void ClearIndices()
			{
				for (int i = IndexStart; i < IndexStart + IndexCount; i++)
				{
					Pool.Indices[i] = 0;
				}
				Pool.IndiceChanged = true;
			}
		}

		public const int BlockSize = 108;

		public Vector3[] Vertices;

		public int[] Indices;

		public Vector2[] UVs;

		public Color[] Colors;

		public Vector2[] UVs2;

		public bool IndiceChanged;

		public bool ColorChanged;

		public bool UVChanged;

		public bool VertChanged;

		public bool UV2Changed;

		public Mesh Mesh;

		public Material Material;

		protected int VertexTotal;

		protected int VertexUsed;

		protected int IndexTotal;

		protected int IndexUsed;

		public bool FirstUpdate = true;

		protected bool VertCountChanged;

		public float BoundsScheduleTime = 1f;

		public float ElapsedTime;

		protected List<VertexSegment> SegmentList = new List<VertexSegment>();

		public VertexPool(Mesh mesh, Material material)
		{
			VertexTotal = (VertexUsed = 0);
			VertCountChanged = false;
			Mesh = mesh;
			Material = material;
			InitArrays();
			IndiceChanged = (ColorChanged = (UVChanged = (UV2Changed = (VertChanged = true))));
		}

		public void RecalculateBounds()
		{
			Mesh.RecalculateBounds();
		}

		public VertexSegment GetRopeVertexSeg(int maxcount)
		{
			return GetVertices(maxcount * 2, (maxcount - 1) * 6);
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
				num = (vcount / 108 + 1) * 108;
			}
			if (IndexUsed + icount >= IndexTotal)
			{
				num2 = (icount / 108 + 1) * 108;
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

		private void InitDefaultShaderParam(Vector2[] uv2)
		{
			for (int i = 0; i < uv2.Length; i++)
			{
				uv2[i].x = 1f;
				uv2[i].y = 0f;
			}
		}

		protected void InitArrays()
		{
			Vertices = new Vector3[4];
			UVs = new Vector2[4];
			UVs2 = new Vector2[4];
			Colors = new Color[4];
			Indices = new int[6];
			VertexTotal = 4;
			IndexTotal = 6;
			InitDefaultShaderParam(UVs2);
		}

		public void EnlargeArrays(int count, int icount)
		{
			Vector3[] vertices = Vertices;
			Vertices = new Vector3[Vertices.Length + count];
			vertices.CopyTo(Vertices, 0);
			Vector2[] uVs = UVs;
			UVs = new Vector2[UVs.Length + count];
			uVs.CopyTo(UVs, 0);
			Vector2[] uVs2 = UVs2;
			UVs2 = new Vector2[UVs2.Length + count];
			uVs2.CopyTo(UVs2, 0);
			InitDefaultShaderParam(UVs2);
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
			UV2Changed = true;
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
			if (UV2Changed)
			{
				Mesh.uv2 = UVs2;
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
			UV2Changed = false;
			VertChanged = false;
		}
	}
}
