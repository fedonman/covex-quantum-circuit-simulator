using CoveX.Base;
using System;

namespace CoveX.ClassicalUtilities
{
    /// <summary>
    /// A complex number class for use in simulating quantum systems
    /// </summary>
    /// <remarks>Implements ICloneable to get deep copies</remarks>
    public class Complex : ICoveObject, ICloneable
    {
        /// <summary>
        /// The tolerance for the equality operation. If the two numbers are being
        /// compared within this then consider them equal. This is to account for losses
        /// in accuracy because these are floating point operations.
        /// Tolerance is to 12 digits, double has a precission of 15-16 digits.
        /// </summary>
        public const double EQUALITY_TOLERANCE = 0.0000000000001;
        //                                       0 1234567891111111
        //                                                  0123456

        public static Complex Zero = new Complex(0);
        public static Complex One = new Complex(1);

        /// <summary>
        /// The real component of the complex number
        /// </summary>
        private double Real;

        /// <summary>
        /// The imaginary component of the complex number
        /// </summary>
        private double Imaginary;


        /// <summary>
        /// Default constuctor, creates a complex number 0 + 0i.
        /// </summary>
        public Complex()
        {
            this.Imaginary = 0.0;
            this.Real = 0.0;
        }


        /// <summary>
        /// Overload to construct a complex number with only the real portion set. (imaginary is 0)
        /// </summary>
        /// <param name="Real">The value to set the real component to</param>
        public Complex(double Real)
        {
            this.Real = Real;
            this.Imaginary = 0.0;
        }


        /// <summary>
        /// Overload to construct a complex number with only the real portion set. (imaginary is 0)
        /// </summary>
        /// <param name="Real">Value to set the real componet to</param>
        public Complex(int Real)
        {
            this.Real = Convert.ToDouble(Real);
            this.Imaginary = 0.0;
        }


        /// <summary>
        /// Overloaded constructor to create the complex number specified
        /// </summary>
        /// <param name="Real">The real part of the complex number</param>
        /// <param name="Imaginary">The imaginary part of the complex number</param>
        public Complex(double Real, double Imaginary)
        {
            this.Real = Real;
            this.Imaginary = Imaginary;
        }


        /// <summary>
        /// Construct a new complex number as a deep copy of an existing one
        /// </summary>
        /// <param name="CopyFrom">The complex number to copy from</param>
        public Complex(Complex CopyFrom)
        {
            this.Real = CopyFrom.Real;
            this.Imaginary = CopyFrom.Imaginary;
        }


        /// <summary>
        /// Allow for implicit conversions from double to complex. This is allowed implicitly since
        /// a double is just a complex number with the imaginary part 0.
        /// </summary>
        /// <param name="Real">The value of the real component.</param>
        /// <returns>The cast complex number</returns>
        public static implicit operator Complex(double Real)
        {
            Complex cRetVal = new Complex(Real);
            return cRetVal;
        }


        /// <summary>
        /// Allow for implicit conversions from int to complex. This is allowed implicitly since
        /// an int is just a complex number with the imaginary part 0.
        /// </summary>
        /// <param name="Real">The value of the real component.</param>
        /// <returns>The cast complex number</returns>
        public static implicit operator Complex(int Real)
        {
            Complex cRetVal = new Complex(Real);
            return cRetVal;
        }


        /// <summary>
        /// Return the absolute value of the complex number, defined by (a^2 + b^2)^(1/2).
        /// Since i^2 = 1, there will be no imaginary component. The absolute value is also known
        /// as the modulus.
        /// Reference: J. Stewart, Calculus, 3 ed. Pacific Grove, CA: Brooks/Cole Publishing Company, 1995.
        /// </summary>
        /// <returns>The absolute value of the complex number</returns>
	    public double AbsoluteValue()
        {
            return System.Math.Pow((System.Math.Pow(this.Real, 2) + System.Math.Pow(this.Imaginary, 2)), 0.5);
        }


        /// <summary>
        /// Returns the square of the absolute value. Since i^2 = 1, there will be no imaginary component
        /// </summary>
        /// <returns>The square of the absolute value</returns>
        public double AbsoluteValueSquared()
        {
            return System.Math.Pow(this.AbsoluteValue(), 2.0);
        }


