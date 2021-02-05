using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptocurrency.Domain.Dto
{
	public class ExchangeRateDto
	{
		public Dictionary<string, double> Rates { get; set; }

		public string Base { get; set; }
	}
}
