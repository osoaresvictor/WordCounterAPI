using FileCounterStore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileCounterStore.Application
{
	public class FileHandler : IFileHandler
	{
		public bool IsCSVFile(string filepath) => filepath.Substring(filepath.Length - 3, 3) == "csv";

		public List<string> ReadFileContent(string fileName)
		{
			var fileContent = default(StreamReader);
			try
			{
				fileContent = File.OpenText(fileName);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + " -  " + ex.StackTrace);
			}

			var line = fileContent.ReadLine()?.Split(new char[] { ',', ';', ' ' });
			var lines = new List<string>();

			while (line?.Length > 0)
			{
				lines.AddRange(line);
				line = fileContent.ReadLine()?.Split(new char[] { ',', ';', ' ' });
			}

			return lines;
		}
	}
}
