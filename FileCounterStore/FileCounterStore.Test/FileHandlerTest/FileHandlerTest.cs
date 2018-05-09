using FileCounterStore.Application;
using FileCounterStore.Application.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace FileCounterStore.Test.FileHandlerTest
{
	[TestClass]
    public class FileHandlerTest
    {
		[TestMethod]
		public void TestIsCSVFile_InvalidFileExtension()
		{
			IFileHandler fileHandler = new FileHandler();
			var test = fileHandler.IsCSVFile("asdasdsa.exe");

			Assert.AreEqual(false, test);
		}

		[TestMethod]
		public void TestIsCSVFile_ValidExtension()
		{
			IFileHandler fileHandler = new FileHandler();
			var test = fileHandler.IsCSVFile("victor.csv");

			Assert.AreEqual(true, test);
		}

		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void TestIsCSVFile_NullParameter()
		{
			IFileHandler fileHandler = new FileHandler();
			var test = fileHandler.IsCSVFile(null);
		}

		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void ReadFileContent_NullParameter()
		{
			IFileHandler fileHandler = new FileHandler();
			var test = fileHandler.ReadFileContent(null);
		}

		[TestMethod]
		public void ReadFileContent_ValidFile()
		{
			var classPath = System.IO.Path.GetFullPath(@"..\..\..\FileHandlerTest");
			var filePath = Path.Combine(classPath, "validCSV.csv");

			IFileHandler fileHandler = new FileHandler();
			var test = fileHandler.ReadFileContent(filePath);

			Assert.AreEqual(test.Count, 5);
		}
	}
}
