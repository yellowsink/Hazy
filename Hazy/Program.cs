using System;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp.PixelFormats;

namespace Hazy
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			if (args.Length == 0 || !Directory.Exists(args[0]))
			{
				Console.WriteLine("Please pass the path to a valid Hazy project");
				Environment.Exit(1);
			}

			var projDir = args[0];
			Console.WriteLine("Reading project...");
			var success = ProjectProcessor.TryParse<Rgba32>(projDir, out var proj);
			if (!success)
			{
				Console.WriteLine("Project load unsuccessful");
				Environment.Exit(2);
			}
			Console.WriteLine(@$"Loaded project ""{proj.Meta.Name}"" @ {proj.Meta.Framerate} fps:
 - {proj.Timelines.Length} timelines
 - {proj.Timelines.Select(t => t.TimePoints.Length).Sum()} timing points
 - {proj.Media.Length} media items");
		}
	}
}