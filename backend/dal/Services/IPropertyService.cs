using Pims.Dal.Entities.Models;
using System.Collections.Generic;
using View = Pims.Dal.Entities.Views;

namespace Pims.Dal.Services
{
    /// <summary>
    /// IPropertyService interface, provides functions to interact with properties within the datasource.
    /// </summary>
    public interface IPropertyService : IService
    {
        int Count();
        IEnumerable<View.Property> Get(AllPropertyFilter filter);
        IEnumerable<string> GetNames(AllPropertyFilter filter);
        IEnumerable<PropertyModel> Search(AllPropertyFilter filter);
        Paged<View.Property> GetPage(AllPropertyFilter filter);
    }
}
