
using System;
using System.Collections.Generic;

namespace CoveX.Base
{
    /// <summary>
    /// This interface provides common quantum algorithms that should
    /// be implemented. These common algorithms let users utilize the
    /// framework for specific quantum tasks without being concerned
    /// with the details of how the algorithms work. Direct implementations of 
    /// this interface should be named QuantumAlgorithms.
    /// </summary>
    public interface IQuantumAlgorithms : ICoveObject
    {
        /// <summary>
        /// Factor a number using Shor's algorithm.
        /// </summary>
        /// <param name="NumberToFactor">The number to factor</param>
        /// <param name="Factor1">The first factor of the number</param>
        /// <param name="Factor2">The second factor of the number</param>
        /// <returns>False if the number could not be factored. In this 
        /// case the two output variables will be 0.</returns>
        bool Factor(int NumberToFactor, out int Factor1, out int Factor2);

        #region "Algorithms needed to perform factoring"

        /// <summary>
        /// Return the operations that perform Sum.
        /// </summary>
        /// <param name="CarryIndex">The index of the carry qubit. 
        /// This remains unchanged
        /// after the operations are applied.</param>
        /// <param name="XIndex">The index of the X qubit. 
        /// This remains unchanged
        /// after the operations are applied.</param>
        /// <param name="YIndex">The index of the Y qubit. 
        /// CarryIndex + XIndex + YIndex 
        /// (mod 2 addition)</param>
        /// <returns>The quantum operations that perform 
        /// sum over the specified qubits.</returns>
        /// <exception cref="DuplicateIndexesException">Thrown
        /// if any of the indexes passed in are the same. All 
        /// indexes must be unique.</exception>
        List<IQuantumOperation> Sum(int CarryIndex, int XIndex,
        int YIndex);


        /// <summary>
        /// Perform Uf for factoring
        /// </summary>
        /// <param name="Register1Indexes">First register</param>
        /// <param name="Register2Indexes">Second register</param>
        /// <param name="N">Number being factored.</param>
        /// <returns>The operations to apply Uf</returns>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the 
        /// indexes specified are duplicates.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the parameters
        /// are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the indexes
        /// specified are invalid.</exception>
        List<IQuantumOperation> FactoringUf(int[] Register1Indexes, int[] Register2Indexes,
        int N);


        /// <summary>
        /// Get the operations to apply the Quantum Fourier
        /// Transform.
        /// </summary>
        /// <param name="Indexes">Indexes to target.</param>
        /// <returns>Operations to perform the QFT.</returns>
        /// <exception cref="DuplicateIndexesException">Thrown if 
        /// any of the indexes specified are duplicaes.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if
        /// any of the indexes are less than 0.</exception>
        List<IQuantumOperation> QuantumFourierTransform(int[] Indexes);


        /// <summary>
        /// Get the operations to apply the inverse Quantum Fourier
        /// Transform.
        /// </summary>
        /// <param name="Indexes">Indexes to target.</param>
        /// <returns>Operations to perform the QFT.</returns>
        /// <exception cref="DuplicateIndexesException">Thrown if 
        /// any of the indexes specified are duplicaes.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if
        /// any of the indexes are less than 0.</exception>
        List<IQuantumOperation> QuantumFourierTransformInverse(int[] Indexes);


        /// <summary>
        /// Return the operations to perform the carry gate.
        /// </summary>
        /// <param name="CarryIndex">The index of the carry qubit. 
        /// Remains unchanged after
        /// the operations are applied.</param>
        /// <param name="XIndex">The index of the X qubit. 
        /// Remains unchanged after
        /// the operations are applied.</param>
        /// <param name="YIndex">The index of the Y qubit. On 
        /// output this will be a + b (mod 2
        /// addition)</param>
        /// <param name="AncilliaIndex">The index of the ancillia 
        /// (scratch) qubit. On output this will be 
        /// (CarryIndex)(XIndex) + (XIndex)(CarryIndex)
        /// + (YIndex)(CarryIndex)
        /// (mod 2 addition)</param>
        /// <returns>The operations to 
        /// perform carry.</returns>
        /// <exception cref="DuplicateIndexesException">
        /// Thrown if any of the indexes are 
        /// duplicates.</exception>
        List<IQuantumOperation> Carry(int CarryIndex, int XIndex,
        int YIndex, int AncilliaIndex);


