using System.IO;
using RenderHaze.VideoRenderer;
using SixLabors.ImageSharp.PixelFormats;

namespace Hazy
{
	public class Project<TPixel> where TPixel : unmanaged, IPixel<TPixel>
	{
		public FileInfo[]         Media;
		public ProjectMeta        Meta;
		public Timeline<TPixel>[] Timelines;

		public Project(Timeline<TPixel>[] timelines, FileInfo[] media, ProjectMeta meta)
		{
			Media     = media;
			Timelines = timelines;
			Meta      = meta;
		}
	}

	public class ProjectMeta
	{
		public float  Framerate;
		public string Name;
	}
}