using System;


namespace CoveX.ClassicalUtilities
{
    /// <summary>
    /// A matrix class of size m x n that contains complex numbers. 
    /// </summary>
    public class ComplexMatrix : CoveX.Base.ICoveObject
    {
        /// <summary>
        /// The individual cells of the matrix
        /// </summary>
        protected Complex[,] caCells;


        /// <summary>
        /// The message in the NotImplemented exception thrown for methods that are not
        /// yet implemented.
        /// </summary>
        public const string NOT_IMPLEMENTED_EXCEPTION_MESSAGE = "Not yet implemented";


        /// <summary>
        /// Default constructor, construct a 2 x 2 matrix with all entries set to 0.
        /// </summary>
        public ComplexMatrix()
        {
            this.caCells = new Complex[2, 2];
            this.ClearAllCells();
        }


        /// <summary>
        /// Construct a Rows x Columns sized matrix. Every entry will be 0
        /// </summary>
        /// <param name="Rows">Number of Rows in the matrix</param>
        /// <param name="Columns">Number of columns in the matrix</param>
        public ComplexMatrix(int Rows, int Columns)
        {
            if (Rows <= 0)
                throw new ArgumentOutOfRangeException("Must specify a positive number of rows");
            if (Columns <= 0)
                throw new ArgumentOutOfRangeException("Must specify a positive number of columns");

            this.caCells = new Complex[Rows, Columns];
            this.ClearAllCells();
        }


        /// <summary>
        /// Construct a matrix based on the specified cells
        /// </summary>
        /// <param name="Cells">The cells of the matrix</param>
        public ComplexMatrix(Complex[,] Cells)
        {
            if (Cells == null)
                throw new ArgumentNullException("Cannot specify null cells");

            this.caCells = Cells;
            //don't clear the cells in this constuctor
        }


        /// <summary>
        /// Construct an identity matrix or size IdentitySize. The Identity matrix created
        /// is IdentitySize x IdentitySize in size. Along the diagonal is 1, all other
        /// entries are 0.
        /// </summary>
        /// <param name="IdentitySize">The size of the identity matrix to create</param>
        /// <exception cref="ArgumentException">Thrown if IdentitySize is less than or equal
        /// to 0. Identity matrices must be of a defined size.</exception>
        public ComplexMatrix(int IdentitySize)
            : this(IdentitySize, IdentitySize)
        {
            if (IdentitySize <= 0)         //this should just be a double check
                throw new ArgumentException("IdentitySize must be greater than 0.");

            //the matrix is already created with all entries as 0, so just set all the entries
            //on the diagonal to 1.
            for (int i = 0; i < IdentitySize; i++)
                this.SetValue(i, i, new Complex(1));
        }


        /// <summary>
        /// Clear all the cells in the matrix to (0 + 0i)
        /// </summary>
        public void ClearAllCells()
        {
            //set each element to a new instance
            for (int iCurRow = 0; iCurRow < this.caCells.GetLength(0); iCurRow++)
            {
                for (int iCurColumn = 0; iCurColumn < this.caCells.GetLength(1); iCurColumn++)
                {
                    this.caCells[iCurRow, iCurColumn] = new Complex();
                }
            }
        }


        /// <summary>
        /// Return the number of columns in the matrix
        /// </summary>
        /// <returns>The number of columns</returns>
        public int GetNumberOfColumns()
        {
            return this.caCells.GetLength(1);
        }


        /// <summary>
        /// Return the number of rows in the matrix
        /// </summary>
        /// <returns>The number of rows</returns>
        public int GetNumberOfRows()
        {
            return this.caCells.GetLength(0);
        }


        /// <summary>
        /// Return the value of a specific cell in the matrix
        /// </summary>
        /// <param name="Row">The row in the matrix</param>
        /// <param name="Column">The column in the matrix</param>
        /// <returns>The value of the specific row</returns>
        /// <exception cref="InvalidMatrixLocationException">Returned if a location outside
        /// of the matrix is specified</exception>
        public Complex GetValue(long Row, long Column)
        {
            //first make sure the parameters specify a location that is actually in the matrix
            if ((Row < 0) || (Row > this.GetNumberOfRows()))
                throw new InvalidMatrixLocationException("Row " + Row.ToString() + " is outside the bounds of the matrix (0 to " + this.GetNumberOfRows().ToString());
            if ((Column < 0) || (Column > this.GetNumberOfColumns()))
                throw new InvalidMatrixLocationException("Column " + Column.ToString() + " is outside the bounds of the matrix (0 to " + this.GetNumberOfColumns().ToString());

            return this.caCells[Row, Column];
        }


        /// <summary>
        /// Set the value of a specific cell in the matrix
        /// </summary>
        /// <param name="Row">The row of the cell to set</param>
        /// <param name="Column">The column of the cell to set</param>
        /// <param name="Value">The value of the cell</param>
        /// <exception cref="InvalidMatrixLocationException">Thrown in a location is specified that
        /// doesn't exist in the matrix</exception>
        public void SetValue(long Row, long Column, Complex Value)
        {
            if ((Row < 0) || (Row >= this.GetNumberOfRows()))
                throw new InvalidMatrixLocationException("Invalid row specified for GenericMatrix.SetValue()");
            if ((Column < 0) || (Column >= this.GetNumberOfColumns()))
                throw new InvalidMatrixLocationException("Invalid column specified for GenericMatrix.SetValue()");

            this.caCells[Row, Column] = Value;
        }


