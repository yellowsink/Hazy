using System;
using RenderHaze.VideoRenderer;
using SixLabors.ImageSharp;

namespace Hazy
{
	public class TimingPointJsonType
	{
		public HorizontalAlignment HAlign;
		public float               Opacity;
		public float               TimingMs;
		public VerticalAlignment   VAlign;
		public int                 X;
		public int                 Y;

		public TimePoint ToTimePoint(float framerate, Image img)
		{
			var frameTime = 1.0 / framerate;
			var frameNum  = (ulong) (TimingMs / frameTime);

			var x = HAlign switch
			{
				HorizontalAlignment.Left   => X,
				HorizontalAlignment.Center => X - img.Width / 2,
				HorizontalAlignment.Right  => X - img.Width,
				_                          => throw new ArgumentOutOfRangeException()
			};

			var y = VAlign switch
			{
				VerticalAlignment.Top    => Y,
				VerticalAlignment.Center => Y - img.Height / 2,
				VerticalAlignment.Bottom => Y - img.Height,
				_                        => throw new ArgumentOutOfRangeException()
			};

			return new TimePoint(frameNum, x, y, Opacity);
		}
	}

	public enum HorizontalAlignment
	{
		Left,
		Center,
		Right
	}

	public enum VerticalAlignment
	{
		Top,
		Center,
		Bottom
	}
}