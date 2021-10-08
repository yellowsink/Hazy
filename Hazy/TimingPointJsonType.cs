using RenderHaze.ImageRenderer;
using RenderHaze.VideoRenderer;

namespace Hazy
{
	public class TimingPointJsonType
	{
		public OriginPoint PosOrigin   = OriginPoint.TopLeft;
		public OriginPoint ScaleOrigin = OriginPoint.TopLeft;
		public float       Opacity     = 1;
		public float       TimingMs;
		public int         X;
		public int         Y;
		public float       Sx;
		public float       Sy;

		public TimePoint ToTimePoint(float framerate)
		{
			var frameNum = (ulong) (framerate * TimingMs / 1000);

			return new TimePoint(frameNum, X, Y, Opacity, Sx, Sy, PosOrigin, ScaleOrigin);
		}
	}
}