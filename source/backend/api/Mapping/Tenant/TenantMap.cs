using System.Text.Json;
using Mapster;
using Microsoft.Extensions.Options;
using Pims.Dal;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Tenant;

namespace Pims.Api.Mapping.Tenant
{
    public class TenantMap : IRegister
    {
        #region Variables
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly PimsOptions _pimsOptions;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a TenantMap, initializes with specified arguments.
        /// </summary>
        /// <param name="serializerOptions"></param>
        /// <param name="pimsOptions"></param>
        public TenantMap(IOptions<JsonSerializerOptions> serializerOptions, IOptions<PimsOptions> pimsOptions)
        {
            _serializerOptions = serializerOptions.Value;
            _pimsOptions = pimsOptions.Value;
        }
        #endregion

        #region Methods
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsTenant, Model.TenantModel>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Name, src => src.Name)
                .AfterMapping((src, dest) =>
                {
                    dest.Settings = JsonSerializer.Deserialize<Model.TenantSettingsModel>(src.Settings ?? "{}", _serializerOptions);

                    // Override if in environmental configuration.
                    dest.Settings.HelpDeskEmail = _pimsOptions.HelpDeskEmail ?? dest.Settings.HelpDeskEmail;
                });

            config.NewConfig<Model.TenantModel, Entity.PimsTenant>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Name, src => src.Name)
                .AfterMapping((src, dest) =>
                {
                    dest.Settings = JsonSerializer.Serialize(src.Settings, _serializerOptions);
                });
        }
        #endregion
    }
}