        /// <summary>
        /// Get the square root of this complex number.
        /// </summary>
        /// <returns>Returns the square root of this complex number. Every complex number
        /// has two square roots, if this returns complex number c then the other
        /// square root is -c.</returns>
        /// <remarks>The implementation is based on 
        /// http://www.mathpropress.com/stan/bibliography/complexSquareRoot.pdf</remarks>
        public Complex SquareRoot()
        {
            double A = this.Real;
            double B = this.Imaginary;
            double p;
            double q;

            // if there is no imaginary part , just return the square root of the real
            if (B == 0)
            {
                if (A > 0)
                {
                    return new Complex(Math.Sqrt(A), 0);
                }
                if (A < 0)
                {
                    return new Complex(0, Math.Sqrt(Math.Abs(A)));
                }
                else
                {
                    return new Complex(0, 0);
                }
            }
            double ABSquaredRooted = Math.Sqrt(Math.Pow(A, 2.0) + Math.Pow(B, 2.0));
            p = (1.0 / Math.Sqrt(2)) * Math.Sqrt(ABSquaredRooted + A);
            q = ((double)Math.Sign(B) / Math.Sqrt(2)) * Math.Sqrt(ABSquaredRooted - A);
            return new Complex(p, q);
        }

        /// <summary>
        /// Get the imaginary part of the complex number. This is b in the complex
        /// number a + bi.
        /// </summary>
        /// <returns>The imaginary part of the complex number</returns>
        public double GetImaginary()
        {
            return this.Imaginary;
        }


        /// <summary>
        /// Get the real part of the complex number. This is a in the complex
        /// number a + bi.
        /// </summary>
        /// <returns>The real part of the complex number</returns>
        public double GetReal()
        {
            return this.Real;
        }


        /// <summary>
        /// Get the conjugate, which is just the complex number with the sign reversed
        /// on the imaginary part.
        /// </summary>
        /// <returns>The conjugate of this number. The complex number this is called
        /// on is unmodified.</returns>
        public Complex GetComplexConjugate()
        {
            //as defined on page 13 of 
            ////S. Lipschutz and M. Lipson, Theory and Problems of Linear Algebra, 3 ed. New York: McGraw-Hill, 2001.
            return new Complex(this.GetReal(), (-1.0 * this.GetImaginary()));
        }


        /// <summary>
        /// Set the complex number. The imaginary component will be 0.
        /// </summary>
        /// <param name="Real">The real number component of the complex number</param>
        public void Set(double Real)
        {
            this.Set(Real, 0);
        }


        /// <summary>
        /// Set the real and imaginary parts of the complex number
        /// </summary>
        /// <param name="Real">The real part of the complex number</param>
        /// <param name="Imaginary">The imaginary part of the complex number</param>
        public void Set(double Real, double Imaginary)
        {
            this.Real = Real;
            this.Imaginary = Imaginary;
        }


        /// <summary>
        /// Set only the real part of the complex number and leave the imaginary part
        /// unchanged.
        /// </summary>
        /// <param name="Real">The real part of the complex number to set</param>
        public void SetReal(double Real)
        {
            this.Real = Real;
        }

        /// <summary>
        /// Set only the imaginary part of the complex number and leave the
        /// imaginary part unchanged.
        /// </summary>
        /// <param name="Imaginary">The imaginary part of the complex number to set</param>
        public void SetImaginary(double Imaginary)
        {
            this.Imaginary = Imaginary;
        }


        /// <summary>
        /// Perform addition between complex numbers. Addition between complex numbers 
        /// is defined as (a + bi) + (c + di) = (a + c) + (b + d)i
        /// </summary>
        /// <param name="LeftSide">The left hand side of the addition operation</param>
        /// <param name="RightSide">The right hand side of the addition operation</param>
        /// <returns>The result of addition between the complex numbers</returns>
        public static Complex operator +(Complex LeftSide, Complex RightSide)
        {
            Complex RetVal = new Complex();

            RetVal.Real = LeftSide.Real + RightSide.Real;
            RetVal.Imaginary = LeftSide.Imaginary + RightSide.Imaginary;

            return RetVal;
        }


