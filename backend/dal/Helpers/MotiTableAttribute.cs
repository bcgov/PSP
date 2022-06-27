using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// MotiTableAttribute class, provides an attribute to identify the abbreviation of a table name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MotiTableAttribute : TableAttribute
    {
        #region Properties
        /// <summary>
        /// get - The abbreviated table name.
        /// </summary>
        public string Abbreviation { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an MotiTableAttribute, initializes with specified arguments.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="abbreviation"></param>
        public MotiTableAttribute(string name, string abbreviation) : base(name)
        {
            if (String.IsNullOrWhiteSpace(abbreviation))
            {
                throw new ArgumentException("Abbreviation cannot be null, empty, or whitespace.", nameof(abbreviation));
            }

            this.Abbreviation = abbreviation;
        }
        #endregion
    }
}
