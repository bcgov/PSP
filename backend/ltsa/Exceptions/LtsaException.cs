using Pims.Core.Http;
using Pims.Ltsa.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Pims.Core.Exceptions
{
    /// <summary>
    /// LtsaException class, provides a way to express HTTP request exceptions that occur.
    /// </summary>
    public class LtsaException : HttpClientRequestException
    {
        #region Properties
        /// <summary>
        /// get - Additional detail on the error.
        /// </summary>
        public string Detail { get; }

        /// <summary>
        /// get - The HTTP request client the exception originated from.
        /// </summary>
        public IHttpRequestClient Client { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an LtsaException class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="client"></param>
        /// <param name="error"></param>
        public LtsaException(HttpClientRequestException exception, IHttpRequestClient client, Error error)
            : base($"{exception.Message}{Environment.NewLine}", exception, exception.StatusCode.Value)
        {
            if (exception?.Response?.Content != null)
            {
                if (error?.ErrorMessages?.Count > 0)
                {
                    this.Detail = String.Join(Environment.NewLine, error.ErrorMessages.Select(e => $"\t{e}"));
                }
            }
            else
                this.Client = client;

        }

        /// <summary>
        /// Creates a new instance of an LtsaException class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public LtsaException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, statusCode)
        {
        }

        /// <summary>
        /// Creates a new instance of an LtsaException class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public LtsaException(string message, Exception innerException, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, innerException, statusCode)
        {
        }

        /// <summary>
        /// Creates a new instance of an LtsaException class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public LtsaException(HttpResponseMessage response) : base(response)
        {
        }

        /// <summary>
        /// Creates a new instance of an LtsaException class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public LtsaException(HttpResponseMessage response, Exception innerException) : base(response, innerException)
        {
        }
        #endregion
    }
}
