using Mapster;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Tenant;

namespace Pims.Api.Mapping.Tenant
{
    public class TenantMap : IRegister
    {
        #region Variables
        private readonly JsonSerializerOptions _serializerOptions;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TenantMap, initializes with specified arguments.
        /// </summary>
        /// <param name="serializerOptions"></param>
        public TenantMap(IOptions<JsonSerializerOptions> serializerOptions)
        {
            _serializerOptions = serializerOptions.Value;
        }
        #endregion

        #region Methods
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Tenant, Model.TenantModel>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Name, src => src.Name)
                .AfterMapping((src, dest) =>
                {
                    dest.Settings = JsonSerializer.Deserialize<Model.TenantSettingsModel>(src.Settings ?? "{}", _serializerOptions);
                })
                .Inherits<Entity.BaseEntity, Models.BaseModel>();

            config.NewConfig<Model.TenantModel, Entity.Tenant>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Name, src => src.Name)
                .AfterMapping((src, dest) =>
                {
                    dest.Settings = JsonSerializer.Serialize(src.Settings, _serializerOptions);
                })
                .Inherits<Models.BaseModel, Entity.BaseEntity>();
        }
        #endregion
    }
}
