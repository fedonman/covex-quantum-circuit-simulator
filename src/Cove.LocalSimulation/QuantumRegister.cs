using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;
using System.Collections.Generic;


namespace CoveX.LocalSimulation
{
    /// <summary>
    /// A quantum register is a collection of qubits.
    /// </summary>
    public class QuantumRegister : IQuantumRegister
    {
        /// <summary>
        /// The location of this quantum resource. For the local simulation this can only be the
        /// local host (127.0.0.1).
        /// </summary>
        private string sLocation = "localhost";

        /// <summary>
        /// The number total number of qubits in this register. The ones actually exposed
        /// to users after slice operations may be a subset of this. The length of the 
        /// exposed register can be obtained through listExposedQubits.Length.
        /// </summary>
        private int iTotalNumberOfQubits = 0;

        /// <summary>
        /// The list of qubits exposed to the user of this register. The index in this list is
        /// the index of the exposed qubit. The value of that index is the actual index of the 
        /// qubit exposed. Example: if listExposedQubits[0] == 3 then the first qubit exposed
        /// to the user is actually the 4th one (index 3) in this register.
        /// </summary>
        private List<int> listExposedQubits = null;

        /// <summary>
        /// The state of the quantum register, expressed as a matrix of complex numbers. The
        /// default size is overridden in the constructors.
        /// </summary>
        private CoveX.ClassicalUtilities.ComplexMatrix cState = null;

        /// <summary>
        /// The random number source needed for the classical simulation.
        /// </summary>
        /// <remarks>
        /// This is a static member so that one value is shared across all instances. This prevents
        /// the problem of creating a bunch of quantum registers at once that are seeded with the same timer
        /// value- the result being far from even pseudo-random.
        /// </remarks>
        private static Random cRandomSource = new Random();

        /// <summary>
        /// The label for one
        /// </summary>
        private string sLabelOne = "1";

        /// <summary>
        /// The label for zero
        /// </summary>
        private string sLabelZero = "0";

        /// <summary>
        /// The maximum number of qubits allowed in a single register.
        /// </summary>
        public const int MAXIMUM_QUBITS_IN_REGISTER = 62;


        /// <summary>
        /// Default constructor, create a register of a single qubit
        /// </summary>
        public QuantumRegister() : this(1)
        {
        }