        /// <summary>
        /// Return the operations to perform the inverse carry.
        /// </summary>
        /// <param name="CarryIndex">The index of the carry qubit. Remains unchanged once
        /// the operations are applied.</param>
        /// <param name="XIndex">The index of the X qubit. Remains unchanged once the 
        /// operations are applied.</param>
        /// <param name="YIndex">The index of the Y qubit. After the operations are
        /// applied this will be X + Y (mod 2 addition)</param>
        /// <param name="CarryPrimeIndex">The index of the carry prime qubit. Will be 
        /// x(x + y) + yc + c' on output.</param>
        /// <returns>The operations to apply the carry inverse.</returns>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes
        /// specified are duplicates.</exception>
        List<IQuantumOperation> CarryInverse(int CarryIndex, int XIndex, int YIndex, int CarryPrimeIndex);


        /// <summary>
        /// Return the operations needed to perform Add over two registers of equal size.
        /// </summary>
        /// <param name="XIndexes">The indexes of the X register. These remain unchanged
        /// once the operations are applied.</param>
        /// <param name="YIndexes">The indexes of the Y register. These contain the
        /// result after the operations are applied, along with the last ancillia qubit</param>
        /// <param name="AncilliaIndexes">The indexes of the ancillia qubits, which should
        /// be initialized to |0>. There should be one more ancillia qubit than there are
        /// X or Y qubits. The result will be in the YIndexes and the last ancillia index.</param>
        /// <returns>The operations to apply add n.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the indexes passed
        /// in are null.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes specified
        /// are duplicates. All indexes must be unique.</exception>
        /// <exception cref="SizeMismatchException">Thrown if XIndexes and YIndexes are not of
        /// equal length, or if AncilliaIndexes is not 1 larger than XIndexes and 
        /// YIndexes.</exception>
        List<IQuantumOperation> AddN(int[] XIndexes, int[] YIndexes, int[] AncilliaIndexes);


        /// <summary>
        /// Return the operations needed to perform Add over two registers of equal size.
        /// </summary>
        /// <param name="XIndexes">The indexes of the X register. These remain unchanged
        /// once the operations are applied.</param>
        /// <param name="YIndexes">The indexes of the Y register. These contain the
        /// result after the operations are applied, along with the last ancillia qubit</param>
        /// <param name="AncilliaIndexes">The indexes of the ancillia qubits, which should
        /// be initialized to |0>. There should be one more ancillia qubit than there are
        /// X or Y qubits. The result will be in the YIndexes and the last ancillia index.</param>
        /// <param name="ResultIndexes">The YIndexes and last ancillia index contain the result,
        /// but this parameter will be populated with them explicitly for ease of use.</param>
        /// <returns>The operations to apply add n.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the indexes passed
        /// in are null.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes specified
        /// are duplicates. All indexes must be unique.</exception>
        /// <exception cref="SizeMismatchException">Thrown if XIndexes and YIndexes are not of
        /// equal length, or if AncilliaIndexes is not 1 larger than XIndexes and 
        /// YIndexes.</exception>
        List<IQuantumOperation> AddN(int[] XIndexes, int[] YIndexes, int[] AncilliaIndexes,
        out int[] ResultIndexes);


        /// <summary>
        /// Return the operations needed to perform Add Inverse over two registers of equal size.
        /// Add inverse is subtraction.
        /// </summary>
        /// <param name="XIndexes">The indexes of the X register. These remain unchanged
        /// once the operations are applied.</param>
        /// <param name="YIndexes">The indexes of the Y register. These contain the
        /// result after the operations are applied, along with the last ancillia qubit</param>
        /// <param name="AncilliaIndexes">The indexes of the ancillia qubits, which should
        /// be initialized to |0>. There should be one more ancillia qubit than there are
        /// X or Y qubits. The result will be in the YIndexes and the last ancillia index.</param>
        /// <returns>The operations to apply add n.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the indexes passed
        /// in are null.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes specified
        /// are duplicates. All indexes must be unique.</exception>
        /// <exception cref="SizeMismatchException">Thrown if XIndexes and YIndexes are not of
        /// equal length, or if AncilliaIndexes is not 1 larger than XIndexes and 
        /// YIndexes.</exception>
        List<IQuantumOperation> AddNInverse(int[] XIndexes, int[] YIndexes, int[] AncilliaIndexes);


