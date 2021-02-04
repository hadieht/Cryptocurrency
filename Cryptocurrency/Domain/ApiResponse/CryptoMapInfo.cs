using System.Collections.Generic;

namespace Cryptocurrency.Domain.ApiResponse
{
	public class CryptoMapInfo
	{
		public IEnumerable<CrypoData> Data { get; set; }
	}
	public class CrypoData
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Symbol { get; set; }
		public int Rank { get; set; }
	}
	
}
