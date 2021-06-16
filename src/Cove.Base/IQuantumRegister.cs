using System;
using System.Collections.Generic;

namespace CoveX.Base
{
    /// <summary>
    /// This interface defines the behavior for quantum registers, which are collections
    /// of qubits. Direct implementations of this interface should be named QuantumRegister.
    /// </summary>
    public interface IQuantumRegister : ICoveObject, IEnumerable<IQuantumRegister>
    {
        #region "Measurement, initialization, and manipulation methods"

        /// <summary>
        /// Returns the location of the quantum resource.
        /// </summary>
        /// <returns>
        /// The location of the quantum resource.
        /// </returns>
        string GetLocation();

        /// <summary>
        /// Set the location of the quantum resource. Note that this resets
        /// the state of all of the qubits.
        /// </summary>
        /// <param name="Location">The location of the quantum resource</param>
        /// <remarks>This sets the locations of all the qubits used by this resource.</remarks>
        void SetLocation(string Location);

        /// <summary>
        /// Perform a measurement on the quantum register, collapsing to an absolute state.
        /// </summary>
        /// <returns>An array of booleans (bits), which are the qubits collapsed to
        /// false (0) or true (1).</returns>
        ClassicalResult Measure();


        /// <summary>
        /// Measure the quantum register and put the results in ReturnValue. A user
        /// can subclass ClassicalResult with their own class and pass that in if
        /// needed.
        /// </summary>
        /// <param name="ReturnValue">The object which will contain the
        /// result of the measurement.</param>
        void Measure(ClassicalResult ReturnValue);


        /// <summary>
        /// Measure a set of qubits in the register.
        /// </summary>
        /// <param name="Indexes">Indexes of the qubits to measure</param>
        /// <returns>An array of booleans (bits), which are the qubits collapsed to
        /// false (0) or true (1).</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the
        /// indexes are invalid.</exception>
        ClassicalResult Measure(IEnumerable<int> Indexes);


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
        void Measure(IEnumerable<int> Indexes, ClassicalResult ReturnValue);


        /// <summary>
        /// Measure one qubit in the register.
        /// </summary>
        /// <param name="Index">Index of the qubit to measure.</param>
        /// <returns>The classical information extracted from the qubit, false (0) or
        /// true (1).</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is invalid.</exception>
        ClassicalResult Measure(int Index);


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
        void Measure(int Index, ClassicalResult ReturnValue);


        /// <summary>
        /// Add another register to the beginning of this one.
        /// </summary>
        /// <param name="RegisterToAppend">Register to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined register.</returns>
        IQuantumRegister InsertQubitsAtBeginning(IQuantumRegister RegisterToAppend);


        /// <summary>
        /// Append a set of registers to the beginning of this register, in the same order as
        /// the array.
        /// </summary>
        /// <param name="RegistersToAppend">Registers to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined register.</returns>
        IQuantumRegister InsertQubitsAtBeginning(IQuantumRegister[] RegistersToAppend);


        /// <summary>
        /// Add a single qubit to the beginning of the register.
        /// </summary>
        /// <param name="QubitToAppend">The qubit to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns></returns>
        IQuantumRegister InsertQubitsAtBeginning(IQubit QubitToAppend);


        /// <summary>
        /// Add a series of qubits to the beginning of the register, in the same order as specified
        /// in the array.
        /// </summary>
        /// <param name="QubitsToAppend">The qubits to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined register.</returns>
        IQuantumRegister InsertQubitsAtBeginning(IQubit[] QubitsToAppend);


        /// <summary>
        /// Insert a number of qubits at the beginning of the register. All other
        /// qubits are shifted to higher indexes.
        /// </summary>
        /// <param name="NumberOfQubitsToAdd">The number of qubits to add to the
        /// register.</param>
        /// <returns>A reference to the expanded register.</returns>
        IQuantumRegister InsertQubitsAtBeginning(int NumberOfQubitsToAdd);


        /// <summary>
        /// Insert a number of qubits at the beginning of the register. All other
        /// qubits are shifted to higher indexes.
        /// </summary>
        /// <param name="NumberOfQubitsToAdd">The number of qubits to add to the
        /// register.</param>
        /// <param name="InitializeTo">Value to initialize the new qubits to.</param>
        /// <returns>A reference to the expanded register.</returns>
        IQuantumRegister InsertQubitsAtBeginning(int NumberOfQubitsToAdd, bool InitializeTo);


