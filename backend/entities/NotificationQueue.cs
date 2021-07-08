using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// NotificationQueue class, provides an entity for the datamodel to manage a notification queue.
    /// </summary>
    [MotiTable("PIMS_NOTIFICATION_QUEUE", "NOTIFQ")]
    public class NotificationQueue : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key unique identity for notification template.
        /// </summary>
        [Column("NOTIFICATION_QUEUE_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this notification when recipients respond.
        /// </summary>
        [Column("KEY")]
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - The status of the notification [accepted, pending, cancelled, failed, completed].
        /// </summary>
        [Column("STATUS")]
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;

        /// <summary>
        /// get/set - The priority of the notification [low, normal, high].
        /// </summary>
        [Column("PRIORITY")]
        public NotificationPriorities Priority { get; set; } = NotificationPriorities.Normal;

        /// <summary>
        /// get/set - The notification encoding [base64, binary, hex, utf-8].
        /// </summary>
        [Column("ENCODING")]
        public NotificationEncodings Encoding { get; set; } = NotificationEncodings.Utf8;

        /// <summary>
        /// get/set - The date the notification should be sent on.
        /// </summary>
        [Column("SEND_ON")]
        public DateTime SendOn { get; set; }

        /// <summary>
        /// get/set - Semi-colon separated list of email addresses that the notification will be sent to.
        /// </summary>
        [Column("TO")]
        public string To { get; set; }

        /// <summary>
        /// get/set - Semi-colon separated list of email addresses that the notification will be blind-copied to.
        /// </summary>
        [Column("BCC")]
        public string Bcc { get; set; }

        /// <summary>
        /// get/set - Semi-colon separated list of email addresses that the notification will be carbon-copied to.
        /// </summary>
        [Column("CC")]
        public string Cc { get; set; }

        /// <summary>
        /// get/set - The notification subject line.
        /// </summary>
        [Column("SUBJECT")]
        public string Subject { get; set; }

        /// <summary>
        /// get/set - The body type of the notification [html, text].
        /// </summary>
        [Column("BODY_TYPE")]
        public NotificationBodyTypes BodyType { get; set; } = NotificationBodyTypes.Html;

        /// <summary>
        /// get/set - The notification body message.
        /// </summary>
        [Column("BODY")]
        public string Body { get; set; }

        /// <summary>
        /// get/set - A tag to group related notifications.
        /// </summary>
        [Column("TAG")]
        public string Tag { get; set; }

        /// <summary>
        /// get/set - Foreign key to the agency this notification was sent to.
        /// </summary>
        [Column("TO_AGENCY_ID")]
        public long? ToAgencyId { get; set; }

        /// <summary>
        /// get/set - The agency this notification was sent to.
        /// </summary>
        public Agency ToAgency { get; set; }

        /// <summary>
        /// get/set - Foreign key to the template that generated this notification.
        /// </summary>
        [Column("NOTIFICATION_TEMPLATE_ID")]
        public long? TemplateId { get; set; }

        /// <summary>
        /// get/set - The notification template that generated this notification.
        /// </summary>
        public NotificationTemplate Template { get; set; }

        /// <summary>
        /// get/set - CHES message Id.
        /// </summary>
        [Column("CHES_MESSAGE_ID")]
        public Guid? ChesMessageId { get; set; }

        /// <summary>
        /// get/set - CHES transaction Id.
        /// </summary>
        [Column("CHES_TRANSACTION_ID")]
        public Guid? ChesTransactionId { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a NotificationQueue class.
        /// </summary>
        public NotificationQueue() { }

        /// <summary>
        /// Create a new instance of a NotificationQueue class, initializes with specified parameters.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public NotificationQueue(NotificationTemplate template, string to, string subject, string body)
        {
            if (String.IsNullOrWhiteSpace(to)) throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.", nameof(to));
            if (String.IsNullOrWhiteSpace(subject)) throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.", nameof(subject));
            if (String.IsNullOrWhiteSpace(body)) throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.", nameof(body));

            this.Key = Guid.NewGuid();
            this.TemplateId = template?.Id ?? throw new ArgumentNullException(nameof(template));
            this.Template = template;
            this.Subject = subject;
            this.Body = body;
            this.BodyType = template.BodyType;
            this.Encoding = template.Encoding;
            this.Priority = template.Priority;
            this.To = to;
            this.SendOn = DateTime.UtcNow;
            this.Tag = template.Tag;
        }

        /// <summary>
        /// Create a new instance of a NotificationQueue class, initializes with specified parameters.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="toAgency"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public NotificationQueue(NotificationTemplate template, Agency toAgency, string subject, string body)
        {
            if (String.IsNullOrWhiteSpace(subject)) throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.", nameof(subject));
            if (String.IsNullOrWhiteSpace(body)) throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.", nameof(body));

            this.Key = Guid.NewGuid();
            this.TemplateId = template?.Id ?? throw new ArgumentNullException(nameof(template));
            this.Template = template;
            this.Subject = subject;
            this.Body = body;
            this.BodyType = template.BodyType;
            this.Encoding = template.Encoding;
            this.Priority = template.Priority;
            this.ToAgencyId = toAgency?.Id;
            this.ToAgency = toAgency;
            this.To = toAgency?.Email;
            this.SendOn = DateTime.UtcNow;
            this.Tag = template.Tag;
        }

        /// <summary>
        /// Create a new instance of a NotificationQueue class, initializes with specified parameters.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public NotificationQueue(NotificationTemplate template, string to, string cc, string bcc, string subject, string body)
        {
            if (String.IsNullOrWhiteSpace(to)) throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.", nameof(to));
            if (String.IsNullOrWhiteSpace(subject)) throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.", nameof(subject));
            if (String.IsNullOrWhiteSpace(body)) throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.", nameof(body));

            this.Key = Guid.NewGuid();
            this.TemplateId = template?.Id ?? throw new ArgumentNullException(nameof(template));
            this.Template = template;
            this.To = to;
            this.Cc = cc;
            this.Bcc = bcc;
            this.Subject = subject;
            this.Body = body;
            this.BodyType = template.BodyType;
            this.Encoding = template.Encoding;
            this.Priority = template.Priority;
            this.SendOn = DateTime.UtcNow;
            this.Tag = template.Tag;
        }
        #endregion
    }
}
