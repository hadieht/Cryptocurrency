using Cryptocurrency.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using Cryptocurrency.Domain.ApiResponse;

namespace Cryptocurrency.Domain.Mapping
{
	public static class Mapper
	{

		public static ExchangeRateDto ToDto(this ExchangeratesApiResponse sourceObject)
		{
			return sourceObject.Adapt<ExchangeRateDto>();
		}


		public static CryptoNameDto ToDto(this CrypoData sourceObject)
		{
			return sourceObject.Adapt<CryptoNameDto>();
		}

		public static List<CryptoNameDto> ToDtoList(this CryptoMapResponse sourceObject)
		{
			var config = new TypeAdapterConfig();

			config
						.NewConfig<CryptoMapResponse, CryptoNameDto>()
						.Map(a => a, d => d.Data.Select(a => a.ToDto()));

			var destObject = sourceObject.Adapt<List<CryptoNameDto>>(config);

			return destObject;
		}
	}
}
