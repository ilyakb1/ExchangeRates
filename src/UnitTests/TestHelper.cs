﻿using System;
using System.IO;
using System.Reflection;

namespace ExchangeRateService.UnitTests
{
	public class TestHelper
	{
		public static Stream GetEmbeddedResource(string resourceName)
		{
			var fullResourceName = Assembly.GetExecutingAssembly().GetName().Name + '.' + resourceName;
			var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullResourceName);
			if (resource == null)
			{
				throw new Exception(string.Format("Could not locate embedded resource '{0}'", fullResourceName));
			}

			return resource;
		}

		public static string GetEmbeddedResourceAsString(string resourceName)
		{
			using (var stream = GetEmbeddedResource(resourceName))
			{
				return new StreamReader(stream).ReadToEnd();
			}
		}

		public static byte[] GetEmbeddedResourceAsByteArray(string resourceName)
		{
			var buffer = new byte[16 * 1024];

			using (var stream = GetEmbeddedResource(resourceName))
			{
				using (var memoryStream = new MemoryStream())
				{
					int read;
					while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
					{
						memoryStream.Write(buffer, 0, read);
					}

					return memoryStream.ToArray();
				}
			}
		}

		public static bool CompareStreams(Stream expectedStream, MemoryStream actualStream)
		{
			expectedStream.Position = 0;
			actualStream.Position = 0;

			if (expectedStream.Length != actualStream.Length)
			{
				return false;
			}

			while (true)
			{
				var expectedByte = expectedStream.ReadByte();
				var actualByte = actualStream.ReadByte();

				if (expectedByte != actualByte)
				{
					return false;
				}

				if (expectedByte == -1 && actualByte == -1)
				{
					return true;
				}
			}
		}

		public static string StreamToString(byte[] data)
		{
			using (var stream = new MemoryStream(data))
			{
				return new StreamReader(stream).ReadToEnd();
			}
		}
	}

}