        /// <summary>
        /// Perform subtraction between two complex numbers. Subtraction is defined as
        /// (a + bi) - (c + di) = (a - c) + (b - d)i
        /// </summary>
        /// <param name="LeftSide">The left hand side of the subtraction operation</param>
        /// <param name="RightSide">The right hand side of the subtraction operation</param>
        public static Complex operator -(Complex LeftSide, Complex RightSide)
        {
            Complex RetVal = new Complex();

            RetVal.Real = LeftSide.Real - RightSide.Real;
            RetVal.Imaginary = LeftSide.Imaginary - RightSide.Imaginary;

            return RetVal;
        }

        /// <summary>
        /// Perform multiplication between complex numbers. Multiplication is defined as
        /// (a + bi)(c + di) = (ac - bd) + (ad + bc)i
        /// </summary>
        /// <param name="LeftSide">The left hand side of the multiplication operation</param>
        /// <param name="RightSide">The right hand side of the multiplication operation</param>
        /// <returns>The result of the multiplication</returns>
        public static Complex operator *(Complex LeftSide, Complex RightSide)
        {
            Complex RetVal = new Complex();

            RetVal.Real = (LeftSide.Real * RightSide.Real) - (LeftSide.Imaginary * RightSide.Imaginary);
            RetVal.Imaginary = (LeftSide.Real * RightSide.Imaginary) + (LeftSide.Imaginary * RightSide.Real);

            return RetVal;
        }


        /// <summary>
        /// Perform multiplication by a number. Multiplication is defined as
        /// x(a + bi) = ax + bxi
        /// </summary>
        /// <param name="LeftSide">The left hand side of the multiplication operation</param>
        /// <param name="RightSide">The right hand side of the multiplication operation</param>
        /// <returns>The result of the multiplication</returns>
        public static Complex operator *(double LeftSide, Complex RightSide)
        {
            Complex RetVal = new Complex();

            RetVal.Real = (LeftSide * RightSide.Real);
            RetVal.Imaginary = (LeftSide * RightSide.Imaginary);

            return RetVal;
        }


        /// <summary>
        /// Perform multiplication by a number. Multiplication is defined as
        /// x(a + bi) = ax + bxi
        /// </summary>
        /// <param name="LeftSide">The left hand side of the multiplication operation</param>
        /// <param name="RightSide">The right hand side of the multiplication operation</param>
        /// <returns>The result of the multiplication</returns>
        public static Complex operator *(Complex LeftSide, double RightSide)
        {
            Complex RetVal = new Complex();

            RetVal.Real = (LeftSide.Real * RightSide);
            RetVal.Imaginary = (LeftSide.Imaginary * RightSide);

            return RetVal;
        }

        /// <summary>
        /// Perform division between two complex numbers
        /// </summary>
        /// <param name="LeftSide">The left hand side of the division (numerator)</param>
        /// <param name="RightSide">The right hand side of the division (denominator)</param>
        /// <returns>The result of the division between the complex numbers</returns>
        /// <exception cref="System.NotImplementedException">Divsion is not yet needed, so it is not
        /// implemented.</exception>
        public static Complex operator /(Complex LeftSide, Complex RightSide)
        {
            Complex RetVal = new Complex();
            double A = LeftSide.Real;
            double B = LeftSide.Imaginary;
            double C = RightSide.Real;
            double D = RightSide.Imaginary;
            double CSquaredPlusDSquared = Math.Pow(RightSide.Real, 2) + Math.Pow(RightSide.Imaginary, 2);

            //for complex numbers c1 = a + bi and c2 = c + di, division
            //is defined as: c1 / c2 = x + yi where
            //x = (ac + bd) / (c^2 + d^2)
            //y = (bc - ad) / (c^2 + d^2)
            //From: http://scholar.hw.ac.uk/site/maths/topic10.asp?outline=no
            RetVal.Real = ((A * C) + (B * D)) / CSquaredPlusDSquared;
            RetVal.Imaginary = ((B * C) - (A * D)) / CSquaredPlusDSquared;

            return RetVal;
        }


