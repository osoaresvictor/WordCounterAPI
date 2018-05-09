using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WordSearcher.Repository.RabbitMQ.Interfaces;

namespace WordSearcher.Controllers
{
	[Route("upload")]
	public class UploadController : Controller
	{
		public IRabbitProducer RabbitMQProducer { get; set; }

		public UploadController(IRabbitProducer rabbitMQProducer)
		{
			this.RabbitMQProducer = rabbitMQProducer;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return View("Upload");
		}

		[HttpPost("UploadFiles")]
		public async Task<IActionResult> Post(List<IFormFile> files)
		{
			var size = files?.Sum(f => f.Length);

			if (files == null || files?.Count == 0) return BadRequest("No file selected to Upload!");
			if (size >= 10000000) return BadRequest("File size larger than allowed (> 10MB)!");

			var rabbitMQHost = Environment.GetEnvironmentVariable("RabbitMQHost");
			var rabbitMQPort = int.Parse(Environment.GetEnvironmentVariable("RabbitMQPort"));
			var queueName = Environment.GetEnvironmentVariable("QueueName");

			foreach (var file in files)
			{
				if (file.FileName.Substring(file.FileName.Length - 3, 3) != "csv")
				{
					ViewBag.Message = "SELECIONE APENAS ARQUIVOS CSV PARA ENVIO!";
					return View("Upload");
				}

				if (file.Length > 0)
				{
					var filePath = $"/repository/{file.FileName}";

					try
					{
						var stream = new FileStream(filePath, FileMode.Create);
						await file.CopyToAsync(stream);
						stream.Dispose();
					}
					catch
					{
						throw;
					}

					this.RabbitMQProducer.SendMessage(rabbitMQHost, rabbitMQPort, queueName, filePath);
				}
			}

			return Ok(new { count = files.Count, totalSize = size, files});
		}
	}
}
