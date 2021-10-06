using System;
using System.IO;
using System.Linq;
using JsonLines;
using RenderHaze.VideoRenderer;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Utf8Json;

namespace Hazy
{
	public static class ProjectProcessor
	{
		public static bool IsValidProject(string path)
		{
			try
			{
				var children = new DirectoryInfo(path).GetDirectories();
				return children.Any(d => d.Name == "tls") && children.Any(d => d.Name == "res");
			}
			catch (IOException) { return false; }
		}

		public static Timeline<TPixel> ParseTimelineFile<TPixel>(string contents, float framerate, string mediaRoot)
			where TPixel : unmanaged, IPixel<TPixel>
		{
			var split    = contents.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
			var media    = split[0];
			var endMs    = float.Parse(split[1]);
			var endFrame = (ulong) (framerate * endMs / 1000);
			var jsonl    = string.Join('\n', split[Range.StartAt(2)]);

			var rawTimingPoints = JsonLinesSerializer.Deserialize<TimingPointJsonType>(jsonl);

			var img          = Image.Load<TPixel>(Path.Combine(mediaRoot, media));
			var timingPoints = rawTimingPoints.Select(t => t.ToTimePoint(framerate, img)).ToArray();

			return new Timeline<TPixel>(new Object<TPixel>(img), endFrame, timingPoints);
		}

		public static Project<TPixel> Parse<TPixel>(string path) where TPixel : unmanaged, IPixel<TPixel>
		{
			if (!IsValidProject(path))
				throw new ProjectInvalidException();

			var children = new DirectoryInfo(path).GetDirectories();
			var media    = children.First(d => d.Name == "res").GetFiles();

			var meta = JsonSerializer.Deserialize<ProjectMeta>(File.ReadAllText(Path.Combine(path, "hazy.json")));

			var timelines = children.First(d => d.Name == "tls")
									.EnumerateFiles()
									.Select(f => ParseTimelineFile<TPixel>(File.ReadAllText(f.FullName),
																		   meta.Framerate,
																		   Path.Combine(path, "res")))
									.ToArray();

			return new Project<TPixel>(timelines, media, meta);
		}

		public static bool TryParse<TPixel>(string path, out Project<TPixel> proj)
			where TPixel : unmanaged, IPixel<TPixel>
		{
			try
			{
				proj = Parse<TPixel>(path);
				return true;
			}
			catch (ProjectProcessingException)
			{
				proj = null;
				return false;
			}
		}
	}
}