        /// <summary>
        /// Returns a deep copy of this matrix, so the original can be modified without affecting
        /// the copy. (As would happen in copy by reference.) 
        /// </summary>
        /// <returns>The deep copy of this matrix</returns>
        /// <remarks>Note that instead of trying to utilize
        /// the Clone() method in the base class, it implements its own.</remarks>
        public ComplexMatrix Clone()
        {
            ComplexMatrix cRetVal = new ComplexMatrix(this.GetNumberOfRows(), this.GetNumberOfColumns());

            //copy each cell
            for (int iCurRow = 0; iCurRow < this.caCells.GetLength(0); iCurRow++)
            {
                for (int iCurColumn = 0; iCurColumn < this.caCells.GetLength(1); iCurColumn++)
                {
                    cRetVal.SetValue(iCurRow, iCurColumn, new Complex(this.caCells[iCurRow, iCurColumn]));
                }
            }

            return cRetVal;
        }


        /// <summary>
        /// Set this instance to a deep copy of Source. (Clone returns a deep copy of this object, while
        /// CopyFrom sets this object to a deep copy.
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public ComplexMatrix CopyFrom(ComplexMatrix Source)
        {
            //copy each cell
            for (int iCurRow = 0; iCurRow < this.caCells.GetLength(0); iCurRow++)
            {
                for (int iCurColumn = 0; iCurColumn < this.caCells.GetLength(1); iCurColumn++)
                {
                    this.SetValue(iCurRow, iCurColumn, new Complex(Source.caCells[iCurRow, iCurColumn]));
                }
            }

            return this;
        }


        /// <summary>
        /// Are two matrices equal?
        /// </summary>
        /// <param name="obj">The matrix to compare against</param>
        /// <returns>True if the matrices have the same values at each location.</returns>
        public bool Equals(ComplexMatrix obj)
        {
            //don't even bother comparing if they are not of equal size.
            if (this.GetNumberOfColumns() != obj.GetNumberOfColumns())
                return false;
            if (this.GetNumberOfRows() != obj.GetNumberOfRows())
                return false;

            //now go through and compare each element
            for (int iCurRow = 0; iCurRow < this.caCells.GetLength(0); iCurRow++)
            {
                for (int iCurColumn = 0; iCurColumn < this.caCells.GetLength(1); iCurColumn++)
                {
                    if (this.GetValue(iCurRow, iCurColumn).Equals(obj.GetValue(iCurRow, iCurColumn)) == false)
                        return false;
                }
            }

            //made it here then everything checks out
            return true;
        }


        /// <summary>
        /// Handle equality against any object.
        /// </summary>
        /// <param name="Value">Object to compare against</param>
        /// <returns>True if Value is an object that can be cast to a complex matrix.</returns>
        public override bool Equals(object Value)
        {
            //if Value can be cast to a more defined type, call that equal op, else they are not equal
            if ((Value is int?) == true)
                return this.Equals((int)Value);
            else if ((Value is double?) == true)
                return this.Equals((double)Value);
            else if ((Value is Complex) == true)
                return this.Equals((Complex)Value);
            else if ((Value is ComplexMatrix) == true)
                return this.Equals((ComplexMatrix)Value);
            else
                return false;
        }


        /// <summary>
        /// Override obtaining the hash code for the object.
        /// </summary>
        /// <returns>The hash code of the object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        /// <summary>
        /// A complex matrix can only be equal to an int if it is a 1 x 1 matrix of the same
        /// value of an int. This allows for multiplication to easily be checked out as in:
        ///         [3]
        /// [7 -4 5][2] = 8
        ///         [1]
        /// </summary>
        /// <param name="Value">The value to compare to</param>
        /// <returns>Only if the int equals the 1 x 1 matrix</returns>
        public bool Equals(int Value)
        {
            if ((this.GetNumberOfColumns() != 1) || (this.GetNumberOfRows() != 1))
                return false;

            return (this.GetValue(0, 0) == Value);
        }


        /// <summary>
        /// A complex matrix can only be equal to a double if it is a 1 x 1 matrix of the same
        /// value of a double. This allows for multiplication to easily be checked out as in:
        ///         [3]
        /// [7 -4 5][2] = 8
        ///         [1]
        /// </summary>
        /// <param name="Value">The value to compare to</param>
        /// <returns>Only if the double equals the 1 x 1 matrix</returns>
        public bool Equals(double Value)
        {
            if ((this.GetNumberOfColumns() != 1) || (this.GetNumberOfRows() != 1))
                return false;

            return (this.GetValue(0, 0) == Value);
        }


        /// <summary>
        /// A complex matrix can only be equal to a complex if it is a 1 x 1 matrix of the same
        /// value of a double. This allows for multiplication to easily be checked out as in:
        ///                              [(3 + 0i)]
        /// [(7 + 0i) (-4 + 0i) (5 + 0i)][(2 + 0i)] = (8 + 0i)
        ///                              [(1 + 0i)]
        /// </summary>
        /// <param name="Value">The value to compare to</param>
        /// <returns>Only if the complex equals the 1 x 1 matrix</returns>
        public bool Equals(Complex Value)
        {
            if ((this.GetNumberOfColumns() != 1) || (this.GetNumberOfRows() != 1))
                return false;

            return (this.GetValue(0, 0) == Value);
        }


