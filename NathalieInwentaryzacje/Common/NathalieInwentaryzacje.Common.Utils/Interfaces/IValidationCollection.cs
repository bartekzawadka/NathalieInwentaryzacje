namespace NathalieInwentaryzacje.Common.Utils.Interfaces
{
    public interface IValidationCollection
    {

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        bool IsValid { get; set; }

        /// <summary>
        /// Gets the items count.
        /// </summary>
        bool IsEmpty { get; set; }

        /// <summary>
        /// Gets the error.
        /// </summary>
        string Error { get; set; }
    }
}