        /// <summary>
        /// Insert a quantum register into this one.
        /// </summary>
        /// <param name="AtIndex">The register will be inserted starting at this index. The qubits
        /// at this index and after at the existing one will be shifted.</param>
        /// <param name="RegisterToInsert">The register to insert into this one. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined regiser.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is out of range.</exception>
        IQuantumRegister InsertQubits(int AtIndex, IQuantumRegister RegisterToInsert);


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
        IQuantumRegister InsertQubits(int AtIndex, IQuantumRegister[] RegistersToInsert);


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
        IQuantumRegister InsertQubits(int AtIndex, IQubit QubitToInsert);


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
        IQuantumRegister InsertQubits(int AtIndex, IQubit[] QubitsToInsert);


        /// <summary>
        /// Insert qubits into an arbitrary location in the register.
        /// </summary>
        /// <param name="AtIndex">The qubits will be inserted starting at this index. 
        /// The qubits at this index and after at the existing register will be shifted.</param>
        /// <param name="NumberOfQubitsToAdd">Number of qubits to insert.</param>
        /// <returns>A reference to the combined register.</returns>
        IQuantumRegister InsertQubits(int AtIndex, int NumberOfQubitsToAdd);


        /// <summary>
        /// Insert qubits into an arbitrary location in the register.
        /// </summary>
        /// <param name="AtIndex">The qubits will be inserted starting at this index. 
        /// The qubits at this index and after at the existing register will be shifted.</param>
        /// <param name="NumberOfQubitsToAdd">Number of qubits to insert.</param>
        /// <param name="InitializeTo">Value to initialize the new qubits to.</param>
        /// <returns>A reference to the combined register.</returns>
        IQuantumRegister InsertQubits(int AtIndex, int NumberOfQubitsToAdd, bool InitializeTo);


        /// <summary>
        /// Append another register to this one.
        /// </summary>
        /// <param name="RegisterToAppend">Register to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined register.</returns>
        IQuantumRegister InsertQubitsAtEnd(IQuantumRegister RegisterToAppend);


        /// <summary>
        /// Append a set of registers to this register, in the same order as
        /// the array.
        /// </summary>
        /// <param name="RegistersToAppend">Registers to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the combined register.</returns>
        IQuantumRegister InsertQubitsAtEnd(IQuantumRegister[] RegistersToAppend);


        /// <summary>
        /// Append a single qubit to the register.
        /// </summary>
        /// <param name="QubitToAppend">The qubit to append. Passing null is valid and has no 
        /// effect on the register.</param>
        /// <returns>A reference to the expanded register</returns>
        IQuantumRegister InsertQubitsAtEnd(IQubit QubitToAppend);


        /// <summary>
        /// Append a specific number of qubits to the register.
        /// </summary>
        /// <param name="NumberOfQubitsToAdd">The number of qubits to add to the register.</param>
        /// <returns>A reference to the expanded register.</returns>
        IQuantumRegister InsertQubitsAtEnd(int NumberOfQubitsToAdd);


        /// <summary>
        /// Append a specific number of qubits to the register, initialized to the specified
        /// value.
        /// </summary>
        /// <param name="NumberOfQubitsToAdd">The number of qubits to add to to the 
        /// register.</param>
        /// <param name="InitializeTo">The value to initialze the qubits to.</param>
        /// <returns>A reference to the expanded register.</returns>
        IQuantumRegister InsertQubitsAtEnd(int NumberOfQubitsToAdd, bool InitializeTo);


        /// <summary>
        /// It is possible to create a register with references to the same qubit. This
        /// goes through and eliminates duplicate references, leaving the reference at
        /// the lowest index.
        /// </summary>
        /// <returns>A reference to the register with the duplicates eliminated.</returns>
        /// <example>If the register contains qubits ABACDAC then a register containing
        /// only ABCD is returned.</example>
        IQuantumRegister EliminateDuplicateReferences();


