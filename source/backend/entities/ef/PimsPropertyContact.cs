using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Defines the contacts that are associated with this property.
/// </summary>
[Table("PIMS_PROPERTY_CONTACT")]
[Index("OrganizationId", Name = "PRPCNT_ORGANIZATION_ID_IDX")]
[Index("PersonId", Name = "PRPCNT_PERSON_ID_IDX")]
[Index("PropertyId", Name = "PRPCNT_PROPERTY_ID_IDX")]
public partial class PimsPropertyContact
{
    [Key]
    [Column("PROPERTY_CONTACT_ID")]
    public long PropertyContactId { get; set; }

    /// <summary>
    /// Primary key of the associated property.
    /// </summary>
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    /// <summary>
    /// Person ID of the property contact.
    /// </summary>
    [Column("PERSON_ID")]
    public long? PersonId { get; set; }

    /// <summary>
    /// Organization ID of the property contact.
    /// </summary>
    [Column("ORGANIZATION_ID")]
    public long? OrganizationId { get; set; }

    /// <summary>
    /// Primary contact for the organization
    /// </summary>
    [Column("PRIMARY_CONTACT_ID")]
    public long? PrimaryContactId { get; set; }

    /// <summary>
    /// Purpose of property contact
    /// </summary>
    [Required]
    [Column("PURPOSE")]
    [StringLength(500)]
    public string Purpose { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long? ConcurrencyControlNumber { get; set; }

    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

    [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbCreateTimestamp { get; set; }

    [Required]
    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    [Required]
    [Column("DB_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string DbLastUpdateUserid { get; set; }

    [ForeignKey("OrganizationId")]
    [InverseProperty("PimsPropertyContacts")]
    public virtual PimsOrganization Organization { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PimsPropertyContactPeople")]
    public virtual PimsPerson Person { get; set; }

    [ForeignKey("PrimaryContactId")]
    [InverseProperty("PimsPropertyContactPrimaryContacts")]
    public virtual PimsPerson PrimaryContact { get; set; }

    [ForeignKey("PropertyId")]
    [InverseProperty("PimsPropertyContacts")]
    public virtual PimsProperty Property { get; set; }
}
