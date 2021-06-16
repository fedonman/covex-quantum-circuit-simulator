namespace CoveX.Base
{
    /// <summary>
    /// Defines various conversion options for converting classical
    /// results to a boolean.
    /// </summary>
    public enum BoolConversionOptions
    {
        /// <summary>
        /// If the classical result contains any ones then true is returned.
        /// </summary>
        TrueIfAnyOnes = 0,            //default

        /// <summary>
        /// If all the bits in the result are one, then true is returned.
        /// </summary>
        TrueIfAllOnes = 1,

        /// <summary>
        /// If a majority of the bits (more than half) are one, then true
        /// is returned. If the same number of bits are one and zero then
        /// false is returned. More than half must be one.
        /// </summary>
        TrueIfMoreThanHalfOnes = 2,

        /// <summary>
        /// If half or more of the bits are one, then return true. If the
        /// same number of bits are one and zero then true is returned.
        /// </summary>
        TrueIfHalfOrMoreOnes = 3
    }

}