        /// <summary>
        /// Slices can be used to obtain subsets of a register. This method returns a register
        /// that contains the whole set of qubits that have been included in this register. This
        /// register will contain the qubits in their original order, regardless of slices. 
        /// </summary>
        /// <returns>The register that contains the whole set of qubits that have ever
        /// been part of this register.</returns>
        IQuantumRegister GetCompleteRegister();


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
        IQuantumRegister SetQubit(int Index, bool SetTo);


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
        IQuantumRegister SetQubits(int StartingIndex, ClassicalResult SetTo);


        /// <summary>
        /// Set the qubits starting at the first index to the classical result SetTo.
        /// </summary>
        /// <param name="SetTo">The classical result which will be read and the applied
        /// to the qubits starting at StartingIndex and in the same order.</param>
        /// <returns>A reference to the modified register.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the length of SetTo
        /// exceeds the length of the register. Example: If the
        /// register consists of 5 qubits and the length of SetTo is
        /// 20 qubits then this exception will be thrown.</exception>
        IQuantumRegister SetQubits(ClassicalResult SetTo);


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
        IQuantumRegister SetQubits(int[] Indexes, bool SetTo);


        /// <summary>
        /// Set qubits based on the boolean values given.
        /// </summary>
        /// <param name="QubitValues">Values to set each qubit to. Qubit at position x will be set
        /// to the boolean element at position x in QubitValues.</param>
        /// <returns>A reference to the modified register.</returns>
        /// <exception cref="ArgumentNullException">Thrown if QubitValues is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the length of QubitValues
        /// does not match the length of the exposed qubits in the register.</exception>
        IQuantumRegister SetQubits(bool[] QubitValues);


        /// <summary>
        /// Set all the qubits of the register to the specified value.
        /// </summary>
        /// <param name="SetTo">Value to set the qubits to, true (1) or false (0).</param>
        /// <returns>A reference to the modified register.</returns>
        /// <remarks>A qubit is measured before it is set, so any other qubits entangled
        /// with this one will be effected.</remarks>
        IQuantumRegister SetAllQubitsTo(bool SetTo);

        #endregion             //end of measurement and initialization methods.

        #region "Utility methods"

        /// <summary>
        /// Get the length of the quantum register.
        /// </summary>
        /// <returns>The number of qubits in the register</returns>
        int GetLength();

        /// <summary>
        /// Slicing may result in a subset of the register being exposed. This gets the
        /// total length of the register, which includes qubits that may be hidden
        /// through slicing.
        /// </summary>
        /// <returns>The total number of qubits in the register, some of which
        /// may be hidden.</returns>
        int GetTotalLength();


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
        int[] GetIndexes(int Beginning, int End);


        /// <summary>
        /// Get an array of indexes. This can be used when passing indexes
        /// to methods that apply operations
        /// </summary>
        /// <returns>The indexes requested</returns>
        /// <remarks>Each register has its own indexes, so registers obtained
        /// through slicing will have their own indexes and start at 0.</remarks>
        int[] GetIndexes();

        #endregion     //end of utility methods

        #region "Slice operations"

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
        IQuantumRegister Slice(int StartIndex, int StopIndex);


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
        IQuantumRegister SliceReverse(int StartIndex, int StopIndex);


        /// <summary>
        /// Returns a quantum register with the qubits reversed.
        /// </summary>
        /// <returns>The quantum register representing this register with the qubits reversed.</returns>
        IQuantumRegister SliceReverse();


        /// <summary>
        /// Returns a slice (subset) of the register from StartIndex to the end.
        /// </summary>
        /// <param name="StartIndex">Starting index in the register to get the slice of.</param>
        /// <returns>The quantum register representing the slice.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the start index is out of range.</exception>
        IQuantumRegister SliceFrom(int StartIndex);


        /// <summary>
        /// Returns a slice (subset) of the register from the beginning to EndIndex.
        /// </summary>
        /// <param name="EndIndex">The ending index of the slice.</param>
        /// <returns>The quantum register representing the slice.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the end index is out of range.</exception>
        IQuantumRegister SliceTo(int EndIndex);


