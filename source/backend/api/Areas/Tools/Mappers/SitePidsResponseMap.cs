using System;
using System.Collections.Generic;
using Mapster;
using Pims.Api.Models.Requests.Geocoder;
using Pims.Geocoder.Models;

namespace Pims.Api.Areas.Tools.Mappers
{
    /// <summary>
    /// PidsMap class, maps the model properties.
    /// </summary>
    public class SitePidsResponseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SitePidsResponseModel, SitePidsResponse>()
                .Map(dest => dest.SiteId, src => src.SiteID)
                .Map(dest => dest.Pids, src => StringToList(src.Pids));
        }

        private static IEnumerable<string> StringToList(string delimSeperated)
        {
            return delimSeperated != null ? delimSeperated.Split('|', ',') : Array.Empty<string>();
        }
    }
}
