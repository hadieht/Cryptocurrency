using Cryptocurrency.Domain.ApiResponse;
using Cryptocurrency.Domain.Dto;
using Mapster;
using System.Collections.Generic;
using System.Linq;

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