        /// <summary>
        /// Equality operator for a complex matrix and an int.
        /// </summary>
        /// <param name="LeftSide">Left side to compare</param>
        /// <param name="RightSide">Right side to compare</param>
        /// <returns>True if LeftSide and RightSide are not equal. This requires that LeftSide is a 1 x 1 matrix
        /// with an element equal to RightSide.</returns>
        public static bool operator ==(ComplexMatrix LeftSide, int RightSide)
        {
            return LeftSide.Equals(RightSide);
        }


        /// <summary>
        /// Inequality operator for a ComplexMatrix and an int.
        /// </summary>
        /// <param name="LeftSide">Left side to compare</param>
        /// <param name="RightSide">Right side to compare</param>
        /// <returns>True if LeftSide and RightSide are not equal</returns>
        public static bool operator !=(ComplexMatrix LeftSide, int RightSide)
        {
            //just return the opposite of the equality operator
            return !(LeftSide == RightSide);
        }


        /// <summary>
        /// Equality operator for a ComplexMatrix and a double.
        /// </summary>
        /// <param name="LeftSide">Left side to compare</param>
        /// <param name="RightSide">Right side to compare</param>
        /// <returns>True if LeftSide and RightSide are not equal</returns>
        public static bool operator ==(ComplexMatrix LeftSide, Double RightSide)
        {
            return LeftSide.Equals(RightSide);
        }


        /// <summary>
        /// Inequality operator for a ComplexMatrix and a double.
        /// </summary>
        /// <param name="LeftSide">Left side to compare</param>
        /// <param name="RightSide">Right side to compare</param>
        /// <returns>True if LeftSide and RightSide are not equal</returns>
        public static bool operator !=(ComplexMatrix LeftSide, Double RightSide)
        {
            //just return the opposite of the equality operator
            return !(LeftSide == RightSide);
        }


        /// <summary>
        /// Equality operator for two ComplexMatrix instances
        /// </summary>
        /// <param name="LeftSide">Left side to compare</param>
        /// <param name="RightSide">Right side to compare</param>
        /// <returns>True if they are equal</returns>
        public static bool operator ==(ComplexMatrix LeftSide, ComplexMatrix RightSide)
        {
            return LeftSide.Equals(RightSide);
        }


        /// <summary>
        /// Inequality operator for two ComplexMatrix instances
        /// </summary>
        /// <param name="LeftSide">Left side to compare</param>
        /// <param name="RightSide">Right side to compare</param>
        /// <returns>True if they are unequal</returns>
        public static bool operator !=(ComplexMatrix LeftSide, ComplexMatrix RightSide)
        {
            return !(LeftSide == RightSide);
        }


        /// <summary>
        /// Equality operator to a ComplexMatrix and a complex
        /// </summary>
        /// <param name="LeftSide">Left side to compare</param>
        /// <param name="RightSide">Right side to compare</param>
        /// <returns>True if they are equal. This requires that LeftSide is a 1 x 1 matrix
        /// with an element equal to RightSide.</returns>
        public static bool operator ==(ComplexMatrix LeftSide, Complex RightSide)
        {
            //handle possible nulls (both nulls then equal, only one null then not equal)
            //if don't cast to object then this becomes an infinite recursive function
            if (((object)LeftSide == null) && ((object)RightSide == null))
                return true;
            else if (((object)LeftSide == null) && ((object)RightSide != null))
                return false;
            else if (((object)LeftSide != null) && ((object)RightSide == null))
                return false;

            return LeftSide.Equals(RightSide);
        }


        /// <summary>
        /// Inequality operator to a ComplexMatrix and a Complex.
        /// </summary>
        /// <param name="LeftSide">Left side to compare</param>
        /// <param name="RightSide">Right side to compare.</param>
        /// <returns>Returns true if LeftSide and RightSide are not equal.</returns>
        public static bool operator !=(ComplexMatrix LeftSide, Complex RightSide)
        {
            return !(LeftSide == RightSide);
        }


        /// <summary>
        /// Is this matrix an identity matrix?
        /// </summary>
        /// <returns>True if it is an identity matrix</returns>
        public bool IsIdentity()
        {
            //identity matrices must be square
            if (this.IsSquare() == false)
                return false;

            //now iterate through and make sure 1's are on the diagonal and 0's else where
            for (int iCurRow = 0; iCurRow < this.GetNumberOfRows(); iCurRow++)
            {
                for (int iCurColumn = 0; iCurColumn < this.GetNumberOfColumns(); iCurColumn++)
                {
                    if (iCurColumn == iCurRow)   //on the diagonal 
                    {
                        if (this.GetValue(iCurRow, iCurColumn) != new Complex(1))
                            return false;

                    }
                    else                         //not on the diagonal 
                    {
                        if (this.GetValue(iCurRow, iCurColumn) != new Complex(0))
                            return false;
                    }
                }                                //end of iteration through columns
            }                                    //end of iteration through rows

            //made it to here then everything checked out, so it is identity
            return true;
        }