        /// <summary>
        /// Construct a quantum register with a specific number of qubits.
        /// </summary>
        /// <param name="NumberOfQubits">Number of qubits in the register.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the NumberOfQubits parameter
        /// is less than or equal to zero- it must be a positive integer. Also thrown if the
        /// maximum number of allowable qubits would be exceeded.</exception>
        public QuantumRegister(int NumberOfQubits)
        {
            if (NumberOfQubits <= 0)
                throw new ArgumentOutOfRangeException("The number of qubits must be a positive integer.");
            if (NumberOfQubits > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException("The maximum number of qubits in a register is limited to " + MAXIMUM_QUBITS_IN_REGISTER.ToString());

            this.iTotalNumberOfQubits = NumberOfQubits;
            this.listExposedQubits = new List<int>(NumberOfQubits);
            for (int iCurIndex = 0; iCurIndex < NumberOfQubits; iCurIndex++)  //initial order matches actual ordering
                this.listExposedQubits.Add(iCurIndex);

            this.cState = new CoveX.ClassicalUtilities.ComplexMatrix((int)Math.Pow(2, NumberOfQubits), 1);
            this.cState.SetValue(0, 0, 1);     //it is absolutely in state 000000... all other cells are already 0

            //verify the state
            this.VerifyValidState();
        }


        /// <summary>
        /// Create a new quantum register where each qubit is set to the bool specified in the array. 
        /// Qubit x will be set to element x in InitialQubitValues.
        /// </summary>
        /// <param name="InitialQubitValues">An array of boolean initial values, must contain
        /// at least 1 element.</param>
        /// <exception cref="ArgumentNullException">Thrown if InitialQubitValues is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if InitialQubitValues does
        /// not contain at least one element.</exception>
        public QuantumRegister(bool[] InitialQubitValues) : this(InitialQubitValues.Length)
        {
            if (InitialQubitValues == null)
                throw new ArgumentNullException("InitialQubitValues cannot be null");
            if (InitialQubitValues.Length <= 0)
                throw new ArgumentOutOfRangeException("InitialQubitValues must contain at least one element.");

            //set the state of the register
            this.SetQubits(InitialQubitValues);
        }


        /// <summary>
        /// Returns the location of the quantum resource.
        /// </summary>
        /// <returns>
        /// The location of the quantum resource.
        /// </returns>
        public string GetLocation()
        {
            return this.sLocation;
        }


        /// <summary>
        /// Returns the exposed qubit list for testing purposes. This is not meant to be
        /// used by users of the class, which is why the access is internal.
        /// </summary>
        /// <returns></returns>
        internal List<int> GetExposedQubits()
        {
            List<int> listRetVal = new List<int>();

            //value types, so ok. if these were objects it would only be a shallow copy.
            listRetVal.AddRange(this.listExposedQubits);

            return listRetVal;
        }


        /// <summary>
        /// Throw an exception if the state of the register is invalid,
        /// do nothing otherwise.
        /// </summary>
        /// <exception cref="ImplementationException">Thrown if the state of</exception>
        public void VerifyValidState()
        {
            if (this.IsValidState() == false)
                throw new ImplementationException("The register is not in a valid state. State:\n" + this.cState.ToString(true));
        }


        /// <summary>
        /// Checks to make sure the register is in a valid state: all entries
        /// in the matrix representing the state must have the squares of their
        /// values total 1.0. 
        /// </summary>
        /// <returns>True if in a valid state.</returns>
        public bool IsValidState()
        {
            double dTotal = 0.0;

            if (this.cState.GetNumberOfColumns() != 1)
                return false;

            //number of rows must be a power of 2 (since there should be 2^n rows)
            if (ClassicalUtilities.ClassicalMath.IsPowerOf(2, this.cState.GetNumberOfRows()) == false)
                return false;

            //now gather the probability totals
            for (int iCurRow = 0; iCurRow < this.cState.GetNumberOfRows(); iCurRow++)
                dTotal += this.cState.GetValue(iCurRow, 0).AbsoluteValueSquared();

            //if the total is within tolerance of 1.0 then consider it valid (all probabilities must total 1.0)
            if ((dTotal > (1.0 - Complex.EQUALITY_TOLERANCE)) && (dTotal < (1.0 + Complex.EQUALITY_TOLERANCE)))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Expose the state of the system. This is a deep copy of the state, so
        /// there is no need to worry about inadvertently altering the state. This
        /// is protected so user subclasses can access the state and internal so
        /// that unit tests can access it.
        /// </summary>
        /// <returns>A deep copy of the current state.</returns>
        protected internal CoveX.ClassicalUtilities.ComplexMatrix GetState()
        {
            return this.cState.Clone();
        }


        /// <summary>
        /// Set the location of the quantum resource. Note that this resets
        /// the state of all of the qubits.
        /// </summary>
        /// <param name="Location">The location of the quantum resource</param>
        /// <remarks>This sets the locations of all the qubits used by this resource.</remarks>
        /// <exception cref="ArgumentException">The local host is the only valid location
        /// for the local simulation.</exception>
        public void SetLocation(string Location)
        {
            //for the local simulation the only valid location is the local host.
            if ((Location.ToLower() != "localhost") && (Location != "127.0.0.1"))
                throw new ArgumentException("The location can only be localhost (127.0.0.1) for the local simulation.");

            this.sLocation = Location;
        }


        /// <summary>
        /// Narrow a value based on a bit mask. Basically all the bits in the value that
        /// are in the bit mask are squezed into a value. Example if the bit mask is 1011
        /// and the value is 1000 then it will return 0100. (because there is a gap in the bit 
        /// mask so it shifts it over one.
        /// </summary>
        /// <param name="BitMask">The bit mask to use</param>
        /// <param name="Value">Value to narrow on</param>
        /// <returns>Returns the narrowed value</returns>
        protected internal long NarrowFromBitMask(long BitMask, long Value)
        {
            long iRetVal = 0;
            long iCurBit = 0;
            int iCurPower = 0;

            for (int i = 0; i < 64; i++)
            {
                //is this a bit to pay attention to?
                iCurBit = (long)Math.Pow(2, i);
                if ((iCurBit & BitMask) == iCurBit)
                {
                    //if it is set in the value, then increase the return value accordingly
                    if ((iCurBit & Value) == iCurBit)
                    {
                        iRetVal += (long)Math.Pow(2, iCurPower);
                    }
                    iCurPower += 1;        //increase the counter for the next bit
                }
            }                //end for i

            return iRetVal;
        }


        /// <summary>
        /// Get the place (0 based) in the mask that the current TestBit 
        /// is at. Example, if bit mask is 0x0A and test bit is 3 then
        /// return 1. This is because 0x0A = 0000 1010 and bit 3 (0x08)
        /// is set to true and is in place 1 (0 based).
        /// </summary>
        /// <param name="BitMask">Bit mask to look in.</param>
        /// <param name="TestBit">Test bit (0 based)</param>
        /// <returns>0th based place of the bit.</returns>
        /// <exception cref="ImplementationException">Thrown if the TestBit
        /// is not 1 in the BitMask.</exception>
        protected internal int GetPlaceInMask(long BitMask, int TestBit)
        {
            int iRetVal = 0;         //is the 0th place by default.

            //make sure the bit being looked for is set in the mask, else the place has
            //no meaning
            if ((BitMask & (long)Math.Pow(2, TestBit)) != (long)Math.Pow(2, TestBit))
                throw new ImplementationException(string.Format("Bit mask of {0} decimal, but bit {1} is not set to 1. The test bit is not in the bit mask.", BitMask, TestBit));

            //go through the bits up to right before the one we're looking for (which we know is set)
            for (int iCurIndex = 0; iCurIndex < TestBit; iCurIndex++)
            {
                //is this bit on in the mask? if so, advance the counter
                if ((BitMask & (long)Math.Pow(2, iCurIndex)) == (long)Math.Pow(2, iCurIndex))
                {
                    iRetVal += 1;
                }
            }

            return iRetVal;
        }


        /// <summary>
        /// Does the current state (State) match the collapsed value in the measuring mask?
        /// Example: Measuring mask is XX11X1, Collapsed value is 011 (3 bits in measuring
        /// mask). So if State is in the form XX01X1 then true is returned.
        /// </summary>
        /// <param name="MeasureMask"></param>
        /// <param name="CollapsedValue"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public bool IsStateInPartialCollapse(long MeasureMask, long CollapsedValue, long State)
        {
            int iCollapsedIndex = 0;
            bool fCollapsedBit = false;

            for (int i = 0; i < 64; i++)
            {
                //is the bit set in the measuring mask, then need to pay attention
                //to it. (else we don't care what it is)
                if ((MeasureMask & (long)Math.Pow(2, i)) == (long)Math.Pow(2, i))
                {
                    //see if the bit in the collapsed value is true or false
                    if ((CollapsedValue & (long)Math.Pow(2, iCollapsedIndex)) == (long)Math.Pow(2, iCollapsedIndex))
                        fCollapsedBit = true;
                    else
                        fCollapsedBit = false;

                    //now see if the bit in State matches the collapsed bit
                    if ((((State & (long)Math.Pow(2, i)) == (long)Math.Pow(2, i)) && (fCollapsedBit == true))
                    || (((State & (long)Math.Pow(2, i)) == 0) && (fCollapsedBit == false)))
                    {
                        //matches, so increase the collapsed counter
                        iCollapsedIndex += 1;
                    }
                    else
                    {
                        //else it doesn't match, so false
                        return false;
                    }
                }             //end if bit to pay attention to
            }                 //end for i

            //made it here then they all matched, so return true.
            return true;
        }


        /// <summary>
        /// Perform a measurement on the quantum register, collapsing to an absolute state.
        /// </summary>
        /// <returns>An array of booleans (bits), which are the qubits collapsed to
        /// false (0) or true (1).</returns>
        public ClassicalResult Measure()
        {
            ClassicalResult cRetVal = null;
            double[] daTempProbabilities = null;
            Complex[] caTempAmplitudes = null;
            Complex cAmplitudeOfMeasurement = null;
            long iMeasuringMask = 0;
            long iCollapsed = 0;
            long iCollapsedBasedOnSlice = 0;
            int iPlaceInCollapsed = 0;
            bool fFoundOutput = false;
            double dSelection = 0.0;
            double dTotal = 0.0;

            //first check that the register is a consistent state so that the selection of the desired
            //output works correctly.
            this.VerifyValidState();

            //Use a bit mask to figure out which temp probability the current element belongs to.
            foreach (long iExposedQubit in this.listExposedQubits)
            {
                iMeasuringMask += (long)Math.Pow(2, iExposedQubit);
            }

            //need to keep track of the probabilities and amplitudes of for the 
            //partial measurement. probabilities are used to determine what it collapses to,
            //amplitudes to alter the state of the register after measurement.
            daTempProbabilities = new double[(int)Math.Pow(2, this.listExposedQubits.Count)];
            caTempAmplitudes = new Complex[(int)Math.Pow(2, this.listExposedQubits.Count)];
            for (int iCurIndex = 0; iCurIndex < (int)Math.Pow(2, this.listExposedQubits.Count); iCurIndex++)
                caTempAmplitudes[iCurIndex] = new Complex(0, 0);   //init all amplitudes to 0 + 0i.
            for (long iCurRow = 0; iCurRow < this.cState.GetNumberOfRows(); iCurRow++)
            {
                daTempProbabilities[this.NarrowFromBitMask(iMeasuringMask, iCurRow)]
                    += this.cState.GetValue(iCurRow, 0).AbsoluteValueSquared();

                //square the amplitudes
                caTempAmplitudes[this.NarrowFromBitMask(iMeasuringMask, iCurRow)]
                    += (this.cState.GetValue(iCurRow, 0) * this.cState.GetValue(iCurRow, 0));
            }                //end of iteration through the states

            //need to take the square root of each to get the final numbers to adjust by
            for (int iCurIndex = 0; iCurIndex < caTempAmplitudes.Length; iCurIndex++)
            {
                caTempAmplitudes[iCurIndex] = caTempAmplitudes[iCurIndex].SquareRoot();
            }

            //This could obviously be sped up, but is simple and suffices for now since this is a prototype.
            //
            //For the Measurement, one of the possible values is selected based on their probability of
            //occuring. So select a random number >= 0.0 and < 1.0 via NextDouble(). Then start going 
            //through the probabilities until that one occurs.
            //Example on a two qubit register:
            //P(|00>) = 0.1
            //P(|01>) = 0.2
            //P(|10>) = 0.5
            //P(|11>) = 0.2
            //dSelection is 0.4
            //Check if |00> by 0.1 > 0.4. Nope,
            //Check if |01> by (0.1 + 0.2) > 0.4. Nope,
            //Check if |10> by (0.1 + 0.2 + 0.5) > 0.4, Yes (we've rolled over the boundry with this check)
            dSelection = cRandomSource.NextDouble();

            //since measurement collapses, go through and find the correct state. Set it to 1 and all other
            //states to 0.
            for (long iCurRow = 0; iCurRow < this.cState.GetNumberOfRows(); iCurRow++)   //start going through the rows
            {
                if (fFoundOutput == false)
                {
                    dTotal += daTempProbabilities[iCurRow];

                    //rolled over, this is the selected output only if there is a chance of this
                    //state occuring
                    if ((daTempProbabilities[iCurRow] > 0)
                    && (dTotal > dSelection))
                    {
                        //found the collapsed state, just get the collapsed value without taking
                        //into account any slicing.
                        iCollapsed = 0;
                        for (int iExposedQubitIndex = 0; iExposedQubitIndex < this.listExposedQubits.Count;
                        iExposedQubitIndex++)
                        {
                            if ((iCurRow & ((long)Math.Pow(2, iExposedQubitIndex)))
                            == ((long)Math.Pow(2, iExposedQubitIndex)))
                            {
                                iCollapsed += (long)Math.Pow(2, iExposedQubitIndex);
                            }
                        }           //end of handling reordering

                        cAmplitudeOfMeasurement = caTempAmplitudes[iCurRow];
                        fFoundOutput = true;
                        iCollapsed = iCurRow;    //reset to use later
                        break;         //no need to keep iterating through looking for the result when we just found it
                    }
                }                      //end if still looking for output
            }                          //end of going through the rows

            //should not make it to here, as an output should be selected above
            if (fFoundOutput == false)
                throw new ImplementationException("Cove.LocalSimulation.QuantumRegister.Measure() didn't select an output in the for loop.");

            //need to construct return value from the collapsed based on the ordering of the exposed qubits.
            //(the collapsed value is the true ordering.)
            //Example: under normal (0-n) ordering, may collapse to XYZ. If the exposed order is reversed then need
            //to return the measurement as ZYX.
            for (int iCurIndex = 0; iCurIndex < this.listExposedQubits.Count; iCurIndex++)
            {
                //get the place in the mask of this qubit in the iteration
                iPlaceInCollapsed = this.GetPlaceInMask(iMeasuringMask, this.listExposedQubits[iCurIndex]);

                //if this qubit is set in the collapsed one then set in the result at the 
                //iCurIndex bit. (since we want to return in the exposed order)
                if ((iCollapsed & (long)Math.Pow(2, iPlaceInCollapsed)) == (long)Math.Pow(2, iPlaceInCollapsed))
                {
                    iCollapsedBasedOnSlice += (long)Math.Pow(2, iCurIndex);
                }
            }
            cRetVal = new ClassicalResult(this.listExposedQubits.Count, iCollapsedBasedOnSlice);

            //now alter the states based on the (potentially partial) measurement
            for (long iCurRow = 0; iCurRow < this.cState.GetNumberOfRows(); iCurRow++)
            {
                //if this state includes the partial measurement then need to increase its 
                //probability amplitude and eliminate those that can no longer occur.
                //don't change the amplitude if it was already absolutely collapsed to this state
                if (this.IsStateInPartialCollapse(iMeasuringMask, iCollapsed, iCurRow) == true)
                {
                    //given a|00> + b|01> + c|10> + d|11>
                    //if we measure the left qubit then we get
                    //u|0> tensor ((a/u)|0> + (b/u)|1>)
                    //see the dissertation for more details.
                    this.cState.SetValue(iCurRow, 0,
                        (this.cState.GetValue(iCurRow, 0) / cAmplitudeOfMeasurement));
                }
                else         //no chance of this state after the measurement
                {
                    this.cState.SetValue(iCurRow, 0, 0);
                }
            }                //end of iteration through potential states
            VerifyValidState();    //make sure the above altering didn't throw things off

            cRetVal.SetLabels(this.sLabelZero, this.sLabelOne);
            return cRetVal;
        }


        /// <summary>
        /// Measure the quantum register and put the results in ReturnValue. A user
        /// can subclass ClassicalResult with their own class and pass that in if
        /// needed.
        /// </summary>
        /// <param name="ReturnValue">The object which will contain the
        /// result of the measurement.</param>
        public void Measure(ClassicalResult ReturnValue)
        {
            ReturnValue = this.Measure();
        }


        /// <summary>
        /// Measure a set of qubits in the register.
        /// </summary>
        /// <param name="Indexes">Indexes of the qubits to measure</param>
        /// <returns>An array of booleans (bits), which are the qubits collapsed to
        /// false (0) or true (1).</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the
        /// indexes are invalid.</exception>
        public ClassicalResult Measure(IEnumerable<int> Indexes)
        {
            return (this.SliceSubset(Indexes)).Measure();
        }


        /// <summary>
        /// Measure a set of qubits in the register. A user
        /// can subclass ClassicalResult with their own class and pass that in if
        /// needed.
        /// </summary>
        /// <param name="Indexes">Indexes of the qubits to measure</param>
        /// <param name="ReturnValue">The object which will contain the
        /// result of the measurement.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the
        /// indexes are invalid.</exception>
        public void Measure(IEnumerable<int> Indexes, ClassicalResult ReturnValue)
        {
            //just wrap the other method.
            ReturnValue = this.Measure(Indexes);
        }


        /// <summary>
        /// Measure one qubit in the register.
        /// </summary>
        /// <param name="Index">Index of the qubit to measure.</param>
        /// <returns>The classical information extracted from the qubit, false (0) or
        /// true (1).</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is invalid.</exception>
        public ClassicalResult Measure(int Index)
        {
            //just call the existing method that calls an array
            return this.Measure(new List<int>() { Index });
        }


        /// <summary>
        /// Measure one qubit in the register.. A user
        /// can subclass ClassicalResult with their own class and pass that in if
        /// needed.
        /// </summary>
        /// <param name="Index">Index of the qubit to measure.</param>
        /// <param name="ReturnValue">The classical information extracted from the qubit, 
        /// false (0) or true (1).</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is invalid.</exception>
        public void Measure(int Index, ClassicalResult ReturnValue)
        {
            ReturnValue = this.Measure(Index);
        }


        /// <summary>
        /// Gets the complex matrix that represents the entire state of the register that is
        /// being tracked, which may be more than is currently exposed. Use PeekAtVisibleState()
        /// to see the state only the qubits currently exposed by this registerThis method is specific to this
        /// simulation and cannot be done on a true quantum computer. As such, this method is
        /// only recommended for learning purposes.
        /// </summary>
        /// <returns>The complex matrix that represents the state of the register.</returns>
        /// <remarks>This method is specific to this local simulation, and is not possible on
        /// an actual quantum computer.</remarks>
        public ComplexMatrix PeekAtEntireState()
        {
            return this.cState.Clone();
        }


        /// <summary>
        /// Get the complex matrix that represents the state of the qubits exposed by this
        /// register. Use PeekAtEntireState() to see the state of all qubits that are part of the
        /// register, which may be more than exposed. This method is specific to this
        /// simulation and cannot be done on a true quantum computer. As such, this method is
        /// only recommended for learning purposes.
        /// </summary>
        /// <returns>The complex matrix that represents the state of the register.</returns>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public ComplexMatrix PeekAtVisibleState()
        {
            //TODO: Nedd to take into account that not all the qubits are exposed.

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Add another register to the beginning of this one.
        /// </summary>
        /// <param name="RegisterToAppend">Register to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubitsAtBeginning(IQuantumRegister RegisterToAppend)
        {
            if (RegisterToAppend == null)     //appending null is valid and does nothing.
                return this;

            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + RegisterToAppend.GetTotalLength()) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, RegisterToAppend.GetTotalLength()));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Append a set of registers to the beginning of this register, in the same order as
        /// the array.
        /// </summary>
        /// <param name="RegistersToAppend">Registers to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size. Also thrown if an element in the array passed is null.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubitsAtBeginning(IQuantumRegister[] RegistersToAppend)
        {
            int iTotalNumOfQubitsToAdd = 0;

            if (RegistersToAppend == null)     //appending null is valid and does nothing
                return this;

            //make sure the max number of qubits isn't exceeded
            foreach (IQuantumRegister cCurRegister in RegistersToAppend)
            {
                if (cCurRegister == null)
                    throw new ArgumentOutOfRangeException("While inserting a null array is valid, inserting a valid array with null elements is not");
                iTotalNumOfQubitsToAdd += cCurRegister.GetTotalLength();
            }

            if ((this.iTotalNumberOfQubits + iTotalNumOfQubitsToAdd) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, iTotalNumOfQubitsToAdd));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Add a single qubit to the beginning of the register.
        /// </summary>
        /// <param name="QubitToAppend">The qubit to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubitsAtBeginning(IQubit QubitToAppend)
        {
            //if already at the max number of qubits, then cannot exceed
            if (this.iTotalNumberOfQubits == MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The register already contains the maximum number of qubits ({0}), you cannot append any more qubits to it.", MAXIMUM_QUBITS_IN_REGISTER));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Add a series of qubits to the beginning of the register, in the same order as specified
        /// in the array.
        /// </summary>
        /// <param name="QubitsToAppend">The qubits to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size. Also thrown if an element in the array passed is null.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubitsAtBeginning(IQubit[] QubitsToAppend)
        {
            if (QubitsToAppend == null)      //appending null is valid and does nothing
                return this;

            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + QubitsToAppend.Length) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, QubitsToAppend.Length));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Insert a number of qubits at the beginning of the register. All other
        /// qubits are shifted to higher indexes.
        /// </summary>
        /// <param name="NumberOfQubitsToAdd">The number of qubits to add to the
        /// register.</param>
        /// <returns>A reference to the expanded register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubitsAtBeginning(int NumberOfQubitsToAdd)
        {
            ComplexMatrix cNewRegister = null;
            int[] iaExposeNewRegister = null;

            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + NumberOfQubitsToAdd) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, NumberOfQubitsToAdd));