        /// <summary>
        /// Are two complex numbers equal? Two complex numbers are equal if given complex numbers
        /// a + bi and c + di, a == c and b == d.
        /// </summary>
        /// <returns>The bool result of value equality</returns>
        public static bool operator ==(Complex LeftSide, Complex RightSide)
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
        /// Are two complex numbers not equal? Two complex numbers are equal if given complex numbers
        /// a + bi and c + di, a == c and b == d.
        /// </summary>
        /// <returns>The result of the value inequality</returns>
        public static bool operator !=(Complex LeftSide, Complex RightSide)
        {
            //just return the opposite of the equality operator
            return !(LeftSide == RightSide);
        }


        // **** .NET Specific ****

        /// <summary>
        /// Have to override the equal method
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if obj is the same complex number</returns>
        public override bool Equals(object obj)
        {
            //not equal if comparing to null
            if (obj == null)
                return false;

            //if it isn't a complex number then they are not equal
            Complex CompareTo = obj as Complex;
            if (((System.Object)CompareTo) == null)
                return false;

            //return true if the real and imaginary parts match
            return ((this.Real == CompareTo.Real) && (this.Imaginary == CompareTo.Imaginary));
        }


        /// <summary>
        /// Overloaded Equals() to compare to a Complex object
        /// </summary>
        /// <param name="CompareTo">The complex number to compare to</param>
        /// <returns>True if the complex number is the same</returns>
        public bool Equals(Complex CompareTo)
        {
            //if null is passed then return false
            if (((object)CompareTo) == null)
                return false;

            //consider the numbers equal if they are within the tolerance
            if (((this.Real - EQUALITY_TOLERANCE) < CompareTo.Real) && ((this.Real + EQUALITY_TOLERANCE) > CompareTo.Real))
            {
                if (((this.Real - EQUALITY_TOLERANCE) < CompareTo.Real) && ((this.Real + EQUALITY_TOLERANCE) > CompareTo.Real))
                {
                    return true;
                }
            }

            //made it here then it exited out of the above ifs, so return false
            return false;

            //old method of absolute equality
            //return true if the real and imaginary parts match
            //return ((this.Real == CompareTo.Real) && (this.Imaginary == CompareTo.Imaginary));
        }


        /// <summary>
        /// A complex number is considered equal to a double if the real part is the same as the
        /// double and the imaginary part is 0.
        /// </summary>
        /// <param name="CompareTo">Double to compare to</param>
        /// <returns>True if they are equal</returns>
        public bool Equals(double CompareTo)
        {
            //if null is passed then return false
            if (((object)CompareTo) == null)
                return false;

            //if there is an imaginary part then they are not equal
            if (this.GetImaginary() != 0)
                return false;

            //see if the real parts are equal
            return (this.GetReal() == CompareTo);
        }


        /// <summary>
        /// A complex number is considered equal to an ubt if the real part is the same as the
        /// int and the imaginary part is 0.
        /// </summary>
        /// <param name="CompareTo">Int to compare to</param>
        /// <returns>True if they are equal</returns>
        public bool Equals(int CompareTo)
        {
            //if null is passed then return false
            if (((object)CompareTo) == null)
                return false;

            //if there is an imaginary part then they are not equal
            if (this.GetImaginary() != 0)
                return false;

            //see if the real parts are equal (need to cast the int to a double)
            return (this.GetReal() == Convert.ToDouble(CompareTo));
        }



        /// <summary>
        /// Return the string representation of the complex number, a + bi.
        /// For negative imaginary numbers a - bi is returned instead of a + -bi
        /// </summary>
        /// <returns>The string representation of the complex number</returns>
        public override string ToString()
        {
            if (this.Imaginary < 0)     //have to convert to abs to not get " - -xi"
                return (this.Real.ToString() + " - " + (System.Math.Abs(this.Imaginary)).ToString() + "i");
            else
                return (this.Real.ToString() + " + " + this.Imaginary.ToString() + "i");
        }


        /// <summary>
        /// Returns the hash code for this object
        /// </summary>
        /// <returns>The hash code for the object</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        /// <summary>
        /// Return a deep copy of the object
        /// </summary>
        /// <returns>A deep copy of the current object</returns>
        public object Clone()
        {
            return ((object)new Complex(this.Real, this.Imaginary));
        }
    }
}