        /// <summary>
        /// Returns a slice (subset) of the quantum register containing the qubits
        /// specified in Indexes, and in that order.
        /// </summary>
        /// <param name="Indexes">The indexes of the qubits in the register being
        /// returned.</param>
        /// <returns>The quantum register representing the subset.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes specified are
        /// outside of the allowable range.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the same index is specified more than
        /// once.</exception>
        IQuantumRegister SliceSubset(IEnumerable<int> Indexes);


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
        IQuantumRegister Slice(int StartIndex, int StopIndex, IQuantumOperation Operation);


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
        IQuantumRegister SliceReverse(int StartIndex, int StopIndex, IQuantumOperation Operation);


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
        IQuantumRegister SliceReverse(IQuantumOperation Operation);


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
        IQuantumRegister SliceFrom(int StartIndex, IQuantumOperation Operation);


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
        IQuantumRegister SliceTo(int EndIndex, IQuantumOperation Operation);


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
        IQuantumRegister SliceSubset(IEnumerable<int> Indexes, IQuantumOperation Operation);


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
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        IQuantumRegister Slice(int StartIndex, int StopIndex, IEnumerable<IQuantumOperation> Operations);


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
        IQuantumRegister SliceReverse(int StartIndex, int StopIndex, IEnumerable<IQuantumOperation> Operations);


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
        IQuantumRegister SliceReverse(IEnumerable<IQuantumOperation> Operations);


        /// <summary>
        /// Returns a slice (subset) of the register from StartIndex to the end, then has
        /// the operations applied to it.
        /// </summary>
        /// <param name="StartIndex">Starting index in the register to get the slice of.</param>
        /// <param name="Operations">The operations to apply to the slice.</param>
        /// <returns>The quantum register representing the slice with the operation then applied.</returns>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        IQuantumRegister SliceFrom(int StartIndex, IEnumerable<IQuantumOperation> Operations);


        /// <summary>
        /// Returns a slice (subset) of the quantum register containing the qubits
        /// specified in Indexes, and in that order. The specified operations are
        /// then applied.
        /// </summary>
        /// <param name="Indexes">The indexes of the qubits in the register being
        /// returned.</param>
        /// <param name="Operations">The operations to apply to the slice.</param>
        /// <returns>The quantum register representing the subset.</returns>
        /// <exception cref="NotUnitaryOperationException">Thrown if a non-unitary operation is
        /// passed. All quantum operations must be unitary.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the size of one of the operations
        /// does not match the size of the register.</exception>
        IQuantumRegister SliceSubset(IEnumerable<int> Indexes, IEnumerable<IQuantumOperation> Operations);


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
        IQuantumRegister SliceReorder(IEnumerable<int> NewIndexes);



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
        IQuantumRegister SliceTo(int EndIndex, List<IQuantumOperation> Operations);


        #endregion        //end of slice operations region

        #region "Applying quantum operations to registers"

        /// <summary>
        /// Apply a generic operation to the register.
        /// </summary>
        /// <param name="Operation">The operation to apply</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the number of qubits
        /// the operation applies to does not match the number of qubits in the
        /// register.</exception>
        IQuantumRegister ApplyOperation(IQuantumOperation Operation);


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
        IQuantumRegister ApplyOperation(IQuantumOperation Operation, int[] Indexes);


        /// <summary>
        /// Apply the single qubit operation to all qubits in the register.
        /// </summary>
        /// <param name="Operation">Operation to apply to all the qubits.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        IQuantumRegister ApplyOperationAll(IQubitOperation Operation);


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
        IQuantumRegister ApplyOperations(IEnumerable<IQuantumOperation> Operations);


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
        IQuantumRegister OperationControlledU(IQubitOperation TargetOperation, int ControlIndex);


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
        IQuantumRegister OperationControlledU(IQubitOperation TargetOperation, int ControlIndex, int[] TargetIndexes);


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
        IQuantumRegister OperationCNot();


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
        IQuantumRegister OperationCNot(int ControlIndex, int TargetIndex);


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
        IQuantumRegister OperationCNot(int[] Indexes);


        /// <summary>
        /// Perform the Fredkin operation (controlled swap). 
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        IQuantumRegister OperationFredkin();


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
        IQuantumRegister OperationFredkin(int ControlIndex, int XIndex, int YIndex);


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
        IQuantumRegister OperationFredkin(int[] Indexes);


