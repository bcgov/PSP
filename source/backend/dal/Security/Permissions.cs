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
        ResearchFileDelete = 26,

        [Display(GroupName = "acquisitionfile", Name = "acquisitionfile-view", Description = "Can view acquisition files.")]
        AcquisitionFileView = 27,

        [Display(GroupName = "acquisitionfile", Name = "acquisitionfile-add", Description = "Can add new acquisition files.")]
        AcquisitionFileAdd = 28,

        [Display(GroupName = "acquisitionfile", Name = "acquisitionfile-edit", Description = "Can edit existing acquisition files.")]
        AcquisitionFileEdit = 29,

        [Display(GroupName = "acquisitionfile", Name = "acquisitionfile-delete", Description = "Can delete existing acquisition files.")]
        AcquisitionFileDelete = 30,

        [Display(GroupName = "note", Name = "note-view", Description = "Can view notes.")]
        NoteView = 31,

        [Display(GroupName = "note", Name = "note-add", Description = "Can add new notes.")]
        NoteAdd = 32,

        [Display(GroupName = "note", Name = "note-edit", Description = "Can update existing notes.")]
        NoteEdit = 33,

        [Display(GroupName = "note", Name = "note-delete", Description = "Can delete existing notes.")]
        NoteDelete = 34,

        [Display(GroupName = "document", Name = "document-view", Description = "Can view documents.")]
        DocumentView = 35,

        [Display(GroupName = "document", Name = "document-add", Description = "Can add new documents.")]
        DocumentAdd = 36,

        [Display(GroupName = "document", Name = "document-edit", Description = "Can update existing documents.")]
        DocumentEdit = 37,

        [Display(GroupName = "document", Name = "document-delete", Description = "Can delete existing documents.")]
        DocumentDelete = 38,

        [Display(GroupName = "document", Name = "document-admin", Description = "Can perform admin functions on documents.")]
        DocumentAdmin = 39,

        [Display(GroupName = "document", Name = "generate-documents", Description = "Can generate documents.")]
        GenerateDocuments = 40,

        [Display(GroupName = "activity", Name = "activity-view", Description = "Can view activities.")]
        ActivityView = 41,

        [Display(GroupName = "activity", Name = "activity-add", Description = "Can add new activities.")]
        ActivityAdd = 42,

        [Display(GroupName = "activity", Name = "activity-edit", Description = "Can update existing activities.")]
        ActivityEdit = 43,

        [Display(GroupName = "activity", Name = "activity-delete", Description = "Can delete existing activities.")]
        ActivityDelete = 44,

        [Display(GroupName = "project", Name = "project-view", Description = "Can view projects.")]
        ProjectView = 45,

        [Display(GroupName = "project", Name = "project-add", Description = "Can add new projects.")]
        ProjectAdd = 46,

        [Display(GroupName = "project", Name = "project-edit", Description = "Can update existing projects.")]
        ProjectEdit = 47,

        [Display(GroupName = "project", Name = "project-delete", Description = "Can delete existing projects.")]
        ProjectDelete = 48,

        [Display(GroupName = "form", Name = "form-view", Description = "Can view file forms.")]
        FileView = 49,

        [Display(GroupName = "form", Name = "form-add", Description = "Can add new forms.")]
        FileAdd = 50,

        [Display(GroupName = "form", Name = "form-edit", Description = "Can update existing file forms.")]
        FileEdit = 51,

        [Display(GroupName = "form", Name = "form-delete", Description = "Can delete existing file forms.")]
        FileDelete = 52,
    }
}
