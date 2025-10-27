using System;
using SerialPortService;
using Microsoft.AspNetCore.Mvc;

namespace SerialPortService.Controller
{
	[ApiController]
	[Route("api/[controller]")]
	public class SerialPortController : ControllerBase
	{
		private readonly SerialPortService _serial;

		public SerialPortController(SerialPortService serial)
		{
			_serial = serial;
		}

		[HttpPost("connect")]
		public IActionResult Connect()
		{
			_serial.Connect();
			return Ok("Serial port connected");
		}

		[HttpPost("disconnect")]
		public IActionResult Disconnect()
		{
			_serial.Disconnect();
			return Ok("Serial port disconnected");
		}
	}
}
