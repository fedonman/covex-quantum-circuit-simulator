

/*
namespace Cove.LocalSimulation
{

    /// <summary>
    /// A simulation of the smallest unit of quantum information, the quantum bit, 
    /// commonly refered to as a qubit.
    /// </summary>
    public class Qubit : IQubit
    {
        /// <summary>
        /// The state of the qubit
        /// </summary>
        private ComplexMatrix State;

        /// <summary>
        /// The label for one
        /// </summary>
        private string LabelOne;

        /// <summary>
        /// The label for zero
        /// </summary>
        private string LabelZero;

        /// <summary>
        /// The random number source needed for the classical simulation.
        /// </summary>
        /// <remarks>
        /// This is a static member so that one value is shared across all instances. This prevents
        /// the problem of creating a bunch of qubits at once that are seeded with the same timer
        /// value- the result being far from even pseudo-random.
        /// </remarks>
        private static Random cRandomSource = new Random();


        /// <summary>
        /// The default location of the quantum resource
        /// </summary>
        public const string DEFAULT_LOCATION = "localhost";   

        /// <summary>
        /// The location of the quantum resource- for this implementation it cannot be changed
        /// from "localhost".
        /// </summary>
        private string Location;


        /// <summary>
        /// Default constructor, set to 0
        /// </summary>
        public Qubit()
        {
            this.CommonConstruction();
        }


        /// <summary>
        /// Overloaded constructor to set the location of the resource at construction time
        /// </summary>
        /// <param name="Location">Location of the quantum resource</param>
        public Qubit(string Location)
        {
            this.CommonConstruction();
            this.SetLocation(Location);
        }


        /// <summary>
        /// Overload the constructor to also set the labels
        /// </summary>
        /// <param name="LabelZero">label for |0></param>
        /// <param name="LabelOne">label for |1></param>
        public Qubit(string LabelZero, string LabelOne)
        {
            this.CommonConstruction();
            this.SetLabels(LabelZero, LabelOne);
        }


        /// <summary>
        /// Overload constructor to set the location and labels.
        /// </summary>
        /// <param name="Location">Location of the quantum resource</param>
        /// <param name="LabelZero">Label for |0></param>
        /// <param name="LabelOne">Label for |1></param>
        public Qubit(string Location, string LabelZero, string LabelOne)
        {
            this.CommonConstruction();
            this.SetLocation(Location);
            this.SetLabels(LabelZero, LabelOne);
        }


        /// <summary>
        /// Carry out initialization that is common to all constructors
        /// </summary>
        private void CommonConstruction()
        {
            this.State = new ComplexMatrix(2, 1);
            this.Location = DEFAULT_LOCATION;
            this.LabelOne = "1";
            this.LabelZero = "0";
            this.ResetToZero();
        }


        /// <summary>
        /// Apply the specified operation to the qubit.
        /// </summary>
        /// <param name="Operation">The operation to apply</param>
        /// <exception cref="ArgumentException">Currently only works on AbstractSimulatedOperation
        /// objects</exception>
        /// <exception cref="NotUnitaryOperationException">Thrown if the operation passed
        /// is not a valid quantum operation. All operations must be unitary.</exception>
        public void ApplyOperation(IQubitOperation Operation)
        {
            GeneralSimulatedOperation ApplyOperation = null;

            if ((Operation.GetType()).IsSubclassOf(typeof(GeneralSimulatedOperation)) == false)
                throw new ArgumentException("The local simulation can only operate on AbstractSimulatedOperation object");
            if (Operation.IsValidOperation() == false)
                throw new NotUnitaryOperationException("The operation passed is not unitary, so it is not a valid quantum operation");

            ApplyOperation = (GeneralSimulatedOperation)Operation;
            this.State = ApplyOperation.GetOperationComplexMatrix() * this.State; 
        }


        /// <summary>
        /// Apply the specified operations to the qubit.
        /// </summary>
        /// <param name="Operations">The operations to apply</param>
        /// <exception cref="ArgumentException">Currently only works on AbstractSimulatedOperation
        /// objects</exception>
        /// <exception cref="NotUnitaryOperationException">Thrown if any of the operations passed
        /// are not unitary. All quantum operation must be unitary. If this exception is thrown
        /// then none of the operations are applied to the qubit- there is no change in the state of
        /// the qubit.</exception>
        public void ApplyOperations(IQubitOperation[] Operations)
        {
            if (Operations.GetType() != typeof(GeneralSimulatedOperation[]))
                throw new ArgumentException("The local simulation can only operate on GeneralSimulatedOperation object");

            //first check to make sure all the operations are valid.
            foreach(IQubitOperation cCurOp in Operations)
            {
                if(cCurOp.IsValidOperation() == false)
                    throw new NotUnitaryOperationException("One of the operations passed is not unitary, and so it is not a valid qubit operation. (Operation: " + cCurOp.ToString() + ")");
            }

            //apply all the operations
            foreach (IQubitOperation cCurOp in Operations)
                this.ApplyOperation(cCurOp);
        }


        /// <summary>
        /// Get the string representation of one
        /// </summary>
        /// <returns>The string representation of one</returns>
        public string GetLabelOne()
        {
            return this.LabelOne;
        }


        /// <summary>
        /// Get the string representation of zero
        /// </summary>
        /// <returns>The string representation of zero</returns>
        public string GetLabelZero()
        {
            return this.LabelZero;
        }


        /// <summary>
        /// Get the location of the quantum resource. In this implementation it is always "localhost"
        /// </summary>
        public string GetLocation()
        {
            return this.Location;
        }


        /// <summary>
        /// Reset to an abstract state. Destroys any existing state (which is not reversible), so this
        /// is consider to reset the state.
        /// </summary>
        /// <param name="Zero"></param>
        /// <param name="One"></param>
        /// <exception cref="ArgumentException">Can only set based on Complex numbers, doubles, or ints
        /// in the current implementation</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The squares of the absolute values
        /// must add up to 1. If not then this exception is thrown.</exception>
        public void ResetTo(object Zero, object One)
        {
            System.Collections.Generic.List<Type> listAllowedTypes = new System.Collections.Generic.List<Type>();
            Complex ApplyZero = null;
            Complex ApplyOne = null;

            //allowed types
            listAllowedTypes.Add(typeof(Complex));
            listAllowedTypes.Add(typeof(int));
            listAllowedTypes.Add(typeof(System.Int32));
            listAllowedTypes.Add(typeof(double));
            listAllowedTypes.Add(typeof(System.Double));

            //check the type
            if (listAllowedTypes.Contains(Zero.GetType()) == false)
                throw new ArgumentException("Current implementation only works on Complex, double, and int types. (Argument Zero is not one of these types, it is type " + Zero.GetType().ToString() + ")");
            if (listAllowedTypes.Contains(One.GetType()) == false)
                throw new ArgumentException("Current implementation only works on Complex, double, and int types. (Argument One is not one of these types, it is type " + One.GetType().ToString() + ")");

            //in this implementation will always apply as complex numbers
            if (Zero.GetType() == typeof(Complex))
                ApplyZero = new Complex((Complex)Zero);
            else 
                ApplyZero = new Complex(System.Convert.ToDouble(Zero));

            if (One.GetType() == typeof(Complex))
                ApplyOne = new Complex((Complex)One);
            else
                ApplyOne = new Complex(System.Convert.ToDouble(One));

            //make sure that the squares of the complex numbers add up to one- else the qubit is in 
            //and invalid state since probabilities must add up to 1.
            if((ApplyZero.AbsoluteValueSquared() + ApplyOne.AbsoluteValueSquared()) != 1.0)
                throw new ArgumentOutOfRangeException("The squares of the absolute values must add up to 1.");

            //everything checked out ok, so set the state.
            this.State = new ComplexMatrix(new Complex[,] {{ApplyZero}, {ApplyOne}});
        }


        /// <summary>
        /// Reset the qubit to |1>. Destroys any existing state (which is not reversible), so this
        /// is consider to reset the state.
        /// </summary>
        public void ResetToOne()
        {
            this.ResetTo(0, 1);
        }

        /// <summary>
        /// Reset the qubit to |0>. Destroys any existing state (which is not reversible), so this
        /// is consider to reset the state.
        /// </summary>
        public void ResetToZero()
        {
            this.ResetTo(1, 0);
        }


        /// <summary>
        /// Measure the qubit. Sets it absolutely to |0> xor |1>
        /// </summary>
        /// <returns>The result of the measurement, 0 xor 1</returns>
        public int Measure()
        {
            double dRandomNum = cRandomSource.NextDouble();

            //if it is within the range of |0>, collapse it to that. else |1>
            if (dRandomNum <= (this.State.GetValue(0, 0)).AbsoluteValueSquared())
            {
                this.ResetToZero();
                return 0;
            }
            else
            {
                this.ResetToOne();
                return 1;
            }
        }


        /// <summary>
        /// Measure the qubit- collapses it absolutely to |0> xor |1>. Instaead of
        /// returning 0 or 1 like Measure() it returns the string label of
        /// the result.
        /// </summary>
        /// <returns>The string label of what the qubit collaped to.</returns>
        public string MeasureWithLabel()
        {
            //measure the qubit, then return the appropriate label.
            if (this.Measure() == 0)
                return this.LabelZero;
            else
                return this.LabelOne;
        }


        /// <summary>
        /// Perform the Hadamard operation on the qubit. This operation is
        /// also known as Hadamard-Walsh and the square root of not.
        /// </summary>
        public void OperationHadamard()
        {
            //just wraps up the more verbose notation for easy use.
            this.ApplyOperation(Operations.Hadamard);
        }


        /// <summary>
        /// Perform the identity operation on the qubit. This does not change the
        /// state of the qubit.
        /// </summary>
        public void OperationIdentity()
        {
            //just wraps up the more verbose notation for easy use.
            this.ApplyOperation(Operations.Identity);
        }


        /// <summary>
        /// Perform the Not operation on the qubit. This operation is also
        /// known as the X gate.
        /// </summary>
        public void OperationNot()
        {
            //just wraps up the more verbose notation for easy use.
            this.ApplyOperation(Operations.Not);
        }


        /// <summary>
        /// Perform the S gate operation, the phase gate, on the qubit.
        /// </summary>
        public void OperationSGate()
        {
            //just wraps up the more verbose notation for easy use.
            this.ApplyOperation(Operations.SGate);
        }


        /// <summary>
        /// Perform the T gate operation, the pi/8 phase gate, on
        /// the qubit.
        /// </summary>
        public void OperationTGate()
        {
            //just wraps up the more verbose notation for easy use.
            this.ApplyOperation(Operations.TGate);
        }


        /// <summary>
        /// Perform the Y Gate operation on the qubit
        /// </summary>
        public void OperationYGate()
        {
            //just wraps up the more verbose notation for easy use.
            this.ApplyOperation(Operations.YGate);
        }


        /// <summary>
        /// Perform the Z Gate operation on the qubit.
        /// </summary>
        public void OperationZGate()
        {
            //just wraps up the more verbose notation for easy use.
            this.ApplyOperation(Operations.ZGate);
        }



        /// <summary>
        /// Sets the location of the quantum resource. For the purpose of the local simulation
        /// implementation, this cannot be changed.
        /// </summary>
        /// <remarks>
        /// In a non local implementation, changing the location will reset the qubit. (Qubits
        /// cannot be copied to the new resource due to the no cloning theorem.)
        /// </remarks>
        /// <param name="NewLocation">The location of the quantum resource</param>
        /// <exception cref="ArgumentException">Thrown if location set to anything other
        /// than "localhost"</exception>
        public void SetLocation(string NewLocation)
        {
            if (NewLocation.ToLower() != "localhost")
                throw new ArgumentException("The location of the quantum resource cannot be changed from "
                    + "'localhost' for the local simulation implementation.");

            //made it to here then it is the default "localhost"
        }


        /// <summary>
        /// Change the label of 1
        /// </summary>
        /// <param name="LabelOne">The new label of one</param>
        public void SetLabelOne(string LabelOne)
        {
            this.LabelOne = LabelOne;
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
            this.LabelZero = LabelZero;
        }

        /// <summary>
        /// Return the string representation of the qubit, which will be (a + bi)|0> + (c + di)|1>
        /// </summary>
        /// <returns>The string representation of the qubt</returns>
        public override string ToString()
        {
            return "(" + this.State.GetValue(0, 0).ToString() + ")|0> + (" + this.State.GetValue(1, 0).ToString() + ")|1>";
        }


        /// <summary>
        /// Get the string representation of the qubit, using the labels provided
        /// for 1 and 0.
        /// </summary>
        /// <returns></returns>
        public string ToStringWithLabels()
        {
            return "(" + this.State.GetValue(0, 0).ToString() + ")|" + this.GetLabelZero() + "> + (" + this.State.GetValue(1, 0).ToString() + ")|" + this.GetLabelOne() + ">";
        }
    }                                            
} 
                                               
 
*/