        /// <summary>
        /// Perform the Hadamard (square root of Not) operation on a qubit.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register contains
        /// more than one qubit.</exception>
        IQuantumRegister OperationHadamard();


        /// <summary>
        /// Apply the Hadamard (square root of Not) operation to the specified qubit.
        /// </summary>
        /// <param name="Index">Index to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index does not exist in
        /// the register.</exception>
        IQuantumRegister OperationHadamard(int Index);


        /// <summary>
        /// Perform the Hadamard (square root of Not) operation to the specified qubits.
        /// </summary>
        /// <param name="Indexes">Indexes of qubits to apply the operations to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the indexes do not exist
        /// in the register.</exception>
        IQuantumRegister OperationHadamard(int[] Indexes);


        /// <summary>
        /// Apply the Hadamard (square root of Not) operation to all the qubits in the
        /// register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        IQuantumRegister OperationHadamardAll();


        /// <summary>
        /// Apply the identity operation to a qubit.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register contains
        /// more than one qubit.</exception>
        IQuantumRegister OperationIdentity();


        /// <summary>
        /// Apply the identity operation to the qubit at the specified index.
        /// </summary>
        /// <param name="Index">Index of the qubit to apply the operation to.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index of the qubit
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// is out of range in the register.</exception>
        IQuantumRegister OperationIdentity(int Index);


        /// <summary>
        /// Apply the identity operation to the qubits at the specified indexes.
        /// </summary>
        /// <param name="Indexes">Indexes to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes
        /// specified are out of range.</exception>
        IQuantumRegister OperationIdentity(int[] Indexes);


        /// <summary>
        /// Apply the identity operation to all the qubits in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        IQuantumRegister OperationIdentityAll();


        /// <summary>
        /// Apply the Not (X gate) operation to the qubit.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register contains
        /// more than one qubit.</exception>
        IQuantumRegister OperationNot();


        /// <summary>
        /// Apply the Not (X gate) operation to the qubit at the specified index.
        /// </summary>
        /// <param name="Index">Index of the qubit to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown in the index specified
        /// is out of range.</exception>
        IQuantumRegister OperationNot(int Index);


        /// <summary>
        /// Apply the Not (X gate) operation to the qubits at the specified indexes.
        /// </summary>
        /// <param name="Indexes">Indexes of qubits to apply the operations to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes are
        /// out of range.</exception>
        IQuantumRegister OperationNot(int[] Indexes);


        /// <summary>
        /// Apply the Not (X gate) operation to all the qubits in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        IQuantumRegister OperationNotAll();


        /// <summary>
        /// Apply the Quantum Fourier Transformation (QFT) to the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        IQuantumRegister OperationQuantumFourierTransform();


        /// <summary>
        /// Apply the inverse Quantum Fourier Transform (QFT) to the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        IQuantumRegister OperationInverseQuantumFourierTransform();


        /// <summary>
        /// Apply the S gate (phase gate) to the qubit.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register contains
        /// more than one qubit.</exception>
        IQuantumRegister OperationSGate();


        /// <summary>
        /// Apply the S gate (phase gate) to the qubit at the specified index.
        /// </summary>
        /// <param name="Index">Index of the qubit to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is out of range.</exception>
        IQuantumRegister OperationSGate(int Index);


        /// <summary>
        /// Apply the S gate (phase gate) to the qubits at the indexes specified.
        /// </summary>
        /// <param name="Indexes">Indexes of the qubits to apply the indexes to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes
        /// specified are out of range.</exception>
        IQuantumRegister OperationSGate(int[] Indexes);


        /// <summary>
        /// Apply the S gate (phase gate) operation to all the qubits in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        IQuantumRegister OperationSGateAll();


        /// <summary>
        /// Apply the swap operation to the qubits in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register does not
        /// contain two qubits.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the indexes specified
        /// refer to the same qubit.</exception>
        IQuantumRegister OperationSwap();


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
        IQuantumRegister OperationSwap(int FirstSwapIndex, int SecondSwapIndex);