        /// <summary>
        /// Return the operations needed to perform Add Inverse over two registers of equal size.
        /// Add inverse is subtraction.
        /// </summary>
        /// <param name="XIndexes">The indexes of the X register. These remain unchanged
        /// once the operations are applied.</param>
        /// <param name="YIndexes">The indexes of the Y register. These contain the
        /// result after the operations are applied, along with the last ancillia qubit</param>
        /// <param name="AncilliaIndexes">The indexes of the ancillia qubits, which should
        /// be initialized to |0>. There should be one more ancillia qubit than there are
        /// X or Y qubits. The result will be in the YIndexes and the last ancillia index.</param>
        /// <param name="ResultIndexes">The YIndexes index contain the result,
        /// but this parameter will be populated with them explicitly for ease of use.</param>
        /// <param name="CarryIndex">The carry index, the element at that location will be 1 if (Y - X) 
        /// less than 0, else the value at that location will be 0.</param>
        /// <returns>The operations to apply add n.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the indexes passed
        /// in are null.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes specified
        /// are duplicates. All indexes must be unique.</exception>
        /// <exception cref="SizeMismatchException">Thrown if XIndexes and YIndexes are not of
        /// equal length, or if AncilliaIndexes is not 1 larger than XIndexes and 
        /// YIndexes.</exception>
        List<IQuantumOperation> AddNInverse(int[] XIndexes, int[] YIndexes, int[] AncilliaIndexes,
        out int[] ResultIndexes, out int CarryIndex);


        /// <summary>
        /// Swap the first indexes and the second indexes. Each qubit in element x of 
        /// FirstSwapIndexes will be swapped with element x of SecondSwapIndexes. Example:
        /// A register is ordered 0, 1, 2, 3. If FirstIndexes = 0, 2 and SecondIndexes = 3, 1
        /// then the resulting order will be 3, 1, 2, 0.
        /// </summary>
        /// <param name="FirstSwapIndexes">First set of indexes to swap.</param>
        /// <param name="SecondSwapIndexes">Second set of indexes to swap.</param>
        /// <returns>The operations that perform this swap.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the arrays passed in are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the indexes passed in are not
        /// of equal length.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes
        /// are duplicated. All indexes in and between the two parameters must be unique.</exception>
        List<IQuantumOperation> SwapIndexes(int[] FirstSwapIndexes, int[] SecondSwapIndexes);


        /// <summary>
        /// Returns the operations to perform the negated CNot. The normal CNot operation flips
        /// the target qubit when the control is 1, otherwise no change is made. This (the negated)
        /// flips the target when the control is 0 instead of 1.
        /// </summary>
        /// <param name="ControlIndex">Index of the control qubit.</param>
        /// <param name="TargetIndex">Index of the target qubit.</param>
        /// <returns>The operations to performt he negated CNot.</returns>
        /// <exception cref="DuplicateIndexesException">Thrown if the control
        /// and target indexes are the same.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either parameter
        /// is less than 0.</exception>
        List<IQuantumOperation> NegatedCNot(int ControlIndex, int TargetIndex);


        /// <summary>
        /// The controlled reset operation. If the control qubit is 1 then all the
        /// target indexes are reset to 0.
        /// </summary>
        /// <param name="ControlIndex">The index of the control qubit.</param>
        /// <param name="TargetIndexes">The target indexes to reset if the control is
        /// set.</param>
        /// <param name="ClassicalValue">The classical value to reset.</param>
        /// <returns>Operations required to perform this.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the parameters
        /// are null.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes
        /// specified are not unique.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any of the indexes
        /// specified are less than 0, or if TargetIndexes is an empty array.</exception>
        List<IQuantumOperation> ControlledReset(int ControlIndex, int[] TargetIndexes,
        ClassicalResult ClassicalValue);


