using System;
using RenderHaze.VideoRenderer;
using SixLabors.ImageSharp;

namespace Hazy
{
	public class TimingPointJsonType
	{
		public HorizontalAlignment HAlign  = HorizontalAlignment.Left;
		public float               Opacity = 1;
		public float               TimingMs;
		public VerticalAlignment   VAlign = VerticalAlignment.Top;
		public int                 X;
		public int                 Y;

		public TimePoint ToTimePoint(float framerate, Image img)
		{
			var frameNum  = (ulong) (framerate * TimingMs / 1000);

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