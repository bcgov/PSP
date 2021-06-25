using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// NotificationTemplate class, provides an entity for the datamodel to manage a notification templates.
    /// </summary>
    [MotiTable("PIMS_NOTIFICATION_TEMPLATE", "NTTMPL")]
    public class NotificationTemplate : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key unique identity for notification template.
        /// </summary>
        [Column("NOTIFICATION_TEMPLATE_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - A unique name to identify the template.
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of the template.
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// get/set - A semi-colon separated list of email addresses this notification will be sent to.
        /// </summary>
        [Column("TO")]
        public string To { get; set; }

        /// <summary>
        /// get/set - A semi-colon separated list of email addresses this notification will be carbon-copied to.
        /// </summary>
        [Column("CC")]
        public string Cc { get; set; }

        /// <summary>
        /// get/set - A semi-colon separated list of email addresses this notification will be blind carbon-copied to.
        /// </summary>
        [Column("BCC")]
        public string Bcc { get; set; }

        /// <summary>
        /// get/set - The audience for this notification template.
        /// </summary>
        [Column("AUDIENCE")]
        public NotificationAudiences Audience { get; set; } = NotificationAudiences.Default;

        /// <summary>
        /// get/set - The notification encoding [base64, binary, hex, utf-8].
        /// </summary>
        [Column("ENCODING")]
        public NotificationEncodings Encoding { get; set; } = NotificationEncodings.Utf8;

        /// <summary>
        /// get/set - The notification body type [html, text].
        /// </summary>
        [Column("BODY_TYPE")]
        public NotificationBodyTypes BodyType { get; set; } = NotificationBodyTypes.Html;

        /// <summary>
        /// get/set - The notification priority [low, normal, high]
        /// </summary>
        [Column("PRIORITY")]
        public NotificationPriorities Priority { get; set; } = NotificationPriorities.Normal;

        /// <summary>
        /// get/set - The subject line of the notification (supports variables).
        /// </summary>
        [Column("SUBJECT")]
        public string Subject { get; set; }

        /// <summary>
        /// get/set - The body of the notification (supports variables).
        /// </summary>
        [Column("BODY")]
        public string Body { get; set; }

        /// <summary>
        /// get/set - Whether this template is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - A way to group notifications within CHES.
        /// </summary>
        [Column("TAG")]
        public string Tag { get; set; }

        /// <summary>
        /// get - A collection of notifications in the queue that used this template.
        /// </summary>
        public ICollection<NotificationQueue> Notifications { get; } = new List<NotificationQueue>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a NotificationTemplate class.
        /// </summary>
        public NotificationTemplate() { }

        /// <summary>
        /// Create a new instance of a NotificationTemplate class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public NotificationTemplate(string name, string subject, string body)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.", nameof(name));

            this.Name = name;
            this.Subject = subject;
            this.Body = body;
        }

        /// <summary>
        /// Create a new instance of a NotificationTemplate class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="encoding"></param>
        /// <param name="bodyType"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public NotificationTemplate(string name, NotificationEncodings encoding, NotificationBodyTypes bodyType, string subject, string body)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.", nameof(name));

            this.Name = name;
            this.Encoding = encoding;
            this.BodyType = bodyType;
            this.Subject = subject;
            this.Body = body;
        }
        #endregion
    }
}
