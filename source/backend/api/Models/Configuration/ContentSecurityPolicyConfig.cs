namespace Pims.Api.Models.Config
{
    /// <summary>
    /// Model that provides CSP (Content Security Policy) header configuration.
    /// </summary>
    public class ContentSecurityPolicyConfig
    {
        /// <summary>
        /// get/set - The 'base-uri' attribute.
        /// </summary>
        public string Base { get; set; } = string.Empty;

        /// <summary>
        /// get/set - The 'default-src' attribute.
        /// </summary>
        public string DefaultSource { get; set; } = string.Empty;

        /// <summary>
        /// get/set - The 'script-src' attribute.
        /// </summary>
        public string ScriptSource { get; set; } = string.Empty;

        /// <summary>
        /// get/set - The 'connect-src' attribute.
        /// </summary>
        public string ConnectSource { get; set; } = string.Empty;

        /// <summary>
        /// get/set - The 'img-src' attribute.
        /// </summary>
        public string ImageSource { get; set; } = string.Empty;

        /// <summary>
        /// get/set - The 'style-src' attribute.
        /// </summary>
        public string StyleSource { get; set; } = string.Empty;

        /// <summary>
        /// get/set - The 'form-action' attribute.
        /// </summary>
        public string FormAction { get; set; } = string.Empty;

        /// <summary>
        /// get/set - The 'font-src' attribute.
        /// </summary>
        public string FontSource { get; set; } = string.Empty;

        /// <summary>
        /// get/set - The 'frame-src' attribute.
        /// </summary>
        public string FrameSource { get; set; } = string.Empty;

        /// <summary>
        /// get/set - The 'frame-ancestors' attribute.
        /// </summary>
        public string FrameAncestors { get; set; } = string.Empty;

        /// <summary>
        /// Puts all of the properties together into a CSP string.
        /// </summary>
        /// <returns>The string CSP from the values.</returns>
        public string GenerateCSPString()
        {
            return $"base-uri {this.Base};" +
                   $"default-src {this.DefaultSource};" +
                   $"script-src {this.ScriptSource};" +
                   $"connect-src {this.ConnectSource};" +
                   $"img-src {this.ImageSource};" +
                   $"style-src {this.StyleSource};" +
                   $"form-action {this.FormAction};" +
                   $"font-src {this.FontSource};" +
                   $"frame-src {this.FrameSource};" +
                   $"frame-ancestors {this.FrameAncestors};";
        }
    }
}
