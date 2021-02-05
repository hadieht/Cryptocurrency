using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptocurrency.Domain.Dto
{
	public class CryptoLatestPriceDto
	{
		public string Name { get; set; }
		public string Symbol { get; set; }
		public double Price { get; set; }
	}
}
