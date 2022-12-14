using System;

namespace Pims.Tools.Core.Keycloak.Models
{
    /// <summary>
    /// RealmModel class, provides a way to represent a Keycloak realm.
    /// </summary>
    public class RealmModel
    {
        #region Properties
        public int Id { get; set; }

        public string ProjectName { get; set; }

        public string AuthType { get; set; }

        public string[] Environments { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        #endregion
    }
}
