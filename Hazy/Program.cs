using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using RenderHaze.VideoRenderer;
using SixLabors.ImageSharp.PixelFormats;

namespace Hazy
{
	internal static class Program
	{
		private const string ClearLine = "\u001b[0K";
	
		private static void Main(string[] args)
		{
			if (args.Length < 2 || !Directory.Exists(args[0]))
			{
				Console.WriteLine("Please pass the path to a valid Hazy project");
				Environment.Exit(1);
			}

			var projDir = args[0];
			var success = ProjectProcessor.TryParse<Rgba32>(projDir, out var proj);
			if (!success)
			{
				Console.WriteLine("Project was invalid, and failed to load");
				Environment.Exit(2);
			}

			Console.WriteLine(@$"Loaded project ""{proj.Meta.Name}"" @ {proj.Meta.Framerate} fps:
 - {proj.Timelines.Length} timelines
 - {proj.Timelines.Select(t => t.TimePoints.Length).Sum()} timing points
 - {proj.Media.Length} media items");

			Console.WriteLine($"Starting render to: {args[1]}");
			var renderer = proj.RenderSupervisor;
			var sw       = Stopwatch.StartNew();
			renderer.RenderVideoTo(args[1], null,
								   (_, r) =>
								   {
									   Console.CursorLeft = 0;
									   Console.Write(r.Type == RenderProgressType.Encoding
														 ? ClearLine + "Encoding video..."
														 : $"Rendering... {r.Completed}/{r.Total} ({100 * r.Completed / r.Total}%)");
								   });
			sw.Stop();
			Console.CursorLeft = 0;
			Console.WriteLine($"Render completed in {sw.Elapsed.TotalSeconds} seconds");
		}
	}
}