            //create a new matrix for the register of the appropriate size, it is absolutely 0
            cNewRegister = new ComplexMatrix((int)Math.Pow(2, NumberOfQubitsToAdd), 1);

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            //TODO: What about other registers using this state? their list of exposed qubits needs
            //to be updated somehow.
            //Perhaps keep track of all registers that share this state through an internal list?
            //every time a slice is created (the only way to get a reference to this same state)
            //that new register is added to the list. When this expansion happens that list is 
            //iterated through to update their list of exposed qubits. When going through this iteration
            //would also check for and remove any nulls. This is because it is possible for a slice
            //to be thrown away.

            //To combine registers A and B just do A tensor B
            //have to do it this way so references from other registers to this state isn't screww
            this.cState.TensorAsRightHandSide(cNewRegister);

            this.iTotalNumberOfQubits = this.iTotalNumberOfQubits + NumberOfQubitsToAdd;

            //update the exposed qubits- these will automatically be exposed
            iaExposeNewRegister = new int[NumberOfQubitsToAdd];
            for (int i = 0; i < NumberOfQubitsToAdd; i++)
                iaExposeNewRegister[i] = i;
            this.listExposedQubits.InsertRange(0, iaExposeNewRegister);

            return this;
        }


        /// <summary>
        /// Insert a number of qubits at the beginning of the register. All other
        /// qubits are shifted to higher indexes.
        /// </summary>
        /// <param name="NumberOfQubitsToAdd">The number of qubits to add to the
        /// register.</param>
        /// <param name="InitializeTo">Value to initialize the new qubits to.</param>
        /// <returns>A reference to the expanded register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubitsAtBeginning(int NumberOfQubitsToAdd, bool InitializeTo)
        {
            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + NumberOfQubitsToAdd) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, NumberOfQubitsToAdd));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Insert a quantum register into this one.
        /// </summary>
        /// <param name="AtIndex">The register will be inserted starting at this index. The qubits
        /// at this index and after at the existing one will be shifted.</param>
        /// <param name="RegisterToInsert">The register to insert into this one.Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined regiser.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubits(int AtIndex, IQuantumRegister RegisterToInsert)
        {
            if (RegisterToInsert == null)    //inserting null is valid and does nothing
                return this;

            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + RegisterToInsert.GetTotalLength()) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, RegisterToInsert.GetTotalLength()));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Insert quantum registers into this one.
        /// </summary>
        /// <param name="AtIndex">The register will be inserted starting at this index. The qubits
        /// at this index and after at the existing one will be shifted.</param>
        /// <param name="RegistersToInsert">The registers to insert. They will be inserted 
        /// in the same order they are in the array. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined register.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size. Also thrown if an element in the array passed is null.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubits(int AtIndex, IQuantumRegister[] RegistersToInsert)
        {
            int iTotalNumberOfQubitsToInsert = 0;

            if (RegistersToInsert == null)    //inserting null is valid and does nothing
                return this;

            foreach (IQuantumRegister cCurRegister in RegistersToInsert)
            {
                if (cCurRegister == null)
                    throw new ArgumentOutOfRangeException("While a null array can be inserted, a valid array with null registers within it cannot");
                iTotalNumberOfQubitsToInsert += cCurRegister.GetTotalLength();
            }

            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + iTotalNumberOfQubitsToInsert) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, iTotalNumberOfQubitsToInsert));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Insert a single qubit into the register.
        /// </summary>
        /// <param name="AtIndex">The qubit will be inserted starting at this index. The qubits
        /// at this index and after at the existing one will be shifted.</param>
        /// <param name="QubitToInsert">The qubit to insert. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>The reference to the combined register.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubits(int AtIndex, IQubit QubitToInsert)
        {
            if (QubitToInsert == null)       //insert null is valid and does nothing
                return this;

            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + 1) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, 1));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Insert multiple qubits to the register.
        /// </summary>
        /// <param name="AtIndex">The qubit will be inserted starting at this index. The qubits
        /// at this index and after at the existing one will be shifted.</param>
        /// <param name="QubitsToInsert">The qubits to insert into the register. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>The reference to the combined register.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size. Also thrown if an element in the array passed is null.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubits(int AtIndex, IQubit[] QubitsToInsert)
        {
            if (QubitsToInsert == null)       //appending null is valid and does nothing
                return this;

            foreach (IQubit cCurQubit in QubitsToInsert)
                if (cCurQubit == null)
                    throw new ArgumentOutOfRangeException("While passing a null array is valid, passing a valid array with null elements is not");

            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + QubitsToInsert.Length) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, QubitsToInsert.Length));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Insert qubits into an arbitrary location in the register.
        /// </summary>
        /// <param name="AtIndex">The qubits will be inserted starting at this index. 
        /// The qubits at this index and after at the existing register will be shifted.</param>
        /// <param name="NumberOfQubitsToAdd">Number of qubits to insert.</param>
        /// <returns>A reference to the combined register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubits(int AtIndex, int NumberOfQubitsToAdd)
        {
            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + NumberOfQubitsToAdd) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, NumberOfQubitsToAdd));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Insert qubits into an arbitrary location in the register.
        /// </summary>
        /// <param name="AtIndex">The qubits will be inserted starting at this index. 
        /// The qubits at this index and after at the existing register will be shifted.</param>
        /// <param name="NumberOfQubitsToAdd">Number of qubits to insert.</param>
        /// <param name="InitializeTo">Value to initialize the new qubits to.</param>
        /// <returns>A reference to the combined register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubits(int AtIndex, int NumberOfQubitsToAdd, bool InitializeTo)
        {
            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + NumberOfQubitsToAdd) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, NumberOfQubitsToAdd));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Append another register to this one.
        /// </summary>
        /// <param name="RegisterToAppend">Register to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubitsAtEnd(IQuantumRegister RegisterToAppend)
        {
            if (RegisterToAppend == null)    //inserting null is valid and does nothing
                return this;

            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + RegisterToAppend.GetLength()) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, RegisterToAppend.GetTotalLength()));
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Append a set of registers to this register, in the same order as
        /// the array.
        /// </summary>
        /// <param name="RegistersToAppend">Registers to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size. Also thrown if an element in the array passed is null.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubitsAtEnd(IQuantumRegister[] RegistersToAppend)
        {
            int iTotalNumberOfQubitsToInsert = 0;

            if (RegistersToAppend == null)   //inserting null is valid and does nothing
                return this;

            foreach (IQuantumRegister cCurRegister in RegistersToAppend)
            {
                if (cCurRegister == null)
                    throw new ArgumentOutOfRangeException("While appending a null array is valid, appending an array with null elements is not.");
                iTotalNumberOfQubitsToInsert += cCurRegister.GetTotalLength();
            }

            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + iTotalNumberOfQubitsToInsert) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, iTotalNumberOfQubitsToInsert));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Append a single qubit to the register.
        /// </summary>
        /// <param name="QubitToAppend">The qubit to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the expanded register</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubitsAtEnd(IQubit QubitToAppend)
        {
            if (QubitToAppend == null)       //inserting null is valid and does nothing
                return this;

            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + 1) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, 1));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Append a specific number of qubits to the register.
        /// </summary>
        /// <param name="NumberOfQubitsToAdd">The number of qubits to add to the register.</param>
        /// <returns>A reference to the expanded register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubitsAtEnd(int NumberOfQubitsToAdd)
        {
            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + NumberOfQubitsToAdd) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, NumberOfQubitsToAdd));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Append a specific number of qubits to the register, initialized to the specified
        /// value.
        /// </summary>
        /// <param name="NumberOfQubitsToAdd">The number of qubits to add to to the 
        /// register.</param>
        /// <param name="InitializeTo">The value to initialze the qubits to.</param>
        /// <returns>A reference to the expanded register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if this insert causes the register to
        /// exceed the maximum allowable size.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister InsertQubitsAtEnd(int NumberOfQubitsToAdd, bool InitializeTo)
        {
            //make sure the max number of qubits isn't exceeded
            if ((this.iTotalNumberOfQubits + NumberOfQubitsToAdd) > MAXIMUM_QUBITS_IN_REGISTER)
                throw new ArgumentOutOfRangeException(string.Format("The maximum number of qubits that can be in a register is {0}. This register currently contains {1} qubits, adding {2} to it exceeds that limit.", MAXIMUM_QUBITS_IN_REGISTER, this.iTotalNumberOfQubits, NumberOfQubitsToAdd));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// It is possible to create a register with references to the same qubit. This
        /// goes through and eliminates duplicate references, leaving the reference at
        /// the lowest index.
        /// </summary>
        /// <returns>A reference to the register with the duplicates eliminated.</returns>
        /// <example>If the register contains qubits ABACDAC then a register containing
        /// only ABCD is returned.</example>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister EliminateDuplicateReferences()
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Slices can be used to obtain subsets of a register. This method returns a register
        /// that contains the whole set of qubits that have been included in this register. This
        /// register will contain the qubits in their original order, regardless of slices. 
        /// </summary>
        /// <returns>The register that contains the whole set of qubits that have ever
        /// been part of this register.</returns>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister GetCompleteRegister()
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }



        /// <summary>
        /// Set a qubit to a particular state.
        /// </summary>
        /// <param name="Index">Index of the qubit to set.</param>
        /// <param name="SetTo">Value to set the qubit to, true (1) or false (0)</param>
        /// <returns>A reference to the modified register.</returns>
        /// <remarks>A qubit is measured before it is set, so any other qubits entangled
        /// with this one will be effected.</remarks>
        /// <exception cref="IndexOutOfRangeException">Thrown if the specified index
        /// is out of range.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister SetQubit(int Index, bool SetTo)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Set the qubits starting at StartingIndex to the classical result SetTo.
        /// </summary>
        /// <param name="StartingIndex">Starting index of the qubits to set.</param>
        /// <param name="SetTo">The classical result which will be read and the applied
        /// to the qubits starting at StartingIndex and in the same order.</param>
        /// <returns>A reference to the modified register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the length of SetTo
        /// exceeds plus StartingIndex exceeds the length of the register. Example: If the
        /// register consists of 5 qubits and StartingIndex is 3 and the length of SetTo is
        /// 20 qubits then this exception will be thrown.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified does
        /// not exist in the register.</exception>
        public IQuantumRegister SetQubits(int StartingIndex, ClassicalResult SetTo)
        {
            IQuantumRegister cWorkingRegister = null;
            ClassicalResult cCollapsedState = null;
            bool[] faCurrentState = null;
            bool[] faSetTo = null;

            //first make sure that the inputs are valid.
            if (StartingIndex > (this.listExposedQubits.Count - 1))
                throw new IndexOutOfRangeException(string.Format("Starting index of {0} is invalid, as the register only goes up to index {1}.", StartingIndex, (this.listExposedQubits.Count - 1)));
            if (SetTo.GetNumberOfBits() > (this.listExposedQubits.Count - StartingIndex))
                throw new ArgumentOutOfRangeException(string.Format("The classical result is {0} bits long, but there is not enough room in the register to set this. This register only has {1} qubits after the starting index {2}", SetTo.ToBoolArray().Length, (this.listExposedQubits.Count - StartingIndex), StartingIndex));

            //first measure from the specified index to the end of the exposed register.
            //this collapses those qubits to an abosulte state.
            cWorkingRegister = this.SliceFrom(StartingIndex);
            cCollapsedState = cWorkingRegister.Measure();

            //now just go through and toggle the bits as needed to match the results. anything
            //not specified in SetTo is set to 0.
            faCurrentState = cCollapsedState.ToBoolArray();
            faSetTo = SetTo.ToBoolArray();
            for (int iCurIndex = 0; iCurIndex < cCollapsedState.GetNumberOfBits(); iCurIndex++)
            {
                //the bit might need to be explicitly set
                if (iCurIndex < faSetTo.Length)
                {
                    if (faCurrentState[iCurIndex] != faSetTo[iCurIndex])    //states don't match, toggle
                    {
                        cWorkingRegister.OperationNot(iCurIndex);
                    }
                }
                else      //else the bit is outside of what was asked to be set, so set to 0
                {
                    if (faCurrentState[iCurIndex] == true)
                    {
                        cWorkingRegister.OperationNot(iCurIndex);    //toggle to false (0)
                    }
                }

            }                //end for iCurIndex

            return this;     //return this register after everything is done
        }


        /// <summary>
        /// Set the qubits starting at StartingIndex to the classical result SetTo.
        /// </summary>
        /// <param name="SetTo">The classical result which will be read and the applied
        /// to the qubits starting at StartingIndex and in the same order.</param>
        /// <returns>A reference to the modified register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the length of SetTo
        /// exceeds plus StartingIndex exceeds the length of the register. Example: If the
        /// register consists of 5 qubits and StartingIndex is 3 and the length of SetTo is
        /// 20 qubits then this exception will be thrown.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified does
        /// not exist in the register.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister SetQubits(ClassicalResult SetTo)
        {
            //just wrap to start at index 0
            return SetQubits(0, SetTo);
        }


        /// <summary>
        /// Set the specified qubits to the value specified.
        /// </summary>
        /// <param name="Indexes">The indexes of the qubits to set.</param>
        /// <param name="SetTo">Value to set the qubits to, true (1) or false (0).</param>
        /// <returns>A reference to the modified register.</returns>
        /// <remarks>A qubit is measured before it is set, so any other qubits entangled
        /// with these will be effected.</remarks>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the specified
        /// indexes are out of range.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister SetQubits(int[] Indexes, bool SetTo)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Set qubits based on the boolean values given.
        /// </summary>
        /// <param name="QubitValues">Values to set each qubit to. Qubit at position x will be set
        /// to the boolean element at position x in QubitValues.</param>
        /// <returns>A reference to the modified register.</returns>
        /// <exception cref="ArgumentNullException">Thrown if QubitValues is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the length of QubitValues
        /// does not match the length of the exposed qubits in the register.</exception>
        public IQuantumRegister SetQubits(bool[] QubitValues)
        {
            long iStateToSet = 0;

            //verify input
            if (QubitValues == null)
                throw new ArgumentNullException("QubitValues cannot be null");
            if (QubitValues.Length != this.listExposedQubits.Count)
                throw new ArgumentOutOfRangeException(string.Format("The length of QubitValues ({0}) must equal the length of exposed qubits ({1}).", QubitValues.Length, this.listExposedQubits.Count));

            //TODO: Take into account that some qubits will not be exposed.

            //it is absolutely at this state, so set it to that and chance of all others is 0
            for (long iCurIndex = 0; iCurIndex < QubitValues.Length; iCurIndex++)
            {
                if (QubitValues[iCurIndex] == true)
                    iStateToSet += (long)Math.Pow(2, iCurIndex);    //always ints
            }

            //now set the state.
            //TODO: Take into account that not all qubits may be exposed. This will involve 
            //      changing what is iterated over?
            for (long iCurIndex = 0; iCurIndex < Math.Pow(2, this.listExposedQubits.Count); iCurIndex++)
            {
                if (iCurIndex == iStateToSet)
                    this.cState.SetValue(iCurIndex, 0, 1);
                else
                    this.cState.SetValue(iCurIndex, 0, 0);
            }

            return this;
        }


        /// <summary>
        /// Set all the qubits of the register to the specified value.
        /// </summary>
        /// <param name="SetTo">Value to set the qubits to, true (1) or false (0).</param>
        /// <returns>A reference to the modified register.</returns>
        /// <remarks>A qubit is measured before it is set, so any other qubits entangled
        /// with this one will be effected.</remarks>
        public IQuantumRegister SetAllQubitsTo(bool SetTo)
        {
            bool[] faValue = new bool[this.listExposedQubits.Count];

            //construct the array of bool values to set the register to and call the more
            //generic set method, which will take into account that not all qubits are exposed.
            for (int iCurIndex = 0; iCurIndex < this.listExposedQubits.Count; iCurIndex++)
                faValue[iCurIndex] = SetTo;

            return this.SetQubits(faValue);
        }


        /// <summary>
        /// Get the length of the quantum register. Note that due to slicing
        /// this length may be smaller than all the qubits actually contained in
        /// the register.
        /// </summary>
        /// <returns>The number of qubits exposed in the register</returns>
        public int GetLength()
        {
            return this.listExposedQubits.Count;
        }


        /// <summary>
        /// Slicing may result in a subset of the register being exposed. This gets the
        /// total length of the register, which includes qubits that may be hidden
        /// through slicing.
        /// </summary>
        /// <returns>The total number of qubits in the register, some of which
        /// may be hidden.</returns>
        public int GetTotalLength()
        {
            return this.iTotalNumberOfQubits;
        }


        /// <summary>
        /// This copies over all the necessary members of the existing register into a new
        /// one so that slice operations only have to worry about manipulating the exposed
        /// qubits. Also makes the expansion of the QuantumRegister class easier since any
        /// copying for slicing can be put here.
        /// </summary>
        /// <returns>The cloned register, except for the list of exposed qubits, which will
        /// be manipulated by the caller.</returns>
        /// <remarks>The state of the qubit is an object reference, so changes to the
        /// returned qubit's state still effect the state of this qubit. Thus no-cloning is not
        /// violated.</remarks>
        protected virtual QuantumRegister CloneForSlice()
        {
            QuantumRegister cRetVal = new QuantumRegister();

            //NOTE: this is an object reference, not a deep copy. that way changes to the returned
            //qubit still alter the state of this register. (as it should)
            cRetVal.cState = this.cState;

            cRetVal.iTotalNumberOfQubits = this.iTotalNumberOfQubits;    //num of total qubits is the same.
            cRetVal.sLocation = this.sLocation;

            return cRetVal;
        }


        /// <summary>
        /// Returns a slice (subset) of the quantum register.
        /// </summary>
        /// <param name="StartIndex">Starting index in the register to get the slice of.</param>
        /// <param name="StopIndex">Ending index in the register to get the slice of.</param>
        /// <returns>A quantum register representing the slice.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the start or stop index parameters
        /// are out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the stop index is less
        /// than the start index.</exception>
        public IQuantumRegister Slice(int StartIndex, int StopIndex)
        {
            QuantumRegister cRetVal = this.CloneForSlice();

            if ((StartIndex < 0) || (StopIndex > (this.listExposedQubits.Count - 1)))
                throw new IndexOutOfRangeException(string.Format("The indexes specified for this slice are invalid. Valid indexes are 0 - {0}, but specified {1} - {2}.", (this.listExposedQubits.Count - 1), StartIndex, StopIndex));
            if (StopIndex < StartIndex)
                throw new ArgumentOutOfRangeException(string.Format("The stop index must be >= the start index. Specified start index of {0} and stop index of {1}.", StartIndex, StopIndex));

            cRetVal.listExposedQubits = this.listExposedQubits.GetRange(StartIndex, (StopIndex - StartIndex + 1));
            return cRetVal;
        }


        /// <summary>
        /// Returns a slice (subset) of the quantum register, but the ordering of the
        /// qubits in the returned register are reversed.
        /// </summary>
        /// <param name="StartIndex">Starting index in the register to get the slice of.</param>
        /// <param name="StopIndex">Ending index in the register to get the slice of.</param>
        /// <returns>The quantum register representing the slice.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the start or stop index parameters
        /// are out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the stop index is less
        /// than the start index.</exception>
        public IQuantumRegister SliceReverse(int StartIndex, int StopIndex)
        {
            QuantumRegister cRetVal = this.CloneForSlice();

            if ((StartIndex < 0) || (StopIndex > (this.listExposedQubits.Count - 1)))
                throw new IndexOutOfRangeException(string.Format("The indexes specified for this slice are invalid. Valid indexes are 0 - {0}, but specified {1} - {2}.", (this.listExposedQubits.Count - 1), StartIndex, StopIndex));
            if (StopIndex < StartIndex)
                throw new ArgumentOutOfRangeException(string.Format("The stop index must be >= the start index. Specified start index of {0} and stop index of {1}.", StartIndex, StopIndex));

            cRetVal.listExposedQubits = this.listExposedQubits.GetRange(StartIndex, (StopIndex - StartIndex + 1));
            cRetVal.listExposedQubits.Reverse();
            return cRetVal;
        }


        /// <summary>
        /// Returns a quantum register with the qubits reversed.
        /// </summary>
        /// <returns>The quantum register representing this register with the qubits reversed.</returns>
        public IQuantumRegister SliceReverse()
        {
            QuantumRegister cRetVal = this.CloneForSlice();

            cRetVal.listExposedQubits = this.listExposedQubits.GetRange(0, this.listExposedQubits.Count);
            cRetVal.listExposedQubits.Reverse();
            return cRetVal;
        }


        /// <summary>
        /// Reorder the qubits in any arbitrary order. NewIndexes is an array of integers that must
        /// equal the length of this register. The qubit at each index in that array will be moved
        /// to the value in the array. Example: the register this is called on contains 3 qubits, indexed
        /// 0 - 2. If the array {2, 0, 1} is passed then: the qubit at index 0 will be moved to index 2;
        /// the qubit at index 1 will be moved to index 0; the qubit at index 2 will be moved to index 1.
        /// </summary>
        /// <param name="NewIndexes">The array of new</param>
        /// <returns>A reference to a register with the reordering performed.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the length of NewIndexes does
        /// not match the length of this register.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if invalid indexes are specified
        /// in NewIndexes</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the same index is specified more than once
        /// in NewIndexes. Example: the NewIndexes is {1, 0, 1} this will be thrown because 1 is
        /// specified twice.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister SliceReorder(IEnumerable<int> NewIndexes)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Returns a slice (subset) of the register from StartIndex to the end.
        /// </summary>
        /// <param name="StartIndex">Starting index in the register to get the slice of.</param>
        /// <returns>The quantum register representing the slice.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the start index is out of range.</exception>
        public IQuantumRegister SliceFrom(int StartIndex)
        {
            QuantumRegister cRetVal = this.CloneForSlice();

            if (StartIndex > (this.listExposedQubits.Count - 1))
                throw new IndexOutOfRangeException(string.Format("The start index of {0} is out of range. The largest index is {1}.", StartIndex, (this.listExposedQubits.Count - 1)));

            cRetVal.listExposedQubits = this.listExposedQubits.GetRange(StartIndex, (this.listExposedQubits.Count - StartIndex));
            return cRetVal;
        }


        /// <summary>
        /// Returns a slice (subset) of the register from the beginning to EndIndex.
        /// </summary>
        /// <param name="EndIndex">The ending index of the slice.</param>
        /// <returns>The quantum register representing the slice.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the end index is out of range.</exception>
        public IQuantumRegister SliceTo(int EndIndex)
        {
            QuantumRegister cRetVal = this.CloneForSlice();

            if (EndIndex > (this.listExposedQubits.Count - 1))
                throw new IndexOutOfRangeException(string.Format("The end index of {0} is out of range. The largest index is {1}.", EndIndex, (this.listExposedQubits.Count - 1)));

            //this slice is performed on the qubits that are exposed, so manipulate that possible
            //slice accordingly.
            cRetVal.listExposedQubits = new List<int>((this.listExposedQubits.GetRange(0, EndIndex + 1)));

            return cRetVal;
        }


        /// <summary>
        /// Returns a slice (subset) of the quantum register containing the qubits
        /// specified in Indexes, and in that order.
        /// </summary>
        /// <param name="Indexes">The indexes of the qubits in the register being
        /// returned.</param>
        /// <returns>The quantum register representing the subset.</returns>
        /// <exception cref="ArgumentNullException">Thrown if a null array is passed.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes specified are
        /// outside of the allowable range.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the same index is specified more than
        /// once.</exception>
        public IQuantumRegister SliceSubset(IEnumerable<int> Indexes)
        {
            QuantumRegister cRetVal = this.CloneForSlice();
            Dictionary<int, bool> dictAlreadyUsed = new Dictionary<int, bool>();

            if (Indexes == null)
                throw new ArgumentNullException("The indexes cannot be null for the SliceSubset() call.");

            //this slice is performed on the qubits that are exposed, so manipulate that possible
            //slice accordingly.
            cRetVal.listExposedQubits = new List<int>();       //clear it to build it up
            foreach (int iCurIndex in Indexes)
            {
                if ((iCurIndex < 0) || (iCurIndex > (this.listExposedQubits.Count - 1)))
                    throw new IndexOutOfRangeException(string.Format("An invalid index, {0}, was specified. The valid range is 0 to {1}.", iCurIndex, (this.listExposedQubits.Count - 1)));
                if (dictAlreadyUsed.ContainsKey(iCurIndex) == true)
                    throw new DuplicateIndexesException(string.Format("You cannot use the same index, {0}, more than once.", iCurIndex));

                cRetVal.listExposedQubits.Add(this.listExposedQubits[iCurIndex]);
            }        //end of iteration through Indexes

            return cRetVal;
        }


        /// <summary>
        /// Slice (subset) a quantum register and perform an operation on the slice.
        /// </summary>
        /// <param name="StartIndex">Starting index in the register to get the slice of.</param>
        /// <param name="StopIndex">Ending index in the register to get the slice of.</param>
        /// <param name="Operation">The operation to apply to the slice.</param>
        /// <returns>A quantum register representing the slice with the operation 
        /// applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the start or stop index parameters
        /// are out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the stop index is less
        /// than the start index.</exception>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister Slice(int StartIndex, int StopIndex, IQuantumOperation Operation)
        {
            IQuantumRegister cRetVal = this.Slice(StartIndex, StopIndex);
            cRetVal.ApplyOperation(Operation);

            return cRetVal;
        }


        /// <summary>
        /// Returns a slice (subset) of the quantum register, but the ordering of the
        /// qubits in the returned register are reversed then have the specified operation
        /// applied.
        /// </summary>
        /// <param name="StartIndex">Starting index in the register to get the slice of.</param>
        /// <param name="StopIndex">Ending index in the register to get the slice of.</param>
        /// <param name="Operation">The operation to apply to the slice.</param>
        /// <returns>The quantum register representing the slice with the operation 
        /// applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the start or stop index parameters
        /// are out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the stop index is less
        /// than the start index.</exception>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister SliceReverse(int StartIndex, int StopIndex, IQuantumOperation Operation)
        {
            IQuantumRegister cRetVal = this.SliceReverse(StartIndex, StopIndex);
            cRetVal.ApplyOperation(Operation);

            return cRetVal;
        }


        /// <summary>
        /// Returns a quantum register with the qubits reversed, then the operation applied.
        /// </summary>
        /// <param name="Operation">The operation to apply to the reversed slice.</param>
        /// <returns>The quantum register representing this register with the qubits reversed
        /// and then the operation applied.</returns>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister SliceReverse(IQuantumOperation Operation)
        {
            IQuantumRegister cRetVal = this.SliceReverse();
            cRetVal.ApplyOperation(Operation);

            return cRetVal;
        }


        /// <summary>
        /// Returns a slice (subset) of the register from StartIndex to the end, then has
        /// the operation applied to it.
        /// </summary>
        /// <param name="StartIndex">Starting index in the register to get the slice of.</param>
        /// <param name="Operation">The operation to apply to the slice.</param>
        /// <returns>The quantum register representing the slice with the operation then applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the start index is out of range.</exception>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister SliceFrom(int StartIndex, IQuantumOperation Operation)
        {
            IQuantumRegister cRetVal = this.SliceFrom(StartIndex);
            cRetVal.ApplyOperation(Operation);

            return cRetVal;
        }


        /// <summary>
        /// Returns a slice (subset) of the register from the beginning to EndIndex, then
        /// has the operation applied to it.
        /// </summary>
        /// <param name="EndIndex">The ending index of the slice.</param>
        /// <param name="Operation">The operation to apply to the slice.</param>
        /// <returns>The quantum register representing the slice.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the end index is out of range.</exception>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister SliceTo(int EndIndex, IQuantumOperation Operation)
        {
            IQuantumRegister cRetVal = this.SliceTo(EndIndex);
            cRetVal.ApplyOperation(Operation);

            return cRetVal;
        }


        /// <summary>
        /// Returns a slice (subset) of the quantum register containing the qubits
        /// specified in Indexes, and in that order. The operation is then
        /// applied to the slice.
        /// </summary>
        /// <param name="Indexes">The indexes of the qubits in the register being
        /// returned.</param>
        /// <param name="Operation">The operation to apply to the slice.</param>
        /// <returns>The quantum register representing the subset.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes specified are
        /// outside of the allowable range.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the same index is specified more than
        /// once.</exception>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister SliceSubset(IEnumerable<int> Indexes, IQuantumOperation Operation)
        {
            IQuantumRegister cRetVal = this.SliceSubset(Indexes);
            cRetVal.ApplyOperation(Operation);

            return cRetVal;
        }


        /// <summary>
        /// Slice (subset) a quantum register and perform operations on the slice.
        /// </summary>
        /// <param name="StartIndex">Starting index in the register to get the slice of.</param>
        /// <param name="StopIndex">Ending index in the register to get the slice of.</param>
        /// <param name="Operations">The operation sto apply to the slice.</param>
        /// <returns>A quantum register representing the slice with the operations 
        /// applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the number of qubits
        /// any of the operations operates on does not match the size of the slice.
        /// </exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the start or stop index parameters
        /// are out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the stop index is less
        /// than the start index.</exception>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister Slice(int StartIndex, int StopIndex, IEnumerable<IQuantumOperation> Operations)
        {
            QuantumRegister cRetVal = (QuantumRegister)this.Slice(StartIndex, StopIndex);
            cRetVal.ApplyOperations(Operations);

            return cRetVal;
        }


        /// <summary>
        /// Returns a slice (subset) of the quantum register, but the ordering of the
        /// qubits in the returned register are reversed then have the specified operations
        /// applied.
        /// </summary>
        /// <param name="StartIndex">Starting index in the register to get the slice of.</param>
        /// <param name="StopIndex">Ending index in the register to get the slice of.</param>
        /// <param name="Operations">The operations to apply to the slice.</param>
        /// <returns>The quantum register representing the slice with the operations 
        /// applied.</returns>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister SliceReverse(int StartIndex, int StopIndex, IEnumerable<IQuantumOperation> Operations)
        {
            IQuantumRegister cRetVal = this.SliceReverse(StartIndex, StopIndex);
            cRetVal.ApplyOperations(Operations);

            return cRetVal;
        }


        /// <summary>
        /// Returns a quantum register with the qubits reversed, then the operations applied.
        /// </summary>
        /// <param name="Operations">The operations to apply to the reversed slice.</param>
        /// <returns>The quantum register representing this register with the qubits reversed
        /// and then the operations applied.</returns>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister SliceReverse(IEnumerable<IQuantumOperation> Operations)
        {
            IQuantumRegister cRetVal = this.SliceReverse();
            cRetVal.ApplyOperations(Operations);

            return cRetVal;
        }


        /// <summary>
        /// Returns a slice (subset) of the register from StartIndex to the end, then has
        /// the operations applied to it.
        /// </summary>
        /// <param name="StartIndex">Starting index in the register to get the slice of.</param>
        /// <param name="Operations">The operations to apply to the slice.</param>
        /// <returns>The quantum register representing the slice with the operation then applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the number of qubits
        /// any of the operations operates on does not match the size of the slice.
        /// </exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the start index is out of range.</exception>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister SliceFrom(int StartIndex, IEnumerable<IQuantumOperation> Operations)
        {
            QuantumRegister cRetVal = (QuantumRegister)this.SliceFrom(StartIndex);
            cRetVal.ApplyOperations(Operations);

            return cRetVal;
        }


        /// <summary>
        /// Returns a slice (subset) of the quantum register containing the qubits
        /// specified in Indexes, and in that order. The specified operations are
        /// then applied.
        /// </summary>
        /// <param name="Indexes">The indexes of the qubits in the register being
        /// returned.</param>
        /// <param name="Operations">The operations to apply to the slice.</param>
        /// <returns>The quantum register representing the subset.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes specified are
        /// outside of the allowable range.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the same index is specified more than
        /// once.</exception>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister SliceSubset(IEnumerable<int> Indexes, IEnumerable<IQuantumOperation> Operations)
        {
            IQuantumRegister cRetVal = this.SliceSubset(Indexes);
            cRetVal.ApplyOperations(Operations);

            return cRetVal;
        }


        /// <summary>
        /// Returns a slice (subset) of the register from the beginning to EndIndex, then
        /// has the operations applied to it.
        /// </summary>
        /// <param name="EndIndex">The ending index of the slice.</param>
        /// <param name="Operations">The operations to apply to the slice.</param>
        /// <returns>The quantum register representing the slice.</returns>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister SliceTo(int EndIndex, List<IQuantumOperation> Operations)
        {
            IQuantumRegister cRetVal = this.SliceTo(EndIndex);
            cRetVal.ApplyOperations(Operations);

            return cRetVal;
        }


        #region "Applying quantum operations to registers"

        /// <summary>
        /// Apply a generic operation to the register.
        /// </summary>
        /// <param name="Operation">The operation to apply</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister ApplyOperation(IQuantumOperation Operation)
        {
            ComplexMatrix cAdjusted = null;
            Complex cTargetCell = null;
            List<int> listAppliedIndices = null;
            int iTargetRow = 0;
            int iTargetColumn = 0;

            //validate the operator before trying to do anything with it.
            if ((Operation is GeneralSimulatedOperation) == false)
                throw new ArgumentException("Cove.LocalSimulation.QuantumRegister is only able to apply GeneralSimulatedOperation derived operations");
            if ((Operation as GeneralSimulatedOperation).NumberOfQubitsOperatesOn() > this.listExposedQubits.Count)
                throw new SizeMismatchException(string.Format("This register has {0} qubits exposed and the operation operates on at least {1} qubits. The register must be at least as big as the operation to apply the operation.", this.listExposedQubits.Count, (Operation as GeneralSimulatedOperation).NumberOfQubitsOperatesOn()));
            //TODO: Add this check back in
            //if (Operation.IsValidOperation() == false)
            //    throw new NotUnitaryOperationException(string.Format("The operation {0} is not a unitary operation, and thus cannot be passed as an operation to a quantum register. All operations must be unitary.", Operation));

            //wait till after the checks to create the adjusted operations marix
            cAdjusted = new ComplexMatrix((int)Math.Pow(2, this.iTotalNumberOfQubits),
                (int)Math.Pow(2, this.iTotalNumberOfQubits));

            listAppliedIndices = this.GetAppliedIndices((Operation as GeneralSimulatedOperation).listTargetQubits);

            //adjust the operation matrix for the size of the register and possible slicing.
            for (int iCurColumn = 0; iCurColumn < cAdjusted.GetNumberOfColumns(); iCurColumn++)
            {
                for (int iCurRow = 0; iCurRow < cAdjusted.GetNumberOfRows(); iCurRow++)
                {
                    //if they differ in their binary representations then the cell is 0
                    if (this.NotExposedBinaryDiffers(listAppliedIndices, iCurColumn, iCurRow) == true)
                    {
                        cAdjusted.SetValue(iCurRow, iCurColumn, new Complex(0));
                    }    //end if they differ
                    else      //they don't differ, so set the cell accordingly.
                    {
                        iTargetColumn = this.GetIncludedBinaryRepresentation(listAppliedIndices, iCurColumn);
                        iTargetRow = this.GetIncludedBinaryRepresentation(listAppliedIndices, iCurRow);
                        cTargetCell = (Operation as GeneralSimulatedOperation).OperationMatrix.GetValue(
                            iTargetRow, iTargetColumn);
                        cAdjusted.SetValue(iCurRow, iCurColumn, new Complex(cTargetCell.GetReal(), cTargetCell.GetImaginary()));
                    }    //end else they don't differ
                }        //end of iteration through rows
            }            //end of iteration through columns

            //at this point the size of the register and operator should be the same size
            //(row wise). if not then there was a problem with the above
            if (cAdjusted.GetNumberOfRows() != this.cState.GetNumberOfRows())
                throw new ImplementationException(string.Format("There is an internal error in Cove.LocalSimulation.QuantumRegister.ApplyOperation(IQuantumOperation Operation) where the register and operation sizes do not match. Operation rows = {0}, columns = {1}. State rows = {2}, columns = {3}.", cAdjusted.GetNumberOfRows(), cAdjusted.GetNumberOfColumns(), cState.GetNumberOfRows(), cState.GetNumberOfColumns()));

            //finally apply the operation
            this.cState.MultiplyAsRightSide(cAdjusted);

            //if this is done cState is replaced with the result, so any other references to this.cState
            //remains unchanged. so don't do it or other slices of this register won't be effected.
            //this.cState = cAdjusted * this.cState;

            return this;
        }


        /// <summary>
        /// Get how the exposed indices in the register match up to the target indices
        /// in the operation. The result can be used in matrix expansion and reordering.
        /// </summary>
        /// <param name="OperationTargets">The target qubits for the operation. The
        /// element number is the position in the standard application, the value
        /// is the target. So if the operation is in standard order then the value
        /// of each element is the index, ie ActualAppliedQubits[x] = x.</param>
        /// <returns>The correct ordering that the operation applies to.</returns>
        protected internal List<int> GetAppliedIndices(List<int> OperationTargets)
        {
            List<int> listRetVal = new List<int>();

            for (int iCurTargetIndex = 0; iCurTargetIndex < OperationTargets.Count; iCurTargetIndex++)
            {
                listRetVal.Add(this.listExposedQubits[OperationTargets[iCurTargetIndex]]);
            }                //end of iteration through iCurTargetIndex

            return listRetVal;
        }


        /// <summary>
        /// Do the binary representations of FirstValue and SecondValue differ
        /// in the positions that are not exposed?
        /// </summary>
        /// <param name="ActualAppliedQubits">The targets of the operation, in the 
        /// order that they are applied. The value of each element is the target qubit. </param>
        /// <param name="FirstValue">First value to compare</param>
        /// <param name="SecondValue">Second value to compare</param>
        /// <returns>True if they differ in the positions that are not exposed
        /// in the register.</returns>
        protected internal bool NotExposedBinaryDiffers(List<int> ActualAppliedQubits, int FirstValue, int SecondValue)
        {
            Dictionary<int, bool> dictAcutalAppliedIndices = new Dictionary<int, bool>();
            int iFirstBit = 0;
            int iSecondBit = 0;

            //construct a hash table for quick lookups
            foreach (int iCurActualAppliedIndex in ActualAppliedQubits)
            {
                dictAcutalAppliedIndices[iCurActualAppliedIndex] = true;
            }

            //iterate through all the qubits in the register, although only a subset may be exposed.
            for (int iCurTotalQubit = 0; iCurTotalQubit < this.iTotalNumberOfQubits; iCurTotalQubit++)
            {
                //want to see if the actual qubit being checked this iteration is either:
                //1) not exposed
                //2) if it is exposed, is it not targetted
                if (dictAcutalAppliedIndices.ContainsKey(iCurTotalQubit) == false)
                {
                    //since this is a qubit to examine, see if the values are the same.
                    iFirstBit = ((int)Math.Pow(2, iCurTotalQubit)) & FirstValue;
                    iSecondBit = ((int)Math.Pow(2, iCurTotalQubit)) & SecondValue;
                    if (iFirstBit != iSecondBit)
                    {
                        return true;
                    }
                }
            }                  //end of iteration through qubits

            //made it to here then they don't differ
            return false;
        }


        /// <summary>
        /// Get the binary representation of the exposed qubits that would be 
        /// operated on by an operation.
        /// </summary>
        /// <param name="OperationTargets">The target qubits of the operation. The value is 
        /// the target qubit in the complete and unsliced register.</param>
        /// <param name="Value">The row or column that would be operated on.</param>
        /// <returns>The binary representation of the exposed qubits.</returns>
        protected internal int GetIncludedBinaryRepresentation(List<int> OperationTargets, int Value)
        {
            int iRetVal = 0;
            int iCurBit = 0;

            //go through the exposed qubits in the order they're exposed in
            for (int iCurExposedQubit = 0; iCurExposedQubit < OperationTargets.Count; iCurExposedQubit++)
            {
                //is the exposed bit set in i? if so then concate it
                iCurBit = (int)Math.Pow(2, OperationTargets[iCurExposedQubit]);
                if ((Value & iCurBit) == iCurBit)
                {
                    //want to set the right bit in the result
                    iRetVal = iRetVal | ((int)Math.Pow(2, iCurExposedQubit));
                }
            }                //end of iteration through exposed qubits

            return iRetVal;
        }


        /// <summary>
        /// Get the binrary representations of the indices that are not included
        /// in Value.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected int GetNotIncludedBinaryRepresentation(int Value)
        {
            int iRetVal = 0;

            //4.	Otherwise concatenate bits from the binary representation of i, 
            //in the positions referenced by the indices in Q (in numerical order), to get  . 
            //Do the same for j to get  . Then set  .
            for (int iCurExposedQubit = 0; iCurExposedQubit < this.listExposedQubits.Count; iCurExposedQubit++)
            {
                iCurExposedQubit = (int)Math.Pow(2, this.listExposedQubits[iCurExposedQubit]);
                if ((Value & iCurExposedQubit) == iCurExposedQubit)
                {
                    iRetVal = iRetVal | iCurExposedQubit;
                }
            }                //end of iteration through exposed qubits

            return iRetVal;
        }


        /// <summary>
        /// Apply a generic operation to the register.
        /// </summary>
        /// <param name="OperationMatrix">The operation to apply</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the number of qubits
        /// the operation applies to does not match the number of qubits in the
        /// register.</exception>
        /// <exception cref="ArgumentException">Thrown if the number of rows
        /// and columns in OperationMatrix are not equal.</exception>
        /// <exception cref="ArgumentException">Thrown if the number of rows and
        /// columns in OpeartionMatrix are not a power of two.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if OperationMatrix doesn't
        /// match the size of the exposed qubits.</exception>
        protected IQuantumRegister ApplyOperation(ComplexMatrix OperationMatrix)
        {
            //apply this matrix by essentially creating a standard op with the matrix 
            //as the state and the applied qubits in order.
            GeneralSimulatedOperation cCustomOperation = new GeneralSimulatedOperation();

            if (OperationMatrix.GetNumberOfColumns() != OperationMatrix.GetNumberOfRows())
                throw new ArgumentException("The number of rows and columns in OperationMatrix must be equal");
            if (ClassicalUtilities.ClassicalMath.IsPowerOf(2, OperationMatrix.GetNumberOfRows()) == false)
                throw new ArgumentException("The number of rows and columns in the operation matrix must be a power of two.");
            if ((int)Math.Pow(2, this.listExposedQubits.Count) != OperationMatrix.GetNumberOfRows())
                throw new ArgumentOutOfRangeException(string.Format("The OperationMatrix size ({0} x {0}) doesn't match the expected size ({1} x {1}) for a {2} qubit register.", OperationMatrix.GetNumberOfRows(), ((int)Math.Pow(2, this.listExposedQubits.Count)), this.listExposedQubits.Count));

            cCustomOperation.OperationMatrix = OperationMatrix;
            cCustomOperation.listTargetQubits = new List<int>();

            //the target index is 1 less than the size (iTargetIndex 0 is a register of 1 size). already checked
            //above that the matrix size is a power of 2, so once it 2^iTargetIndex reaches that size we're done.
            //(<= instead of < makes it 1 register too big since 1 more target index is added.)
            for (int iTargetIndex = 0; (int)Math.Pow(2, iTargetIndex) < OperationMatrix.GetNumberOfColumns(); iTargetIndex++)
            {
                cCustomOperation.listTargetQubits.Add(iTargetIndex);
            }

            return this.ApplyOperation(cCustomOperation);
        }


        /// <summary>
        /// Apply a generic operation to the specified qubits.
        /// </summary>
        /// <param name="Operation">Operation to apply to the qubit(s)</param>
        /// <param name="Indexes">Indexes to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes
        /// are out of range.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the operation does not 
        /// size does not match the size of the register.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister ApplyOperation(IQuantumOperation Operation, int[] Indexes)
        {
            if (Operation.IsValidOperation() == false)
                throw new NotUnitaryOperationException(string.Format("The operation {0} is not a unitary operation, and thus cannot be passed as an operation to a quantum register. All operations must be unitary.", Operation));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Apply the single qubit operation to all qubits in the register.
        /// </summary>
        /// <param name="Operation">Operation to apply to all the qubits.</param>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister ApplyOperationAll(IQubitOperation Operation)
        {
            if (Operation.IsValidOperation() == false)
                throw new NotUnitaryOperationException(string.Format("The operation {0} is not a unitary operation, and thus cannot be passed as an operation to a quantum register. All operations must be unitary.", Operation));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Apply a series of operations to the quantum register. If any of the operations
        /// are invalid and an exception is thrown then the state of the register is not altered.
        /// </summary>
        /// <param name="Operations">Operations to apply. They will be applied
        /// in order from lowest index to highest.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        /// <exception cref="ArgumentException">Thrown if the operation does not have
        /// AbstractSimulatedQuantumOperation as a base class.</exception>
        public IQuantumRegister ApplyOperations(IEnumerable<IQuantumOperation> Operations)
        {
            //validate all ops before applying them. if they are checked as they are applied there
            //could be an invalid one after some are applied, meaning that others have been applied
            //and the state has been altered. 
            foreach (IQuantumOperation cCurOp in Operations)
            {
                if ((cCurOp is GeneralSimulatedOperation) == false)
                    throw new ArgumentException("Cove.LocalSimulation.QuantumRegister is only able to apply GeneralSimulatedOperation derived operations");
                //TODO: Add this back in
                //if (cCurOp.IsValidOperation() == false)
                //    throw new NotUnitaryOperationException("One of the operations passed was not unitary. All quantum operations must be unitary");
                if (cCurOp.NumberOfQubitsOperatesOn() > this.GetLength())
                    throw new SizeMismatchException(string.Format("Opeartion size ({0}) is greater than the size of the register ({1}). Operations must be smaller or of equal size.", cCurOp.NumberOfQubitsOperatesOn(), this.GetLength()));
            }           //end of iteration for validation

            //now apply the operations
            foreach (IQuantumOperation cCurOp in Operations)
            {
                this.ApplyOperation(cCurOp);
            }

            return this;
        }


        /// <summary>
        /// Perform the TargetOperation on all qubits except the qubit at ControlIndex
        /// if the qubit at ControlIndex is |1>. Else there is no change in the register.
        /// This arbitrary application of a single qubit operator to n qubits in a register
        /// based on a control qubit is called ControlU.
        /// </summary>
        /// <param name="TargetOperation">The operation to apply to all qubits except the
        /// qubit at ControlIndex.</param>
        /// <param name="ControlIndex">The index of the qubit that will serve as the
        /// control. If this is |1> then the TargetOperation is applied to all other
        /// qubits.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if ControlIndex is out
        /// of range.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationControlledU(IQubitOperation TargetOperation, int ControlIndex)
        {
            if (TargetOperation.IsValidOperation() == false)
                throw new NotUnitaryOperationException(string.Format("The operation {0} is not a unitary operation, and thus cannot be passed as an operation to a quantum register. All operations must be unitary.", TargetOperation));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Perform the TargetOperation on the qubits at TargetIndexes if ControlIndex is |1>.
        /// </summary>
        /// <param name="TargetOperation">The operation to apply to the target qubits.</param>
        /// <param name="ControlIndex">Index of the control qubit.</param>
        /// <param name="TargetIndexes">Indexes of the target qubits</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes are
        /// out of range.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the same qubit
        /// is specified as control and target.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationControlledU(IQubitOperation TargetOperation, int ControlIndex, int[] TargetIndexes)
        {
            if (TargetOperation.IsValidOperation() == false)
                throw new NotUnitaryOperationException(string.Format("The operation {0} is not a unitary operation, and thus cannot be passed as an operation to a quantum register. All operations must be unitary.", TargetOperation));

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Apply the CNot operation to the register. (Must be a 2 qubit register.)
        /// The first qubit is the control and the second is the target.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the number of qubits
        /// the operation applies to does not match the number of qubits in the
        /// register.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the indexes specified
        /// refer to the same qubit.</exception>
        public IQuantumRegister OperationCNot()
        {
            //confirm sizes of opeartion and register match
            if (this.listExposedQubits.Count != 2)
                throw new SizeMismatchException(string.Format("The CNot operation applies to two qubits. This register exposes {0} qubits.", this.listExposedQubits.Count));

            this.ApplyOperation(new OperationCNot());

            return this;
        }


        /// <summary>
        /// Apply the CNot operation to the register. 
        /// </summary>
        /// <param name="ControlIndex">Index of the qubit which will be the control.</param>
        /// <param name="TargetIndex">Index of the qubit which will be the target.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if either of the indexes
        /// are invalid for this register.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the ControlIndex and
        /// TargetIndex specify the same qubit.</exception>
        public IQuantumRegister OperationCNot(int ControlIndex, int TargetIndex)
        {
            IOperationCNot cCNot = new OperationCNot(ControlIndex, TargetIndex);

            return this.ApplyOperation(cCNot);
        }


        /// <summary>
        /// Apply the CNot operation to the register. (Must be a 2 qubit register.)
        /// The first qubit is the control and the second is the target.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the number of qubits
        /// the operation applies to does not match the number of qubits in the
        /// register.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the indexes
        /// are invalid for this register.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the control and
        /// target qubits are the same.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationCNot(int[] Indexes)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Perform the Fredkin operation (controlled swap). 
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if this is not a register of 3 qubits</exception>
        public IQuantumRegister OperationFredkin()
        {
            //confirm sizes of opeartion and register match
            if (this.listExposedQubits.Count != 3)
                throw new SizeMismatchException(string.Format("The Fredkin operation applies to 3 qubits. This register exposes {0} qubits.", this.listExposedQubits.Count));

            this.ApplyOperation(new OperationFredkin());

            return this;
        }


        /// <summary>
        /// Perform the Fredkin operation (controlled swap).
        /// </summary>
        /// <param name="ControlIndex">Index of the control qubit.</param>
        /// <param name="XIndex">qubit to swap</param>
        /// <param name="YIndex">qubit to swap</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the indexes specified do not
        /// exist in the register.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the indexes specified
        /// refer to the same qubit.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationFredkin(int ControlIndex, int XIndex, int YIndex)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Perform the Fredkin operation (controlled swap).
        /// </summary>
        /// <param name="Indexes">Indexes of the qubits to perform the operation on.
        /// The first is the control, the second and third are the ones to swap.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the indexes specified do not
        /// exist in the register.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the indexes specified
        /// refer to the same qubit.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationFredkin(int[] Indexes)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Perform the Hadamard (square root of Not) operation on a qubit.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register contains
        /// more than one qubit.</exception>
        public IQuantumRegister OperationHadamard()
        {
            //confirm sizes of opeartion and register match
            if (this.listExposedQubits.Count != 1)
                throw new SizeMismatchException(string.Format("The Hadamard operation applies to a single qubit. This register exposes {0} qubits.", this.listExposedQubits.Count));

            this.ApplyOperation(new OperationHadamard());

            return this;
        }


        /// <summary>
        /// Apply the Hadamard (square root of Not) operation to the specified qubit.
        /// </summary>
        /// <param name="Index">Index to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index does not exist in
        /// the register.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationHadamard(int Index)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Perform the Hadamard (square root of Not) operation to the specified qubits.
        /// </summary>
        /// <param name="Indexes">Indexes of qubits to apply the operations to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the indexes do not exist
        /// in the register.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationHadamard(int[] Indexes)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Apply the Hadamard (square root of Not) operation to all the qubits in the
        /// register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        public IQuantumRegister OperationHadamardAll()
        {
            ComplexMatrix cApplyOperation = (new OperationHadamard()).OperationMatrix;
            ComplexMatrix cSingleQubitOp = (new OperationHadamard()).OperationMatrix;

            //expand the op to apply to all the exposed qubits. start at 1 since the first op counts
            //as the first qubit
            for (int iExposedSize = 1; iExposedSize < this.listExposedQubits.Count; iExposedSize++)
                cApplyOperation = cApplyOperation.Tensor(cSingleQubitOp);

            this.ApplyOperation(cApplyOperation);

            return this;
        }


        /// <summary>
        /// Apply the identity operation to a qubit.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register contains
        /// more than one qubit.</exception>
        public IQuantumRegister OperationIdentity()
        {
            OperationIdentity cOperation = new OperationIdentity();

            //apply the operation
            this.ApplyOperation(cOperation);

            return this;
        }


        /// <summary>
        /// Apply the identity operation to the qubit at the specified index.
        /// </summary>
        /// <param name="Index">Index of the qubit to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index of the qubit
        /// is out of range in the register.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationIdentity(int Index)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Apply the identity operation to the qubits at the specified indexes.
        /// </summary>
        /// <param name="Indexes">Indexes to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes
        /// specified are out of range.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationIdentity(int[] Indexes)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Apply the identity operation to all the qubits in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        public IQuantumRegister OperationIdentityAll()
        {
            ComplexMatrix cApplyOperation = (new OperationIdentity()).OperationMatrix;
            ComplexMatrix cSingleQubitOp = (new OperationIdentity()).OperationMatrix;

            //expand the op to apply to all the exposed qubits. start at 1 since the first op counts
            //as the first qubit
            for (int iExposedSize = 1; iExposedSize < this.listExposedQubits.Count; iExposedSize++)
                cApplyOperation = cApplyOperation.Tensor(cSingleQubitOp);

            this.ApplyOperation(cApplyOperation);

            return this;
        }


        /// <summary>
        /// Apply the Not (X gate) operation to the qubit.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register contains
        /// more than one qubit.</exception>
        public IQuantumRegister OperationNot()
        {
            //confirm sizes of opeartion and register match
            if (this.listExposedQubits.Count != 1)
                throw new SizeMismatchException(string.Format("The Not operation applies to a single qubit. This register exposes {0} qubits.", this.listExposedQubits.Count));

            this.ApplyOperation(new OperationNot());

            return this;
        }


        /// <summary>
        /// Apply the Not (X gate) operation to the qubit at the specified index.
        /// </summary>
        /// <param name="Index">Index of the qubit to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown in the index specified
        /// is out of range.</exception>
        public IQuantumRegister OperationNot(int Index)
        {
            this.SliceSubset(new int[] { Index }, new OperationNot());

            return this;
        }


        /// <summary>
        /// Apply the Not (X gate) operation to the qubits at the specified indexes.
        /// </summary>
        /// <param name="Indexes">Indexes of qubits to apply the operations to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes are
        /// out of range.</exception>
        public IQuantumRegister OperationNot(int[] Indexes)
        {
            foreach (int iCurIndex in Indexes)
                this.OperationNot(iCurIndex);

            return this;
        }


        /// <summary>
        /// Apply the Not (X gate) operation to all the qubits in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        public IQuantumRegister OperationNotAll()
        {
            ComplexMatrix cApplyOperation = (new OperationNot()).OperationMatrix;
            ComplexMatrix cSingleQubitOp = (new OperationNot()).OperationMatrix;

            //expand the op to apply to all the exposed qubits. start at 1 since the first op counts
            //as the first qubit
            for (int iExposedSize = 1; iExposedSize < this.listExposedQubits.Count; iExposedSize++)
                cApplyOperation = cApplyOperation.Tensor(cSingleQubitOp);

            this.ApplyOperation(cApplyOperation);

            return this;
        }


        /// <summary>
        /// Apply the Quantum Fourier Transformation (QFT) to the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        public IQuantumRegister OperationQuantumFourierTransform()
        {
            List<IQuantumOperation> listOperations = null;

            //get and apply the operations
            listOperations = this.GetQuantumFourierTransformOperations();
            this.ApplyOperations(listOperations);

            return this;
        }


        /// <summary>
        /// Perform the inverse Quantum Fourier Transform on the register.
        /// </summary>
        /// <returns>The register after the operation has been applied.</returns>
        public IQuantumRegister OperationInverseQuantumFourierTransform()
        {
            List<IQuantumOperation> listOperations = null;

            //get and apply the operations in reverse since this is the inverse.
            listOperations = this.GetQuantumFourierTransformOperations();
            listOperations.Reverse();
            this.ApplyOperations(listOperations);

            return this;
        }


        /// <summary>
        /// Get the operations that apply the Quantum Fourier Transform. These are then
        /// applied in order for the normal QFT, or reversed and applied for the inverse
        /// QFT. Thus the application of (inverse) QFT is merely the application of these
        /// operations.
        /// </summary>
        /// <returns>The operations to perform the normal QFT.</returns>
        protected List<IQuantumOperation> GetQuantumFourierTransformOperations()
        {
            List<IQuantumOperation> listRetVal = new List<IQuantumOperation>();

            //iterate through the exposed qubits 0 - n. 
            for (int iCurTargetQubit = 0; iCurTargetQubit < this.GetLength(); iCurTargetQubit++)
            {
                //first apply the Hadamard operation to the current qubit
                listRetVal.Add(new OperationHadamard(iCurTargetQubit));

                //now need to apply Control-Rk to this qubit with the target always being
                //this qubit and the control being each qubit of higher index (if any).
                //on the last qubit this will fall through, leaving just Hadamard applied.
                for (int iCurControlQubit = (iCurTargetQubit + 1); iCurControlQubit < this.GetLength(); iCurControlQubit++)
                {
                    listRetVal.Add(new OperationControlledU(
                        new OperationRotateK((iCurControlQubit - iCurTargetQubit) + 1),
                        iCurControlQubit, iCurTargetQubit));
                }            //end for iCurControlQubit
            }                //end for iCurQubit

            //need to reverse the qubits, do it by utilizing swap.
            //NOTE: integer division in the comparision intentionally so iteration will stop
            //      at the index before the half way point if the length is odd.
            for (int iCurSwapQubit = 0; iCurSwapQubit < (this.GetLength() / 2); iCurSwapQubit++)
            {
                listRetVal.Add(new OperationSwap(iCurSwapQubit, (this.GetLength() - iCurSwapQubit)));
            }                //end for iCurSwapQubit

            return listRetVal;
        }


        /// <summary>
        /// Apply the S gate (phase gate) to the qubit.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register contains
        /// more than one qubit.</exception>
        public IQuantumRegister OperationSGate()
        {
            //confirm sizes of opeartion and register match
            if (this.listExposedQubits.Count != 1)
                throw new SizeMismatchException(string.Format("The S gate operation applies to a single qubit. This register exposes {0} qubits.", this.listExposedQubits.Count));

            this.ApplyOperation(new OperationSGate());

            return this;
        }


        /// <summary>
        /// Apply the S gate (phase gate) to the qubit at the specified index.
        /// </summary>
        /// <param name="Index">Index of the qubit to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is out of range.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationSGate(int Index)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Apply the S gate (phase gate) to the qubits at the indexes specified.
        /// </summary>
        /// <param name="Indexes">Indexes of the qubits to apply the indexes to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes
        /// specified are out of range.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationSGate(int[] Indexes)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Apply the S gate (phase gate) operation to all the qubits in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        public IQuantumRegister OperationSGateAll()
        {
            ComplexMatrix cApplyOperation = (new OperationSGate()).OperationMatrix;
            ComplexMatrix cSingleQubitOp = (new OperationSGate()).OperationMatrix;

            //expand the op to apply to all the exposed qubits. start at 1 since the first op counts
            //as the first qubit
            for (int iExposedSize = 1; iExposedSize < this.listExposedQubits.Count; iExposedSize++)
                cApplyOperation = cApplyOperation.Tensor(cSingleQubitOp);

            this.ApplyOperation(cApplyOperation);

            return this;
        }


        /// <summary>
        /// Apply the swap operation to the qubits in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register does not
        /// contain two qubits.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the indexes specified
        /// refer to the same qubit.</exception>
        public IQuantumRegister OperationSwap()
        {
            //confirm sizes of opeartion and register match
            if (this.listExposedQubits.Count != 2)
                throw new SizeMismatchException(string.Format("The Swap operation applies to 2 qubits. This register exposes {0} qubits.", this.listExposedQubits.Count));

            this.ApplyOperation(new OperationSwap());

            return this;
        }


        /// <summary>
        /// Perform the swap operation on the two qubits specified.
        /// </summary>
        /// <param name="FirstSwapIndex">Index of the first qubit to swap.</param>
        /// <param name="SecondSwapIndex">Index of the second qubit to swap.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if one of the indexes
        /// is out of range.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the indexes specified
        /// refer to the same qubit.</exception>
        public IQuantumRegister OperationSwap(int FirstSwapIndex, int SecondSwapIndex)
        {
            //verify the input
            if ((FirstSwapIndex < 0) || (SecondSwapIndex < 0))
                throw new IndexOutOfRangeException("Indexes must be at least 0.");
            if ((FirstSwapIndex >= this.listExposedQubits.Count) || (SecondSwapIndex >= this.listExposedQubits.Count))
                throw new IndexOutOfRangeException("The indexes specified must be less than the maximum index in this register, " + this.listExposedQubits.Count.ToString());
            if (FirstSwapIndex == SecondSwapIndex)
                throw new DuplicateIndexesException("The indexes passed cannot be the same");

            this.ApplyOperation(new OperationSwap(FirstSwapIndex, SecondSwapIndex));

            return this;
        }


        /// <summary>
        /// Perform the swap operation on the qubits specified.
        /// </summary>
        /// <param name="Indexes">Indexes of the qubits to swap.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the parameter is null.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the indexes are
        /// out of range.</exception>
        /// <exception cref="SizeMismatchException">Thrown if 2 indexes are not
        /// specified in the array.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the indexes specified
        /// refer to the same qubit.</exception>
        public IQuantumRegister OperationSwap(int[] Indexes)
        {
            if (Indexes == null)
                throw new ArgumentNullException("Cannot pass a null array of indexes");
            if (Indexes.Length != 2)
                throw new SizeMismatchException("The length of the array Indexes must be 2 for the swap operation.");

            return this.OperationSwap(Indexes[0], Indexes[1]);
        }


        /// <summary>
        /// Perform the T gate (pi/8 phase gate) operation on the qubit.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register does not
        /// contain one qubit.</exception>
        public IQuantumRegister OperationTGate()
        {
            //confirm sizes of opeartion and register match
            if (this.listExposedQubits.Count != 1)
                throw new SizeMismatchException(string.Format("The T gate operation applies to a single qubit. This register exposes {0} qubits.", this.listExposedQubits.Count));

            this.ApplyOperation(new OperationTGate());

            return this;
        }


        /// <summary>
        /// Perform the T gate (pi/8 phase gate) operation on the qubit at the specified
        /// index.
        /// </summary>
        /// <param name="Index">Index of the qubit to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is out of range.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationTGate(int Index)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Perform the T gate (pi/8 phase gate) operation on the qubits at the
        /// specified indexes.
        /// </summary>
        /// <param name="Indexes">Indexes of the qubits to apply the operations to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes are
        /// out of range.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationTGate(int[] Indexes)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Apply the T gate (pi/8 phase gate) operation to all the qubits
        /// in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        public IQuantumRegister OperationTGateAll()
        {
            ComplexMatrix cApplyOperation = (new OperationTGate()).OperationMatrix;
            ComplexMatrix cSingleQubitOp = (new OperationTGate()).OperationMatrix;

            //expand the op to apply to all the exposed qubits. start at 1 since the first op counts
            //as the first qubit
            for (int iExposedSize = 1; iExposedSize < this.listExposedQubits.Count; iExposedSize++)
                cApplyOperation = cApplyOperation.Tensor(cSingleQubitOp);

            this.ApplyOperation(cApplyOperation);

            return this;
        }


        /// <summary>
        /// Apply the Toffoli (controlled controlled not) operation to the register.
        /// The lowest two indexes are the control and the highest is the target.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register does not
        /// contain 3 qubits.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the indexes specified
        /// refer to the same qubit.</exception>
        public IQuantumRegister OperationToffoli()
        {
            //confirm sizes of opeartion and register match
            if (this.listExposedQubits.Count != 3)
                throw new SizeMismatchException(string.Format("The Toffoli operation applies to 3 qubits. This register exposes {0} qubits.", this.listExposedQubits.Count));

            this.ApplyOperation(new OperationToffoli(0, 1, 2));

            return this;
        }


        /// <summary>
        /// Apply the Toffoli (controlled controlled not) operation to the register.
        /// </summary>
        /// <param name="FirstControlIndex">Index of the first control qubit.</param>
        /// <param name="SecondControlIndex">Index of the second control qubit.</param>
        /// <param name="TargetIndex">Index of the target qubit.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes
        /// are out of range.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the indexes specified
        /// refer to the same qubit.</exception>
        public IQuantumRegister OperationToffoli(int FirstControlIndex, int SecondControlIndex, int TargetIndex)
        {
            return this.ApplyOperation(new OperationToffoli(FirstControlIndex, SecondControlIndex, TargetIndex));
        }


        /// <summary>
        /// Perform the Toffoli (controlled controlled not) operation to the register.
        /// </summary>
        /// <param name="Indexes">The indexes of the qubits that the operation applies to. 
        /// The first two indexes are the control and the third is the target.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes are
        /// out of range.</exception>
        /// <exception cref="SizeMismatchException">Thrown if there are not three
        /// elements in the Indexes array.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the indexes specified
        /// refer to the same qubit.</exception>
        /// <exception cref="ArgumentNullException">Thrown if Indexes is null</exception>
        public IQuantumRegister OperationToffoli(int[] Indexes)
        {
            if (Indexes == null)
                throw new ArgumentNullException("Indexes parameter to OperationToffoli cannot be null");

            return this.OperationToffoli(Indexes[0], Indexes[1], Indexes[2]);
        }


        /// <summary>
        /// Perform the Y gate operation to the qubit.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register does
        /// not contain one qubit.</exception>
        public IQuantumRegister OperationYGate()
        {
            //confirm sizes of opeartion and register match
            if (this.listExposedQubits.Count != 1)
                throw new SizeMismatchException(string.Format("The Y gate operation applies to a single qubit. This register exposes {0} qubits.", this.listExposedQubits.Count));

            this.ApplyOperation(new OperationYGate());

            return this;
        }


        /// <summary>
        /// Perform the Y gate operation to the qubit at the specified index.
        /// </summary>
        /// <param name="Index">Index of the qubit to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is out of range.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationYGate(int Index)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Perform the Y gate operation to the qubits at the specified indexes.
        /// </summary>
        /// <param name="Indexes">Indexes to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes
        /// specified are out of range.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationYGate(int[] Indexes)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Perform the Y gate operation to all the qubits in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        public IQuantumRegister OperationYGateAll()
        {
            ComplexMatrix cApplyOperation = (new OperationYGate()).OperationMatrix;
            ComplexMatrix cSingleQubitOp = (new OperationYGate()).OperationMatrix;

            //expand the op to apply to all the exposed qubits. start at 1 since the first op counts
            //as the first qubit
            for (int iExposedSize = 1; iExposedSize < this.listExposedQubits.Count; iExposedSize++)
                cApplyOperation = cApplyOperation.Tensor(cSingleQubitOp);

            this.ApplyOperation(cApplyOperation);

            return this;
        }


        /// <summary>
        /// Perform the Z gate operation to the qubit in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register does not
        /// contain one qubit.</exception>
        public IQuantumRegister OperationZGate()
        {
            //confirm sizes of opeartion and register match
            if (this.listExposedQubits.Count != 1)
                throw new SizeMismatchException(string.Format("The Z gate operation applies to a single qubit. This register exposes {0} qubits.", this.listExposedQubits.Count));

            this.ApplyOperation(new OperationZGate());

            return this;
        }


        /// <summary>
        /// Perform the Z gate operation to the qubit at the specified index.
        /// </summary>
        /// <param name="Index">Index of the qubit to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is out of range.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationZGate(int Index)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Perform the Z gate operation to the qubits at the specified indexes.
        /// </summary>
        /// <param name="Indexes">Indexes of qubits to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes
        /// are out of range.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister OperationZGate(int[] Indexes)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);

            return this;
        }


        /// <summary>
        /// Apply the Z gate operation to all the qubits in a register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        public IQuantumRegister OperationZGateAll()
        {
            ComplexMatrix cApplyOperation = (new OperationZGate()).OperationMatrix;
            ComplexMatrix cSingleQubitOp = (new OperationZGate()).OperationMatrix;

            //expand the op to apply to all the exposed qubits. start at 1 since the first op counts
            //as the first qubit
            for (int iExposedSize = 1; iExposedSize < this.listExposedQubits.Count; iExposedSize++)
                cApplyOperation = cApplyOperation.Tensor(cSingleQubitOp);

            this.ApplyOperation(cApplyOperation);

            return this;
        }

        #endregion


        /// <summary>
        /// Correct an operation matrix so it takes into account the actual number of qubits in
        /// the register.
        /// </summary>
        /// <param name="Operation">The operation that will be applied.</param>
        protected ComplexMatrix CorrectOperationMatrix(ComplexMatrix Operation)
        {
            ComplexMatrix cRetVal = null;
            ComplexMatrix cCurQubitOp = null;

            //TODO: Implement. Don't throw a NotImplementedException so existing prototype
            //      code will work until it is done.

            //take slicing to account, or do it within each manipulation?

            //correct a single qubit operation
            //Per Kaye, if we are applying a single qubit operator x  to the first qubit in an n qubit system
            //we tensor x with I (n - 1) times.
            //P. Kaye, R. Laflamme, and M. Mosca, An Introduction to Quantum Computing. New York City, 
            //New York: Oxford University Press, 2007.
            if ((Operation.GetNumberOfColumns() == 2) && (Operation.GetNumberOfRows() == 2))
            {
                //check should already be done, but just to be safe
                if (this.listExposedQubits.Count != 1)     //a single qubit op can only operate on one qubit
                    throw new SizeMismatchException(string.Format("This register has {0} qubits exposed and the operation operates on {1} qubits. The two must be equal to apply the operation.", this.listExposedQubits.Count, 1));

                //need to go through and correct for all qubits in the register, not just the ones currently exposed.
                for (int iAbsoluteIndex = 0; iAbsoluteIndex < this.iTotalNumberOfQubits; iAbsoluteIndex++)
                {
                    //determine the operation to use for this qubit
                    if (this.listExposedQubits[0] == iAbsoluteIndex)   //at the qubit to apply the operation to
                        cCurQubitOp = Operation.Clone();    //clone so we don't alter it if it used as the return value
                    else
                        cCurQubitOp = (new OperationIdentity()).OperationMatrix;

                    //then tensor it to what has been built up so far (or it is the initial operation)
                    if (cRetVal == null)
                        cRetVal = cCurQubitOp;
                    else
                        cRetVal.Tensor(cCurQubitOp);
                }      //end of iteration through all qubits in the register
            }          //end if a single qubit operation

            //TODO: Take larger operations into account
            else
            {
                cRetVal = Operation;
            }

            return cRetVal;
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


        /// <summary>
        /// Enumerator to return a single qubit register for each qubit in
        /// the register.
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<IQuantumRegister> GetEnumerator()
        {
            foreach (int iCurExposedIndex in this.listExposedQubits)
            {
                yield return this.SliceSubset(new int[] { iCurExposedIndex });
            }
        }


        /// <summary>
        /// The general enumerator.
        /// </summary>
        /// <returns>The enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// Get an array of indexes. This can be used when passing indexes
        /// to methods that apply operations
        /// </summary>
        /// <param name="Beginning">Starting index</param>
        /// <param name="End">End Index</param>
        /// <returns>The indexes requested</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either
        /// parameter is outside the bounds of the register.</exception>
        /// <remarks>Each register has its own indexes, so registers obtained
        /// through slicing will have their own indexes and start at 0.</remarks>
        public int[] GetIndexes(int Beginning, int End)
        {
            if ((Beginning < 0) || (End < 0)
            || (Beginning >= this.listExposedQubits.Count) || (End >= this.listExposedQubits.Count))
                throw new ArgumentOutOfRangeException(string.Format("Parameters are outside of the bounds of the indexes. Allowable range is 0 - {0}, but {1} and {2} were passed in.", this.listExposedQubits.Count, Beginning, End));

            return (this.listExposedQubits.GetRange(Beginning, (End - Beginning))).ToArray();
        }


        /// <summary>
        /// Get an array of indexes. This can be used when passing indexes
        /// to methods that apply operations
        /// </summary>
        /// <returns>The indexes requested</returns>
        /// <remarks>Each register has its own indexes, so registers obtained
        /// through slicing will have their own indexes and start at 0.</remarks>
        public int[] GetIndexes()
        {
            return (this.listExposedQubits.GetRange(0, this.listExposedQubits.Count)).ToArray();
        }

    }
}
