using RenderHaze.ImageRenderer;
using RenderHaze.VideoRenderer;

namespace Hazy
{
	public class TimingPointJsonType
	{
		public OriginPoint PosOrigin   { get; set; } = OriginPoint.TopLeft;
		public OriginPoint ScaleOrigin { get; set; } = OriginPoint.TopLeft;
		public float       Opacity     { get; set; } = 1;
		public float       TimingMs    { get; set; }
		public int         X           { get; set; }
		public int         Y           { get; set; }
		public float       Sx          { get; set; } = 1;
		public float       Sy          { get; set; } = 1;

		public TimePoint ToTimePoint(float framerate)
		{
			var frameNum = (ulong) (framerate * TimingMs / 1000);

			return new TimePoint(frameNum, X, Y, Opacity, Sx, Sy, PosOrigin, ScaleOrigin);
		}
	}
}