        /// <summary>
        /// Perform addition between two matrices of equal size
        /// </summary>
        /// <param name="LeftSide">The left side of the addition</param>
        /// <param name="RightSide">The right side of the addition</param>
        /// <returns>The result of the addition</returns>
        /// <exception cref="MatrixSizeMismatchException">Thrown when the matrix sizes
        /// do not match for this operation</exception>
        public static ComplexMatrix operator +(ComplexMatrix LeftSide, ComplexMatrix RightSide)
        {
            ComplexMatrix cRetVal = new ComplexMatrix(LeftSide.GetNumberOfRows(), LeftSide.GetNumberOfColumns());

            //if the dimentions are different then don't even bother checking the cells
            if ((LeftSide.GetNumberOfColumns() != RightSide.GetNumberOfColumns())
            || (LeftSide.GetNumberOfRows() != RightSide.GetNumberOfRows()))
                throw new MatrixSizeMismatchException("Matrices were not of equal size, cannot add them");

            //go through each cell and add 
            for (int iCurRow = 0; iCurRow < LeftSide.caCells.GetLength(0); iCurRow++)
            {
                for (int iCurColumn = 0; iCurColumn < LeftSide.caCells.GetLength(1); iCurColumn++)
                {
                    cRetVal.SetValue(iCurRow, iCurColumn,
                        (LeftSide.GetValue(iCurRow, iCurColumn) + RightSide.GetValue(iCurRow, iCurColumn)));
                }
            }

            return cRetVal;
        }


        /// <summary>
        /// Allow an implict cast from int to a 1 x 1 matrix with the single element being Value.
        /// </summary>
        /// <param name="Value">Value to cast</param>
        /// <returns>A new 1 x 1 matrix with Value as the single element.</returns>
        public static implicit operator ComplexMatrix(int Value)
        {
            return new ComplexMatrix(new Complex[,] { { Value } });
        }


        /// <summary>
        /// Allow an implict cast from double to a 1 x 1 matrix with the single element being Value.
        /// </summary>
        /// <param name="Value">Value to cast</param>
        /// <returns>A new 1 x 1 matrix with Value as the single element.</returns>
        public static implicit operator ComplexMatrix(double Value)
        {
            return new ComplexMatrix(new Complex[,] { { Value } });
        }


        /// <summary>
        /// Allow an implict cast from complex to a 1 x 1 matrix with the single element being Value.
        /// </summary>
        /// <param name="Value">Value to cast</param>
        /// <returns>A new 1 x 1 matrix with Value as the single element</returns>
        public static implicit operator ComplexMatrix(Complex Value)
        {
            return new ComplexMatrix(new Complex[,] { { Value } });
        }


        /// <summary>
        /// Raise this matrix to the specified power. To the power of 0 return an identity matrix
        /// of the same size. To the power of 1 leaves the matrix unaltered. Any higher number is
        /// the matrix times itself Power times.
        /// </summary>
        /// <param name="Power">Power to raise to.</param>
        /// <returns>A reference to this matrix, after the operation is applied</returns>
        /// <exception cref="ArgumentException">Raised if argument Power is less than 0.</exception>
        public ComplexMatrix RaiseToPower(int Power)
        {
            ComplexMatrix cWorkingResult = this.Clone();

            //can only raise to a power if square
            if (this.IsSquare() == false)
                throw new MatrixSizeMismatchException("This matrix is not square. Only square matrices can be raised to a power.");

            //must pass in 0 or greater
            if (Power < 0)
                throw new ArgumentException("Cannot raise to powers less than 0.");

            //it is just the identity if raised to the power of 0.
            if (Power == 0)
            {
                this.caCells = (new ComplexMatrix(this.GetNumberOfColumns())).caCells;
                return this;
            }

            //go ahead and just multiple the matrix by this matrix the appropriate number of times.
            //note that starting at 0 instead of 1 since cTemp is already to the first power
            for (int i = 1; i < Power; i++)
            {
                cWorkingResult = cWorkingResult * this;
            }

            this.caCells = cWorkingResult.caCells;
            return this;
        }


        /// <summary>
        /// Transpose this matrix. This means that the columns are written as rows while
        /// preserving order.
        /// </summary>
        /// <returns>A reference to this matrix once the operation has been applied.</returns>
        public ComplexMatrix Transpose()
        {
            ComplexMatrix cWorkingCopy = new ComplexMatrix(this.GetNumberOfColumns(),
                this.GetNumberOfRows());

            //now go through writing the columns in the rows
            for (int iSourceRow = 0; iSourceRow < this.GetNumberOfRows(); iSourceRow++)
            {
                for (int iSourceColumn = 0; iSourceColumn < this.GetNumberOfColumns(); iSourceColumn++)
                {
                    cWorkingCopy.SetValue(iSourceColumn, iSourceRow, new Complex(this.GetValue(iSourceRow, iSourceColumn)));
                }       //end of iteration through columns of the source
            }           //end of iteration through rows on the source

            //now set this to the working copy and return
            this.caCells = cWorkingCopy.caCells;
            return this;
        }


        /// <summary>
        /// Return the conjugate transpose of this matrix. This is typically written A^H, although
        /// sometimes A^* is used.
        /// </summary>
        /// <returns></returns>
        public ComplexMatrix ConjugateTranspose()
        {
            //from page 40 in:
            //S. Lipschutz and M. Lipson, Theory and Problems of Linear Algebra, 3 ed. New York: McGraw-Hill, 2001.
            this.Transpose();
            for (int iCurRow = 0; iCurRow < this.GetNumberOfRows(); iCurRow++)
            {
                for (int iCurColumn = 0; iCurColumn < this.GetNumberOfColumns(); iCurColumn++)
                {
                    this.SetValue(iCurRow, iCurColumn, (this.GetValue(iCurRow, iCurColumn)).GetComplexConjugate());
                }            //end of iteration through columns
            }                //end of iteration through rows

            return this;
        }


