using System;

namespace Pims.Dal.Entities
{
    public abstract class IdentityBaseAppEntity<T>
    { 
        public abstract T Id { get; set; }
    }
}
