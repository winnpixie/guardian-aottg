using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	public class XWeaponTrail : MonoBehaviour
	{
		public class Element
		{
			public Vector3 PointStart;

			public Vector3 PointEnd;

			public Vector3 Pos => (PointStart + PointEnd) / 2f;

			public Element(Vector3 start, Vector3 end)
			{
				PointStart = start;
				PointEnd = end;
			}

			public Element()
			{
			}
		}

		public static string Version = "1.0.1";
		public Transform PointStart;
		public Transform PointEnd;
		public int MaxFrame = 14;
		public int Granularity = 60;
		public float Fps = 60f;
		public Color MyColor = Color.white;
		public Material MyMaterial;
		protected float mTrailWidth;
		protected Element mHeadElem = new Element();
		protected List<Element> mSnapshotList = new List<Element>();
		protected Spline mSpline = new Spline();
		protected float mFadeT = 1f;
		protected bool mIsFading;
		protected float mFadeTime = 1f;
		protected float mElapsedTime;
		protected float mFadeElapsedime;
		protected GameObject mMeshObj;
		protected VertexPool mVertexPool;
		protected VertexPool.VertexSegment mVertexSegment;
		protected bool mInited;
		public float UpdateInterval => 1f / Fps;
		public Vector3 CurHeadPos => (PointStart.position + PointEnd.position) / 2f;
		public float TrailWidth => mTrailWidth;

		public void Init()
		{
			if (!mInited)
			{
				mTrailWidth = (PointStart.position - PointEnd.position).magnitude;
				InitMeshObj();
				InitOriginalElements();
				InitSpline();
				mInited = true;
			}
		}

		public void Activate()
		{
			MaxFrame = 14;
			Init();
			if (mMeshObj == null)
			{
				InitMeshObj();
				return;
			}
			base.gameObject.SetActive(value: true);
			if (mMeshObj != null)
			{
				mMeshObj.SetActive(value: true);
			}
			mFadeT = 1f;
			mIsFading = false;
			mFadeTime = 1f;
			mFadeElapsedime = 0f;
			mElapsedTime = 0f;
			for (int i = 0; i < mSnapshotList.Count; i++)
			{
				mSnapshotList[i].PointStart = PointStart.position;
				mSnapshotList[i].PointEnd = PointEnd.position;
				mSpline.ControlPoints[i].Position = mSnapshotList[i].Pos;
				mSpline.ControlPoints[i].Normal = mSnapshotList[i].PointEnd - mSnapshotList[i].PointStart;
			}
			RefreshSpline();
			UpdateVertex();
		}

		public void Deactivate()
		{
			base.gameObject.SetActive(value: false);
			if (mMeshObj != null)
			{
				mMeshObj.SetActive(value: false);
			}
		}

		public void StopSmoothly(float fadeTime)
		{
			mIsFading = true;
			mFadeTime = fadeTime;
		}

		public void update()
		{
			if (!mInited)
			{
				return;
			}
			if (mMeshObj == null)
			{
				InitMeshObj();
				return;
			}
			UpdateHeadElem();
			mElapsedTime += Time.deltaTime;
			if (!(mElapsedTime < UpdateInterval))
			{
				mElapsedTime -= UpdateInterval;
				RecordCurElem();
				RefreshSpline();
				UpdateFade();
				UpdateVertex();
			}
		}

		public void lateUpdate()
		{
			if (mInited)
			{
				mVertexPool.LateUpdate();
			}
		}

		private void Start()
		{
			Init();
		}

		private void OnDrawGizmos()
		{
			if (!(PointEnd == null) && !(PointStart == null))
			{
				float magnitude = (PointStart.position - PointEnd.position).magnitude;
				if (!(magnitude < float.Epsilon))
				{
					Gizmos.color = Color.red;
					Gizmos.DrawSphere(PointStart.position, magnitude * 0.04f);
					Gizmos.color = Color.blue;
					Gizmos.DrawSphere(PointEnd.position, magnitude * 0.04f);
				}
			}
		}

		private void InitSpline()
		{
			mSpline.Granularity = Granularity;
			mSpline.Clear();
			for (int i = 0; i < MaxFrame; i++)
			{
				mSpline.AddControlPoint(CurHeadPos, PointStart.position - PointEnd.position);
			}
		}

		private void RefreshSpline()
		{
			for (int i = 0; i < mSnapshotList.Count; i++)
			{
				mSpline.ControlPoints[i].Position = mSnapshotList[i].Pos;
				mSpline.ControlPoints[i].Normal = mSnapshotList[i].PointEnd - mSnapshotList[i].PointStart;
			}
			mSpline.RefreshSpline();
		}

		private void UpdateVertex()
		{
			VertexPool pool = mVertexSegment.Pool;
			for (int i = 0; i < Granularity; i++)
			{
				int num = mVertexSegment.VertStart + i * 3;
				float num2 = (float)i / (float)Granularity;
				float tl = num2 * mFadeT;
				Vector2 zero = Vector2.zero;
				Vector3 vector = mSpline.InterpolateByLen(tl);
				Vector3 vector2 = mSpline.InterpolateNormalByLen(tl);
				Vector3 vector3 = vector + vector2.normalized * mTrailWidth * 0.5f;
				Vector3 vector4 = vector - vector2.normalized * mTrailWidth * 0.5f;
				pool.Vertices[num] = vector3;
				pool.Colors[num] = MyColor;
				zero.x = 0f;
				zero.y = num2;
				pool.UVs[num] = zero;
				pool.Vertices[num + 1] = vector;
				pool.Colors[num + 1] = MyColor;
				zero.x = 0.5f;
				zero.y = num2;
				pool.UVs[num + 1] = zero;
				pool.Vertices[num + 2] = vector4;
				pool.Colors[num + 2] = MyColor;
				zero.x = 1f;
				zero.y = num2;
				pool.UVs[num + 2] = zero;
			}
			mVertexSegment.Pool.UVChanged = true;
			mVertexSegment.Pool.VertChanged = true;
			mVertexSegment.Pool.ColorChanged = true;
		}

		private void UpdateIndices()
		{
			VertexPool pool = mVertexSegment.Pool;
			for (int i = 0; i < Granularity - 1; i++)
			{
				int num = mVertexSegment.VertStart + i * 3;
				int num2 = mVertexSegment.VertStart + (i + 1) * 3;
				int num3 = mVertexSegment.IndexStart + i * 12;
				pool.Indices[num3] = num2;
				pool.Indices[num3 + 1] = num2 + 1;
				pool.Indices[num3 + 2] = num;
				pool.Indices[num3 + 3] = num2 + 1;
				pool.Indices[num3 + 4] = num + 1;
				pool.Indices[num3 + 5] = num;
				pool.Indices[num3 + 6] = num2 + 1;
				pool.Indices[num3 + 7] = num2 + 2;
				pool.Indices[num3 + 8] = num + 1;
				pool.Indices[num3 + 9] = num2 + 2;
				pool.Indices[num3 + 10] = num + 2;
				pool.Indices[num3 + 11] = num + 1;
			}
			pool.IndiceChanged = true;
		}

		private void UpdateHeadElem()
		{
			mSnapshotList[0].PointStart = PointStart.position;
			mSnapshotList[0].PointEnd = PointEnd.position;
		}

		private void UpdateFade()
		{
			if (mIsFading)
			{
				mFadeElapsedime += Time.deltaTime;
				float num = mFadeElapsedime / mFadeTime;
				mFadeT = 1f - num;
				if (mFadeT < 0f)
				{
					Deactivate();
				}
			}
		}

		private void RecordCurElem()
		{
			Element item = new Element(PointStart.position, PointEnd.position);
			if (mSnapshotList.Count < MaxFrame)
			{
				mSnapshotList.Insert(1, item);
				return;
			}
			mSnapshotList.RemoveAt(mSnapshotList.Count - 1);
			mSnapshotList.Insert(1, item);
		}

		private void InitOriginalElements()
		{
			mSnapshotList.Clear();
			mSnapshotList.Add(new Element(PointStart.position, PointEnd.position));
			mSnapshotList.Add(new Element(PointStart.position, PointEnd.position));
		}

		private void InitMeshObj()
		{
			mMeshObj = new GameObject("_XWeaponTrailMesh: " + base.gameObject.name);
			mMeshObj.layer = base.gameObject.layer;
			mMeshObj.SetActive(value: true);
			MeshFilter meshFilter = mMeshObj.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = mMeshObj.AddComponent<MeshRenderer>();
			meshRenderer.castShadows = false;
			meshRenderer.receiveShadows = false;
			meshRenderer.renderer.sharedMaterial = MyMaterial;
			meshFilter.sharedMesh = new Mesh();
			mVertexPool = new VertexPool(meshFilter.sharedMesh, MyMaterial);
			mVertexSegment = mVertexPool.GetVertices(Granularity * 3, (Granularity - 1) * 12);
			UpdateIndices();
		}
	}
}