        /// <summary>
        /// Get the tensor product of this matrix and another matrix. If this matrix is an m x n
        /// matrix and RightHandSide is a p x q matrix then the resulting matrix is a mp x nq 
        /// matrix.
        /// </summary>
        /// <param name="RightHandSide">The right hand side of the tensor product to return. (This 
        /// matrix is the left hand side.)</param>
        /// <returns>The tensor product of this matrix and RightHandSide.</returns>
        public ComplexMatrix Tensor(ComplexMatrix RightHandSide)
        {
            //implementation based off of page 33 - 35 of: P. Kaye, R. Laflamme, and M. Mosca, 
            //An Introduction to Quantum Computing. New York City, New York: Oxford University Press, 2007.
            Complex[,] caResult = new Complex[(this.GetNumberOfRows() * RightHandSide.GetNumberOfRows()),
                (this.GetNumberOfColumns() * RightHandSide.GetNumberOfColumns())];

            for (int iThisRowPos = 0; iThisRowPos < this.GetNumberOfRows(); iThisRowPos++)
            {
                for (int iThisColumnPos = 0; iThisColumnPos < this.GetNumberOfColumns(); iThisColumnPos++)
                {
                    for (int iRHSRowPos = 0; iRHSRowPos < RightHandSide.GetNumberOfRows(); iRHSRowPos++)
                    {
                        for (int iRHSColumnPos = 0; iRHSColumnPos < RightHandSide.GetNumberOfColumns(); iRHSColumnPos++)
                        {
                            caResult[((iThisRowPos * RightHandSide.GetNumberOfRows()) + iRHSRowPos),
                                ((iThisColumnPos * RightHandSide.GetNumberOfColumns()) + iRHSColumnPos)]
                                = this.GetValue(iThisRowPos, iThisColumnPos)
                                * RightHandSide.GetValue(iRHSRowPos, iRHSColumnPos);
                        }              //end of iteration through right hand side column positition
                    }                  //end of iteration through right hand side row position
                }                      //end of iteration through this columns
            }                          //end of iteration through this rows

            //replace the values in this matrix with the result and return
            this.caCells = caResult;
            return this;
        }


        /// <summary>
        /// Get the tensor product of this matrix and another matrix. If this matrix is an m x n
        /// matrix and RightHandSide is a p x q matrix then the resulting matrix is a mp x nq 
        /// matrix.
        /// </summary>
        /// <param name="LeftHandSide">The left hand side of the tensor product to return. (This 
        /// matrix is the right hand side.)</param>
        /// <returns>The tensor product of this matrix and LeftHandSide.</returns>
        public ComplexMatrix TensorAsRightHandSide(ComplexMatrix LeftHandSide)
        {
            this.caCells = (LeftHandSide.Tensor(this)).caCells;
            return this;
        }

        /// <summary>
        /// Tensor this matrix with itself NumberOfTimes times.
        /// </summary>
        /// <param name="NumberOfTimes">Number of times to tensor this matrix with itself.</param>
        /// <returns>A reference to this register after the tensoring has been applied.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if NumberOfTimes is less than 0.</exception>
        public ComplexMatrix TensorSelf(int NumberOfTimes)
        {
            if (NumberOfTimes < 0)
                throw new ArgumentOutOfRangeException("NumberOfTimes must be greater than or equal to 0, negative values are not allowed.");

            for (int i = 0; i < NumberOfTimes; i++)
            {
                this.Tensor(this);
            }

            return this;
        }


        /// <summary>
        /// Get the Hermitian conjugate (adjoint) of this matrix. This is typically
        /// notated by A^(Cross)
        /// </summary>
        /// <returns>A reference to this matrix after it has been transformed to its Hermitian
        /// conjugate (adjoint).</returns>
        public ComplexMatrix HermitianConjugate()
        {
            return this.ConjugateTranspose();
        }


        /// <summary>
        /// Get the Ajoint (Hermitian conjugate) of this matrix. This is typically
        /// notated by A^(Cross)
        /// </summary>
        /// <returns>A reference to this matrix after it has been transformed to
        /// its Ajoint (Hermitian conjugate).</returns>
        public ComplexMatrix Ajoint()
        {
            return this.HermitianConjugate();
        }


