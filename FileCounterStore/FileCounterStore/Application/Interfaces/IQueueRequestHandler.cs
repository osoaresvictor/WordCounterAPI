using System;

namespace FileCounterStore.Application.Interfaces
{
	public interface IQueueRequestHandler
	{
		void OnReceiveFilepathInQueue(object sender, EventArgs e);
	}
}