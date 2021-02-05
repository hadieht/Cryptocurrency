using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptocurrency.Domain.AppConfig
{
	public class CoinMarketCapSetting
	{
		public string ApiKey { get; set; }
		public string MapApiUrl { get; set; }
		public string LatestPriceApiUrl { get; set; }

	}
}
