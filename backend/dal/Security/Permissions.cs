using System.ComponentModel.DataAnnotations;

namespace Pims.Dal.Security
{
    /// <summary>
    /// Permissions enum, provides a list of compile-time safe claim names.
    /// These are fixed claims that will control all access within the application.
    ///
    /// Keycloak
    ///     Keycloak calls theses claims 'roles'.
    ///     Within Keycloak users will be assigned to 'groups', which will be composed of 'roles'.
    /// </summary>
    public enum Permissions
    {
        [Display(GroupName = "admin", Name = "system-administrator", Description = "Can administer application settings.")]
        SystemAdmin = 1,

        [Display(GroupName = "admin", Name = "organization-administrator", Description = "Can administer organizations.")]
        OrganizationAdmin = 2,

        [Display(GroupName = "admin", Name = "admin-users", Description = "Can administer user accounts.")]
        AdminUsers = 3,

        [Display(GroupName = "admin", Name = "admin-roles", Description = "Can administer application roles.")]
        AdminRoles = 4,

        [Display(GroupName = "admin", Name = "admin-organizations", Description = "Can administer application roles.")]
        AdminOrganizations = 5,

        [Display(GroupName = "admin", Name = "admin-properties", Description = "Can administer properties.")]
        AdminProperties = 6,

        [Display(GroupName = "admin", Name = "admin-projects", Description = "Can administer projects.")]
        AdminProjects = 7,

        [Display(GroupName = "property", Name = "property-view", Description = "Can view properties from inventory.")]
        PropertyView = 10,

        [Display(GroupName = "property", Name = "property-add", Description = "Can add new properties to inventory.")]
        PropertyAdd = 11,

        [Display(GroupName = "property", Name = "property-edit", Description = "Can edit properties in inventory.")]
        PropertyEdit = 12,

        [Display(GroupName = "property", Name = "property-delete", Description = "Can delete properties in inventory.")]
        PropertyDelete = 13,

        [Display(GroupName = "property", Name = "sensitive-view", Description = "Can view sensitive properties in inventory.")]
        SensitiveView = 14,

        [Display(GroupName = "contact", Name = "contact-view", Description = "Can view contacts.")]
        ContactView = 15,

        [Display(GroupName = "contact", Name = "contact-add", Description = "Can add new contacts.")]
        ContactAdd = 16,

        [Display(GroupName = "contact", Name = "contact-edit", Description = "Can edit existing contacts.")]
        ContactEdit = 17,

        [Display(GroupName = "contact", Name = "contact-delete", Description = "Can delete existing contacts.")]
        ContactDelete = 18,

        [Display(GroupName = "lease", Name = "lease-view", Description = "Can view leases.")]
        LeaseView = 19,

        [Display(GroupName = "lease", Name = "lease-add", Description = "Can add new leases.")]
        LeaseAdd = 20,

        [Display(GroupName = "lease", Name = "lease-edit", Description = "Can edit existing leases.")]
        LeaseEdit = 21,

        [Display(GroupName = "lease", Name = "lease-delete", Description = "Can delete existing leases.")]
        LeaseDelete = 22,

        [Display(GroupName = "researchfile", Name = "researchfile-view", Description = "Can view research files.")]
        ResearchFileView = 23,

        [Display(GroupName = "researchfile", Name = "researchfile-add", Description = "Can add new research files.")]
        ResearchFileAdd = 24,

        [Display(GroupName = "researchfile", Name = "researchfile-edit", Description = "Can edit existing research files.")]
        ResearchFileEdit = 25,

        [Display(GroupName = "researchfile", Name = "researchfile-delete", Description = "Can delete existing research files.")]
        ResearchFileDelete = 26
    }
}