        /// <summary>
        /// Perform the swap operation on the qubits specified.
        /// </summary>
        /// <param name="Indexes">Indexes of the qubits to swap.</param>
        /// <exception cref="ArgumentNullException">Thrown if the parameter is null.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the indexes are
        /// out of range.</exception>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if 2 indexes are not
        /// specified in the array.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the indexes specified
        /// refer to the same qubit.</exception>
        IQuantumRegister OperationSwap(int[] Indexes);


        /// <summary>
        /// Perform the T gate (pi/8 phase gate) operation on the qubit.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register does not
        /// contain one qubit.</exception>
        IQuantumRegister OperationTGate();


        /// <summary>
        /// Perform the T gate (pi/8 phase gate) operation on the qubit at the specified
        /// index.
        /// </summary>
        /// <param name="Index">Index of the qubit to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is out of range.</exception>
        IQuantumRegister OperationTGate(int Index);


        /// <summary>
        /// Perform the T gate (pi/8 phase gate) operation on the qubits at the
        /// specified indexes.
        /// </summary>
        /// <param name="Indexes">Indexes of the qubits to apply the operations to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes are
        /// out of range.</exception>
        IQuantumRegister OperationTGate(int[] Indexes);


        /// <summary>
        /// Apply the T gate (pi/8 phase gate) operation to all the qubits
        /// in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        IQuantumRegister OperationTGateAll();


        /// <summary>
        /// Apply the Toffoli (controlled controlled not) operation to the register.
        /// The lowest two indexes are the control and the highest is the target.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register does not
        /// contain 3 qubits.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the indexes specified
        /// refer to the same qubit.</exception>
        IQuantumRegister OperationToffoli();


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
        IQuantumRegister OperationToffoli(int FirstControlIndex, int SecondControlIndex, int TargetIndex);


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
        IQuantumRegister OperationToffoli(int[] Indexes);


        /// <summary>
        /// Perform the Y gate operation to the qubit.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register does
        /// not contain one qubit.</exception>
        IQuantumRegister OperationYGate();


        /// <summary>
        /// Perform the Y gate operation to the qubit at the specified index.
        /// </summary>
        /// <param name="Index">Index of the qubit to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is out of range.</exception>
        IQuantumRegister OperationYGate(int Index);


        /// <summary>
        /// Perform the Y gate operation to the qubits at the specified indexes.
        /// </summary>
        /// <param name="Indexes">Indexes to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes
        /// specified are out of range.</exception>
        IQuantumRegister OperationYGate(int[] Indexes);


        /// <summary>
        /// Perform the Y gate operation to all the qubits in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        IQuantumRegister OperationYGateAll();


        /// <summary>
        /// Perform the Z gate operation to the qubit in the register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="SizeMismatchException">Thrown if the register does not
        /// contain one qubit.</exception>
        IQuantumRegister OperationZGate();


        /// <summary>
        /// Perform the Z gate operation to the qubit at the specified index.
        /// </summary>
        /// <param name="Index">Index of the qubit to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index specified
        /// is out of range.</exception>
        IQuantumRegister OperationZGate(int Index);


        /// <summary>
        /// Perform the Z gate operation to the qubits at the specified indexes.
        /// </summary>
        /// <param name="Indexes">Indexes of qubits to apply the operation to.</param>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if any of the indexes
        /// are out of range.</exception>
        IQuantumRegister OperationZGate(int[] Indexes);


        /// <summary>
        /// Apply the Z gate operation to all the qubits in a register.
        /// </summary>
        /// <returns>A reference to this register after the operation has been applied.</returns>
        IQuantumRegister OperationZGateAll();


        /// <summary>
        /// Change the label of 0
        /// </summary>
        /// <param name="LabelZero">The new label of zero</param>
        void SetLabelZero(string LabelZero);


        /// <summary>
        /// Change the label of 1
        /// </summary>
        /// <param name="LabelOne">The new label of one</param>
        void SetLabelOne(string LabelOne);


        /// <summary>
        /// Set both the label for |0> and label for |1> in one call
        /// </summary>
        /// <param name="LabelZero">The label of zero</param>
        /// <param name="LabelOne">The label of one</param>
        void SetLabels(string LabelZero, string LabelOne);

        #endregion        //end of applying quantum operations to registers
    }
}