        /// <summary>
        /// Get the identity matrix (square matrix) of the specified Length. Hence
        /// the returned matrix is a Length x Length matrix with 1's along the diagonal. 
        /// </summary>
        /// <param name="Length">Length of the identity matrix.</param>
        /// <returns>The identity matrix of the specified length.</returns>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public static ComplexMatrix CreateIdentityMatrix(int Length)
        {
            throw new NotImplementedException(NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Is this matrix symmetric? In other words, does A^T = A?
        /// </summary>
        /// <returns>True if the matrix is symmetric, false otherwise.</returns>
        public bool IsSymmetric()
        {
            //from page 38 in:
            //S. Lipschutz and M. Lipson, Theory and Problems of Linear Algebra, 3 ed. New York: McGraw-Hill, 2001.
            if ((this.Clone()).Transpose() == this)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Is this matrix skew symmetric? In other words, does A^T = -A?
        /// </summary>
        /// <returns>True if the matrix is skew symmetric, false otherwise.</returns>
        public bool IsSkewSymmetric()
        {
            //from page 38 in:
            //S. Lipschutz and M. Lipson, Theory and Problems of Linear Algebra, 3 ed. New York: McGraw-Hill, 2001.
            if ((this.Clone()).Transpose() == (-1 * this.Clone()))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Is this matrix unitary? A matrix is unitary if (A^H)(A^-1) = (A^-1)(A^H) = I, or
        /// if A^H = A^-1.
        /// </summary>
        /// <returns>True if the matrix is unitary.</returns>
        public bool IsUnitary()
        {
            //from page 40 in:
            //S. Lipschutz and M. Lipson, Theory and Problems of Linear Algebra, 3 ed. New York: McGraw-Hill, 2001.
            if ((this.Clone()).ConjugateTranspose() == (this.Clone()).Inverse())
                return true;
            else
                return false;
        }


        /// <summary>
        /// Is this matrix normal? A matrix is normal if (A)(A^H) == (A^H)(A)
        /// </summary>
        /// <returns>True if the matrix is normal</returns>
        public bool IsNormal()
        {
            //from page 39 in:
            //S. Lipschutz and M. Lipson, Theory and Problems of Linear Algebra, 3 ed. New York: McGraw-Hill, 2001.
            if ((this * (this.Clone()).ConjugateTranspose()) == ((this.Clone()).ConjugateTranspose() * this))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Is this matrix square? That is, does it have the same number of rows and columns?
        /// </summary>
        /// <returns>True if the matrix is square.</returns>
        public bool IsSquare()
        {
            if (this.GetNumberOfColumns() == this.GetNumberOfRows())
                return true;
            else
                return false;
        }


        /// <summary>
        /// Is Compare the inverse of this matrix?
        /// </summary>
        /// <param name="Compare">Matrix to test to see if it is an inverse of this matrix.</param>
        /// <returns>True if Compare is an inverse.</returns>
        public bool IsInverse(ComplexMatrix Compare)
        {
            ComplexMatrix cResult = null;
            ComplexMatrix cExpected = null;

            //implementation based on page 35-36 of
            //S. Lipschutz and M. Lipson, Theory and Problems of Linear Algebra, 3 ed. New York: McGraw-Hill, 2001.

            //if they cannot be multiplied, then they are not inverses
            if (this.GetNumberOfColumns() != Compare.GetNumberOfRows())
                return false;

            //two matrices are inverses if AB = I
            cResult = this * Compare;
            cExpected = new ComplexMatrix(cResult.GetNumberOfColumns());

            if (cResult == cExpected)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Is this matrix invertible?
        /// </summary>
        /// <returns>True if this matrix is invertible</returns>
        public bool IsInvertible()
        {
            //implementation based on page 36 of
            //S. Lipschutz and M. Lipson, Theory and Problems of Linear Algebra, 3 ed. New York: McGraw-Hill, 2001.
            if (this.IsSquare() == false)
            {
                return false;
            }
            if (ComplexMatrix.Determinant(this) == Complex.Zero)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Is this matrix Hermitian? In other words, does A^H = A?
        /// </summary>
        /// <returns>True if this matrix is Hermatian.</returns>
        public bool IsHermitian()
        {
            //from page 40 in:
            //S. Lipschutz and M. Lipson, Theory and Problems of Linear Algebra, 3 ed. New York: McGraw-Hill, 2001.
            if ((this.Clone()).ConjugateTranspose() == this)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Is this matrix skew Hermitian? In other words, does A^H = -A
        /// </summary>
        /// <returns>True if the matrix is skew Hermitian</returns>
        public bool IsSkewHermitian()
        {
            //from page 40 in:
            //S. Lipschutz and M. Lipson, Theory and Problems of Linear Algebra, 3 ed. New York: McGraw-Hill, 2001.
            if ((this.Clone()).ConjugateTranspose() == (-1 * this))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Transform this matrix into its inverse. The inverse of matrix A is typically 
        /// written A^-1.
        /// </summary>
        /// <returns>A reference to this matrix after it has been inverted.</returns>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public ComplexMatrix Inverse()
        {
            //page 36, chapter 3
            throw new NotImplementedException(NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Perform subtraction between two matrices
        /// </summary>
        /// <param name="LeftSide">Left side of the subtraction</param>
        /// <param name="RightSide">Right side of the subtraction</param>
        /// <remarks>Result of the subtraction</remarks>
        /// <exception cref="MatrixSizeMismatchException">Thrown when the matrix sizes
        /// do not match for this operation</exception>
        public static ComplexMatrix operator -(ComplexMatrix LeftSide, ComplexMatrix RightSide)
        {
            ComplexMatrix cRetVal = new ComplexMatrix(LeftSide.GetNumberOfRows(), LeftSide.GetNumberOfColumns());

            //if the dimentions are different then don't even bother checking the cells
            if ((LeftSide.GetNumberOfColumns() != RightSide.GetNumberOfColumns())
            || (LeftSide.GetNumberOfRows() != RightSide.GetNumberOfRows()))
                throw new MatrixSizeMismatchException("Matrices were not of equal size, cannot subtract them");

            //go through each cell and add 
            for (int iCurRow = 0; iCurRow < LeftSide.caCells.GetLength(0); iCurRow++)
            {
                for (int iCurColumn = 0; iCurColumn < LeftSide.caCells.GetLength(1); iCurColumn++)
                {
                    cRetVal.SetValue(iCurRow, iCurColumn,
                        (LeftSide.GetValue(iCurRow, iCurColumn) - RightSide.GetValue(iCurRow, iCurColumn)));
                }
            }

            return cRetVal;
        }


        /// <summary>
        /// Perform matrix multiplication
        /// </summary>
        /// /// <param name="LeftSide">The right side of the multiplication</param>
        /// <param name="RightSide">The right side of the multiplication</param>
        /// <returns>Result of the multiplication</returns>
        /// <exception cref="MatrixSizeMismatchException">Thrown when the matrix sizes
        /// do not match for this operation</exception>
        public static ComplexMatrix operator *(ComplexMatrix LeftSide, ComplexMatrix RightSide)
        {
            ComplexMatrix cRetVal = new ComplexMatrix(LeftSide.GetNumberOfRows(), RightSide.GetNumberOfColumns());
            Complex cCurCell = new Complex();

            //make sure the rows in the right side match the columns in the left.
            if (LeftSide.GetNumberOfColumns() != RightSide.GetNumberOfRows())
                throw new MatrixSizeMismatchException("The number of columns in the left side must be equal to the number of rows in the right side");

            //everything validated, so go through and multiply
            for (int iCurRow = 0; iCurRow < cRetVal.GetNumberOfRows(); iCurRow++)
            {
                for (int iCurColumn = 0; iCurColumn < cRetVal.GetNumberOfColumns(); iCurColumn++)
                {
                    cCurCell.Set(0);

                    for (int iAddLocation = 0; iAddLocation < LeftSide.GetNumberOfColumns(); iAddLocation++)
                    {
                        Complex First = LeftSide.GetValue(iCurRow, iAddLocation);
                        Complex Second = RightSide.GetValue(iAddLocation, iCurColumn);
                        cCurCell = cCurCell + (LeftSide.GetValue(iCurRow, iAddLocation) * RightSide.GetValue(iAddLocation, iCurColumn));
                    }

                    //need to do a deep copy or else all cells will be set the same
                    cRetVal.SetValue(iCurRow, iCurColumn, (Complex)cCurCell.Clone());
                }
            }

            return cRetVal;
        }


        /// <summary>
        /// Multiple a matrix by the given value
        /// </summary>
        /// <param name="LeftSide">The right side of the multiplication, a scalar</param>
        /// <param name="RightSide">The right side of the multiplication, a matrix</param>
        /// <returns>Result of the multiplication</returns>
        public static ComplexMatrix operator *(double LeftSide, ComplexMatrix RightSide)
        {
            ComplexMatrix cRetVal = new ComplexMatrix(RightSide.GetNumberOfRows(), RightSide.GetNumberOfColumns());

            //go through each cell multiply by the scalar
            for (int iCurRow = 0; iCurRow < RightSide.GetNumberOfRows(); iCurRow++)
            {
                for (int iCurColumn = 0; iCurColumn < RightSide.GetNumberOfColumns(); iCurColumn++)
                {
                    cRetVal.SetValue(iCurRow, iCurColumn,
                        new Complex(LeftSide * RightSide.GetValue(iCurRow, iCurColumn)));
                }
            }

            return cRetVal;
        }


        /// <summary>
        /// Perform multiplication where this matrix is considered the right side and
        /// LeftSide is considered the left. This matrix is replaced with the result, so
        /// after performing x.MultipleAsRightSide(y), x = y * x. 
        /// </summary>
        /// <param name="LeftSide">The left side of the multiplication</param>
        /// <returns>A reference to the matrix after the multiplcation has been performed.</returns>
        public ComplexMatrix MultiplyAsRightSide(ComplexMatrix LeftSide)
        {
            ComplexMatrix cResult = LeftSide * this;

            //replace the state with that of the result
            this.caCells = cResult.caCells;

            return this;
        }


        /// <summary>
        /// Perform multiplication where this matrix is considered the left side and
        /// RightSide is considered the right. This matrix is replaced with the result, so
        /// after performing x.MultipleAsLeftSide(y), x = x * y. 
        /// </summary>
        /// <param name="RightSide">The right side of the multiplication</param>
        /// <returns>A reference to the matrix after the multiplcation has been performed.</returns>
        public ComplexMatrix MultiplyAsLeftSide(ComplexMatrix RightSide)
        {
            ComplexMatrix cResult = this * RightSide;

            //replace the state with that of the result
            this.caCells = cResult.caCells;

            return this;
        }




        /// <summary>
        /// Set this matrix based on cells, may change size of the matrix. Note that these
        /// values are deep copied into the matrix.
        /// </summary>
        /// <param name="Cells">The cells to set the matrix to</param>
        public void Set(Complex[,] Cells)
        {
            this.caCells = new Complex[Cells.GetLength(0), Cells.GetLength(1)];

            //go through each cell multiply by the scalar
            for (int iCurRow = 0; iCurRow < Cells.GetLength(0); iCurRow++)
            {
                for (int iCurColumn = 0; iCurColumn < Cells.GetLength(1); iCurColumn++)
                {
                    this.SetValue(iCurRow, iCurColumn,
                        new Complex(Cells[iCurRow, iCurColumn]));
                }
            }
        }

        /// <summary>
        /// Get the string representation of this matrix. Given matrix
        /// [a b c]
        /// [d e f]
        /// the string returned will be [ [a b c] [d e f] ]
        /// </summary>
        /// <returns>The string representation of this matrix</returns>
        public override string ToString()
        {
            System.Text.StringBuilder RetVal = new System.Text.StringBuilder();
            System.Text.StringBuilder CurLine = new System.Text.StringBuilder();

            RetVal.Append("[");

            for (int CurRow = 0; CurRow < this.caCells.GetLength(0); CurRow++)
            {
                RetVal.Append("[");

                CurLine = new System.Text.StringBuilder();
                for (int CurColumn = 0; CurColumn < this.caCells.GetLength(1); CurColumn++)
                {
                    if (CurLine.ToString() != "")
                        CurLine.Append(" ");
                    CurLine.Append("(" + this.caCells[CurRow, CurColumn].ToString() + ")");
                }

                RetVal.Append(CurLine);
                RetVal.Append("]");
            }
            RetVal.Append("]");
            return RetVal.ToString();
        }


        /// <summary>
        /// String representation of the matrix- possibly with new lines
        /// </summary>
        /// <param name="InsertNewLinesAfterRows">True to insert newlines after each row</param>
        /// <returns>The string representation of the matrix</returns>
        public string ToString(bool InsertNewLinesAfterRows)
        {
            System.Text.StringBuilder RetVal = new System.Text.StringBuilder();
            System.Text.StringBuilder CurLine = new System.Text.StringBuilder();

            //if no, then just normal ToString()
            if (InsertNewLinesAfterRows == false)
                return this.ToString();

            for (int CurRow = 0; CurRow < this.caCells.GetLength(0); CurRow++)
            {
                RetVal.Append("[");

                CurLine = new System.Text.StringBuilder();
                for (int CurColumn = 0; CurColumn < this.caCells.GetLength(1); CurColumn++)
                {
                    if (CurLine.ToString() != "")
                        CurLine.Append(" ");
                    CurLine.Append("(" + this.caCells[CurRow, CurColumn].ToString() + ")");
                }

                RetVal.Append(CurLine);
                RetVal.Append("]\n");
            }

            return RetVal.ToString();
        }

        public ComplexMatrix RREF()
        {
            int lead = 0;
            int rowCount = this.GetNumberOfRows();
            int columnCount = this.GetNumberOfColumns();
            for (int r = 0; r < rowCount; r++)
            {
                if (columnCount <= lead) break;
                int i = r;
                while (caCells[i, lead] == Complex.Zero)
                {
                    i++;
                    if (i == rowCount)
                    {
                        i = r;
                        lead++;
                        if (columnCount == lead)
                        {
                            lead--;
                            break;
                        }
                    }
                }
                for (int j = 0; j < columnCount; j++)
                {
                    Complex temp = caCells[r, j];
                    caCells[r, j] = caCells[i, j];
                    caCells[i, j] = temp;
                }
                Complex div = caCells[r, lead];
                if (div != Complex.Zero)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        caCells[r, j] /= div;
                    }
                }
                for (int j = 0; j < rowCount; j++)
                {
                    if (j != r)
                    {
                        Complex sub = caCells[j, lead];
                        for (int k = 0; k < columnCount; k++)
                        {
                            caCells[j, k] -= (sub * caCells[r, k]);
                        }
                    }
                }
                lead++;
            }
            return this;
        }

        public static Complex Determinant(ComplexMatrix matrix)
        {
            if (matrix.IsSquare() == false)
            {
                throw new MatrixSizeMismatchException(" Matrix is not square . Only squared matrices can be determined by this function.");
            }
            Complex det = 0;
            int dimension = matrix.GetNumberOfRows();
            if (dimension == 2)
            {
                Complex p = matrix.caCells[0, 0] * matrix.caCells[1, 1];
                Complex n = matrix.caCells[0, 1] * matrix.caCells[1, 0];
                det = p - n;
                return det;
            }
            for (int h = 0; h < dimension; h++)
            {
                if (matrix.caCells[h, 0] == Complex.Zero)
                {
                    continue;
                }
                ComplexMatrix reduced = ComplexMatrix.reduce(matrix, h);
                if (h % 2 == 0) det += ComplexMatrix.Determinant(reduced) * matrix.GetValue(h, 0);
                if (h % 2 == 1) det -= ComplexMatrix.Determinant(reduced) * matrix.GetValue(h, 0);
            }
            return det;
        }

        private static ComplexMatrix reduce(ComplexMatrix matrix, int index)
        {
            int rowCount = matrix.GetNumberOfRows();
            ComplexMatrix reduced = new ComplexMatrix(rowCount - 1, rowCount - 1);
            for (int h = 0, j = 0; h < rowCount; h++)
            {
                if (h == index)
                {
                    continue;
                }
                for (int i = 0, k = 0; i < rowCount; i++)
                {
                    if (i == 0)
                    {
                        continue;
                    }
                    reduced.SetValue(j, k, matrix.caCells[h, i]);
                    k++;
                }
                j++;
            }
            return reduced;
        }
    }
}
