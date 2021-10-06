using System.IO;
using RenderHaze.VideoRenderer;
using SixLabors.ImageSharp.PixelFormats;

namespace Hazy
{
	public class Project<TPixel> where TPixel : unmanaged, IPixel<TPixel>
	{
		public FileInfo[]         Media;
		public Timeline<TPixel>[] Timelines;
		public ProjectMeta        Meta;

		public Project(Timeline<TPixel>[] timelines, FileInfo[] media, ProjectMeta meta)
		{
			Media     = media;
			Timelines = timelines;
			Meta      = meta;
		}
	}

	public class ProjectMeta
	{
		public string Name;
		public float  Framerate;
	}
}