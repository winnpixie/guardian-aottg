using System.IO;
using UnityEngine;

public class UVAnimation
{
	public Vector2[] frames;

	public Vector2[] UVDimensions;

	public int curFrame;

	protected int stepDir = 1;

	protected int numLoops;

	public string name;

	public int loopCycles;

	public bool loopReverse;

	public void Reset()
	{
		curFrame = 0;
		stepDir = 1;
		numLoops = 0;
	}

	public void PlayInReverse()
	{
		stepDir = -1;
		curFrame = frames.Length - 1;
	}

	public bool GetNextFrame(ref Vector2 uv, ref Vector2 dm)
	{
		if (curFrame + stepDir >= frames.Length || curFrame + stepDir < 0)
		{
			if (stepDir > 0 && loopReverse)
			{
				stepDir = -1;
				curFrame += stepDir;
				uv = frames[curFrame];
				dm = UVDimensions[curFrame];
			}
			else
			{
				if (numLoops + 1 > loopCycles && loopCycles != -1)
				{
					return false;
				}
				numLoops++;
				if (loopReverse)
				{
					stepDir *= -1;
					curFrame += stepDir;
				}
				else
				{
					curFrame = 0;
				}
				uv = frames[curFrame];
				dm = UVDimensions[curFrame];
			}
		}
		else
		{
			curFrame += stepDir;
			uv = frames[curFrame];
			dm = UVDimensions[curFrame];
		}
		return true;
	}

	public void BuildFromFile(string path, int index, float uvTime, Texture mainTex)
	{
		if (!File.Exists(path))
		{
			Debug.LogError("wrong ean file path!");
			return;
		}
		FileStream fileStream = new FileStream(path, FileMode.Open);
		BinaryReader br = new BinaryReader(fileStream);
		EanFile eanFile = new EanFile();
		eanFile.Load(br, fileStream);
		fileStream.Close();
		EanAnimation eanAnimation = eanFile.Anims[index];
		frames = new Vector2[eanAnimation.TotalCount];
		UVDimensions = new Vector2[eanAnimation.TotalCount];
		int tileCount = eanAnimation.TileCount;
		int num = (eanAnimation.TotalCount + tileCount - 1) / tileCount;
		int num2 = 0;
		int width = mainTex.width;
		int height = mainTex.height;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < tileCount; j++)
			{
				if (num2 >= eanAnimation.TotalCount)
				{
					break;
				}
				Vector2 zero = Vector2.zero;
				zero.x = (float)(int)eanAnimation.Frames[num2].Width / (float)width;
				zero.y = (float)(int)eanAnimation.Frames[num2].Height / (float)height;
				frames[num2].x = (float)(int)eanAnimation.Frames[num2].X / (float)width;
				frames[num2].y = 1f - (float)(int)eanAnimation.Frames[num2].Y / (float)height;
				UVDimensions[num2] = zero;
				UVDimensions[num2].y = 0f - UVDimensions[num2].y;
				num2++;
			}
		}
	}

	public Vector2[] BuildUVAnim(Vector2 start, Vector2 cellSize, int cols, int rows, int totalCells)
	{
		int num = 0;
		frames = new Vector2[totalCells];
		UVDimensions = new Vector2[totalCells];
		frames[0] = start;
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				if (num >= totalCells)
				{
					break;
				}
				frames[num].x = start.x + cellSize.x * (float)j;
				frames[num].y = start.y - cellSize.y * (float)i;
				UVDimensions[num] = cellSize;
				UVDimensions[num].y = 0f - UVDimensions[num].y;
				num++;
			}
		}
		return frames;
	}

	public void SetAnim(Vector2[] anim)
	{
		frames = anim;
	}
}
