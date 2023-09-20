namespace Pims.Dal.Repositories
{
    using System.Collections.Generic;
    using Pims.Dal.Entities;

    /// <summary>
    /// IPropertyContactRepository interface, provides functions to interact with property contacts within the datasource.
    /// </summary>
    public interface IPropertyContactRepository : IRepository<PimsPropertyContact>
    {
        IList<PimsPropertyContact> GetContactsByProperty(long propertyId);

        PimsPropertyContact Create(PimsPropertyContact propertyContact);

        PimsPropertyContact Update(PimsPropertyContact propertyContact);

        void Delete(long propertyContactId);
    }
}
