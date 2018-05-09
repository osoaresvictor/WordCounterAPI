using System.Collections.Generic;

namespace FileCounterStore.Application.Interfaces
{
	public interface IFileHandler
	{
		bool IsCSVFile(string filepath);
		List<string> ReadFileContent(string fileName);
	}
}