using System;
using System.Text;

namespace CoveX.Base
{
    /// <summary>
    /// The result of a measurement from quantum computation.
    /// </summary>
    public class ClassicalResult : ICoveObject
    {
        /// <summary>
        /// Since a measurement is just a classical result, a series of 0's and 1's, store it that way.
        /// </summary>
        protected bool[] baResult = null;


        /// <summary>
        /// The label for one
        /// </summary>
        private string sLabelOne = "1";


        /// <summary>
        /// The label for zero
        /// </summary>
        private string sLabelZero = "0";


        /// <summary>
        /// Default constructor, 0 length result
        /// </summary>
        public ClassicalResult() : this(new bool[] { false })
        {
        }


        /// <summary>
        /// Constructor to create a classical result based on the number of qubits
        /// in the register and the value of the result. (Which should be treated like
        /// an unsigned long, but long is used since it is CLS compliant.)
        /// </summary>
        /// <param name="NumberOfQubits">Number of qubits the result represents</param>
        /// <param name="Value">The value, to treat as an unsigned integer (so it is 
        /// known which bits to toggle on).</param>
        public ClassicalResult(int NumberOfQubits, long Value)
        {
            long iCurTotal = Value;

            //Make sure the Value can be represented with this many qubits
            if (Value > Math.Pow(2, NumberOfQubits))
            {
                throw new ArgumentOutOfRangeException(string.Format("Cannot represent {0} with only {1} qubits. (Max that can be represented is {2}.)", Value, NumberOfQubits, Math.Pow(2, NumberOfQubits)));
            }

            //now set the results, just go through and toggle the bits at each position
            this.baResult = new bool[NumberOfQubits];
            for (int iPower = (NumberOfQubits - 1); iPower >= 0; iPower--)
            {
                if (Math.Pow(2, iPower) <= iCurTotal)
                {
                    this.baResult[iPower] = true;
                    iCurTotal -= (long)Math.Pow(2, iPower);    //cast ok int to and int power results in an int
                }
                else
                {
                    this.baResult[iPower] = false;
                }
            }
        }


        /// <summary>
        /// Constructor to create a classical result based on the number of qubits
        /// in the register and the value of the result. (Which should be treated like
        /// an unsigned long, but long is used since it is CLS compliant.) It will
        /// create the result with the necessary number of qubits to hold the value
        /// and no more.
        /// </summary>
        /// <param name="Value">The value, to treat as an unsigned integer (so it is 
        /// known which bits to toggle on).</param>
        public ClassicalResult(long Value) : this(Utilities.BitsToExpress(Value), Value)
        {
        }


        /// <summary>
        /// Construct a classical result based on an array of booleans.
        /// </summary>
        /// <param name="Values">Array of bool values to set to.</param>
        /// <exception cref="ArgumentNullException">Thrown if Values is null.</exception>
        public ClassicalResult(bool[] Values)
        {
            if (Values == null)
                throw new ArgumentNullException("Values cannot be null");

            //set all values
            this.baResult = new bool[Values.Length];
            for (int iCurIndex = 0; iCurIndex < Values.Length; iCurIndex++)
            {
                this.baResult[iCurIndex] = Values[iCurIndex];
            }
        }


        /// <summary>
        /// Get the number of bits returned from the measurement.
        /// </summary>
        /// <returns>The number of bits in the result</returns>
        public int GetNumberOfBits()
        {
            if (this.baResult == null)       //check null for safety
                return 0;
            else
                return this.baResult.Length;
        }


        /// <summary>
        /// Return an array of booleans that represent the classical bits of 
        /// information in this result.
        /// </summary>
        /// <returns>The bool array representing the classical bits of information. This
        /// is a deep copy of what is contained in this result.</returns>
        public bool[] ToBoolArray()
        {
            bool[] baRetVal = null;

            if (this.baResult == null)       //return an empty array if null
                return new bool[] { };

            //return a copy so changes to the copy don't effect this class.
            baRetVal = new bool[this.baResult.Length];
            Array.Copy(this.baResult, baRetVal, this.baResult.Length);
            return baRetVal;
        }


        /// <summary>
        /// Return true if there are any ones in the result, else false.
        /// </summary>
        /// <returns></returns>
        public bool ToBool()
        {
            return ToBool(BoolConversionOptions.TrueIfAnyOnes);
        }


        /// <summary>
        /// Convert the result to a boolean based on the option specified.
        /// </summary>
        /// <param name="BoolConversionOption">Describes what rules should
        /// be use to convert to a boolean.</param>
        /// <returns>The result of the bool conversion.</returns>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public bool ToBool(BoolConversionOptions BoolConversionOption)
        {
            throw new NotImplementedException("Not yet implemented");

            return true;
        }