        /// <summary>
        /// Performs modular addition of X and Y: (x + y) mod n. All index arrays must be of 
        /// equal length.
        /// </summary>
        /// <param name="XIndexes">The indexes for x.</param>
        /// <param name="YIndexes">The indexes for y.</param>
        /// <param name="NIndexes">The indexes for N.</param>
        /// <param name="AncilliaIndexes">The index of an ancillia qubits, must be set to 0. This 
        /// array has to be 2 qubits greater than the others.</param>
        /// <returns>The operations to perform modular addition.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the arrays passed in are
        /// null.</exception>
        /// <exception cref="ArgumentException">Thrown if the arrays passed in are not
        /// of equal length or less than zero..</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any indexes are specified
        /// more than once.</exception>
        List<IQuantumOperation> ModularNAdder(int[] XIndexes, int[] YIndexes, int[] NIndexes,
        int[] AncilliaIndexes);

        #endregion "Algorithms needed to perform factoring"

        #region "Get common quantum registers"

        /// <summary>
        /// Get the EPR Pair (Phi plus, or Einstein Podolsky Rosen), which is defined 
        /// as the 2 qubit register:
        /// (1 / sqrt(2))(|00> + |11>)
        /// This is also a bell state.
        /// </summary>
        /// <returns>A quantum register in the state of Phi plus or the EPR pair.</returns>
        /// <seealso cref="GetRegisterPhiPlus"/>
        /// <remarks>This is the same register as returned by GetRegisterPhiPlus, and
        /// is included for completeness for those who may not be as familiar with the
        /// names of the other bell states.</remarks>
        IQuantumRegister GetRegisterEPRPair();


        /// <summary>
        /// Get the EPR Pair (Phi plus, or Einstein Podolsky Rosen), which is defined 
        /// as the 2 qubit register:
        /// (1 / sqrt(2))(|00> + |11>)
        /// This is also a bell state.
        /// </summary>
        /// <returns>A quantum register in the state of Phi plus or the EPR pair.</returns>
        /// <seealso cref="GetRegisterEPRPair"/>
        /// <remarks>This is the same register as returned by GetRegisterEPRPair().</remarks>
        IQuantumRegister GetRegisterPhiPlus();


        /// <summary>
        /// Get the Phi minus register, which is defined as the two qubits:
        /// (1 / sqrt(2))(|00> - |11>)j
        /// This is also a bell state.
        /// </summary>
        /// <returns>A quantum register in the state of Phi minus.</returns>
        IQuantumRegister GetRegisterPhiMinus();


        /// <summary>
        /// Get the Psi plus register, which is defined as the two qubits:
        /// (1 / sqrt(2))(|01> + |10>)
        /// This is also a bell state.
        /// </summary>
        /// <returns>A quantum register in the state of Psi plus.</returns>
        IQuantumRegister GetRegisterPsiPlus();


        /// <summary>
        /// Get the Psi minus register, which is defined as the two qubits:
        /// (1 / sqrt(2))(|01> - |10>)
        /// This is also a bell state.
        /// </summary>
        /// <returns>A quantum register in the state of Psi plus.</returns>
        IQuantumRegister GetRegisterPsiMinus();


        /// <summary>
        /// Get the GHZ (Greenberger-Horne-Zeilinger) register, which is defined
        /// as the three qubits in the state:
        /// (1 / sqrt(2))(|000> + |111>)
        /// </summary>
        /// <returns>A quantum register in the GHZ state.</returns>
        IQuantumRegister GetRegisterGHZ();


        /// <summary>
        /// Get the W register, which is defined as the three qubits in the state:
        /// (1 / sqrt(3))(|100> + |010> + |001>)
        /// </summary>
        /// <returns>A quantum register in the W state.</returns>
        IQuantumRegister GetRegisterW();


        #endregion   //end of getting common states
    }
}
