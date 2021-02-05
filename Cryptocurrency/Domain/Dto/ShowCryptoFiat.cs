using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptocurrency.Domain.Dto
{
	public class ShowCryptoFiat
	{
		public Dictionary<string,decimal> Fiats { get; set; }
	}
}