        /// <summary>
        /// Convert this result to an unsigned 32 bit integer. If this result
        /// contains less than 32 bits, then the extra bits will be set to 0.
        /// </summary>
        /// <returns>The 32 bit unsigned integer that represents the result.</returns>
        /// <exception cref="OverflowException">Thrown if the result contains more than
        /// 32 bits of classical data.</exception>
        public UInt32 ToUnsignedInt32()
        {
            UInt32 iRetVal = 0;

            //just build up the result from the bits that are 1
            for (int iCurElement = 0; iCurElement < this.baResult.Length; iCurElement++)
            {
                if (this.baResult[iCurElement] == true)
                {
                    iRetVal += (UInt32)Math.Pow(2, iCurElement);
                }
            }

            return iRetVal;
        }


        /// <summary>
        /// Assume this number is a 32 bit signed integer (using 2's
        /// compliment and return the signed integer that it represents
        /// </summary>
        /// <returns>The 32 bit signed integer that this value represents</returns>
        public Int32 ToSignedInt32()
        {
            int iRetVal = 0;

            //if the most significant bit is 1 then it is a negative number
            if (this.baResult[this.baResult.Length - 1] == true)
            {
                //essentially flip the bits
                for (int iCurElement = 0; iCurElement < (this.baResult.Length - 1);
                iCurElement++)
                {
                    if (this.baResult[iCurElement] == false)
                    {
                        iRetVal += (int)Math.Pow(2, iCurElement);
                    }
                }
                iRetVal++;          //add 1
                iRetVal *= -1;      //negate since this is a negative num
            }
            else
            {
                //just build up the result from the bits that are 1
                for (int iCurElement = 0; iCurElement < (this.baResult.Length - 1);
                iCurElement++)
                {
                    if (this.baResult[iCurElement] == true)
                    {
                        iRetVal += (int)Math.Pow(2, iCurElement);
                    }
                }
            }

            return iRetVal;
        }


        /// <summary>
        /// Convert this result to an unsigned 32 bit integer. If the result
        /// contains less than 32 bits, then the extra bits will be set to
        /// SetExtraBitsTo.
        /// </summary>
        /// <param name="SetExtraBitsTo">The extra bits in the result will be
        /// set to this value.</param>
        /// <returns>The 32 bit unsigned integer that represents the result.</returns>
        /// <exception cref="OverflowException">Thrown if the result contains more than
        /// 32 bits of classical data.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public UInt32 ToUnsignedInt32(bool SetExtraBitsTo)
        {
            //TODO: Implement this method
            throw new NotImplementedException("This method has not been implemented yet");
        }


        /// <summary>
        /// Return the string representation of this result.
        /// </summary>
        /// <returns>The string representation of this result.</returns>
        public override string ToString()
        {
            return this.BoolArrayToString(this.baResult);
        }


        /// <summary>
        /// Convert the bool array that represents this result to a string
        /// </summary>
        /// <returns>The string representation</returns>
        public string BoolArrayToString()
        {
            return this.BoolArrayToString(this.baResult);
        }


        /// <summary>
        /// Convert a boolean array to a string of 0's and 1's.
        /// </summary>
        /// <param name="BoolArray">Bool array to convert.</param>
        /// <returns>A string of 0's and 1's that represent the bool array.</returns>
        public string BoolArrayToString(bool[] BoolArray)
        {
            StringBuilder sbOutput = new StringBuilder();

            if (BoolArray == null)    //take null into account
                return "";

            //build up the output and return it
            foreach (bool bCurElement in BoolArray)
            {
                if (bCurElement == true)
                    sbOutput.Append(this.sLabelOne);
                else
                    sbOutput.Append(this.sLabelZero);
            }

            return sbOutput.ToString();
        }


        /// <summary>
        /// Return the boolean value for the specified index.
        /// </summary>
        /// <param name="Index">Index to get the value of</param>
        /// <returns>The boolean value of that index.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified 
        /// is out of range for this result.</exception>
        public bool GetIndexAsBool(int Index)
        {
            if ((Index < 0) || (Index > this.baResult.Length))
                throw new IndexOutOfRangeException(string.Format("Index {0} is outside the allowable range of {1} - {2}", Index, 0, this.baResult.Length));

            return this.baResult[Index];
        }


        /// <summary>
        /// Change the label of 1
        /// </summary>
        /// <param name="LabelOne">The new label of one</param>
        public void SetLabelOne(string LabelOne)
        {
            this.sLabelOne = LabelOne;
        }


        /// <summary>
        /// Set both the label for |0> and label for |1> in one call
        /// </summary>
        /// <param name="LabelZero">The label of zero</param>
        /// <param name="LabelOne">The label of one</param>
        public void SetLabels(string LabelZero, string LabelOne)
        {
            this.SetLabelZero(LabelZero);
            this.SetLabelOne(LabelOne);
        }


        /// <summary>
        /// Change the label of 0
        /// </summary>
        /// <param name="LabelZero">The new label of zero</param>
        public void SetLabelZero(string LabelZero)
        {
            this.sLabelZero = LabelZero;
        }

    }
}
