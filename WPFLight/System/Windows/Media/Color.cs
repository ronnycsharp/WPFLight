// Original-License, Modified Namespace by Ronny Weidemann March, 2014

#region License
/*
MIT License
Copyright © 2006 The Mono.Xna Team

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion License


using Microsoft.Xna.Framework;
using System.ComponentModel;
using System;
using System.Runtime.Serialization;

namespace System.Windows.Media
{
	/// <summary>
	/// Describe a 32-bit packed color.
	/// </summary>
	[DataContract]
	[TypeConverter(typeof(ColorConverter))]
	public struct Color : IEquatable<Color>
	{
		internal Color(uint packedValue)
		{
			_packedValue = packedValue;
			// ARGB
			//_packedValue = (packedValue << 8) | ((packedValue & 0xff000000) >> 24);
			// ABGR			
			//_packedValue = (packedValue & 0xff00ff00) | ((packedValue & 0x000000ff) << 16) | ((packedValue & 0x00ff0000) >> 16);
		}

		public Color ( float r, float g, float b ) {
			_packedValue = 0;

			R = (byte)MathHelper.Clamp(r * 255, Byte.MinValue, Byte.MaxValue);
			G = (byte)MathHelper.Clamp(g * 255, Byte.MinValue, Byte.MaxValue);
			B = (byte)MathHelper.Clamp(b * 255, Byte.MinValue, Byte.MaxValue);
			A = 0xFF;
		}

		public Color ( float r, float g, float b, float a ) {
			_packedValue = 0;

			R = (byte)MathHelper.Clamp(r * 255, Byte.MinValue, Byte.MaxValue);
			G = (byte)MathHelper.Clamp(g * 255, Byte.MinValue, Byte.MaxValue);
			B = (byte)MathHelper.Clamp(b * 255, Byte.MinValue, Byte.MaxValue);
			A = (byte)MathHelper.Clamp(a * 255, Byte.MinValue, Byte.MaxValue);
		}

		public Color ( byte r, byte g, byte b, byte a ) {
			_packedValue = 0;

			R = r;
			G = g;
			B = b;
			A = a;
		}

		public Color ( byte r, byte g, byte b ) {
			_packedValue = 0;

			R = r;
			G = g;
			B = b;
			A = 0xFF;
		}
						
		public static Color FromArgb (byte a, byte r, byte g, byte b)
		{
			return new Color (r, g, b) * ( ( ( float ) a ) / 255f );

			/*
			Color c;

			c._packedValue = 0;
			c.R = (byte)MathHelper.Clamp(r, Byte.MinValue, Byte.MaxValue);
			c.G = (byte)MathHelper.Clamp(g, Byte.MinValue, Byte.MaxValue);
			c.B = (byte)MathHelper.Clamp(b, Byte.MinValue, Byte.MaxValue);
			c.A = (byte)MathHelper.Clamp(a, Byte.MinValue, Byte.MaxValue);

			return c;
			*/
		}

		public static Color FromRgb (byte r, byte g, byte b)
		{
			return new Color (r, g, b);

			/*
			Color c;

			c._packedValue = 0;
			c.R = (byte)MathHelper.Clamp(r, Byte.MinValue, Byte.MaxValue);
			c.G = (byte)MathHelper.Clamp(g, Byte.MinValue, Byte.MaxValue);
			c.B = (byte)MathHelper.Clamp(b, Byte.MinValue, Byte.MaxValue);
			c.A = 255;

			return c;
			*/
		}

		/// <summary>
		/// Gets or sets the blue component of <see cref="Color"/>.
		/// </summary>
		[DataMember]
		public byte B
		{
			get
			{
				return (byte)(this._packedValue >> 16);
			}
			set
			{
				this._packedValue = (this._packedValue & 0xff00ffff) | (uint)(value << 16);
			}
		}

		/// <summary>
		/// Gets or sets the green component of <see cref="Color"/>.
		/// </summary>
		[DataMember]
		public byte G
		{
			get
			{
				return (byte)(this._packedValue >> 8);
			}
			set
			{
				this._packedValue = (this._packedValue & 0xffff00ff) | ((uint)(value << 8));
			}
		}

		/// <summary>
		/// Gets or sets the red component of <see cref="Color"/>.
		/// </summary>
		[DataMember]
		public byte R
		{
			get
			{
				return (byte)(this._packedValue);
			}
			set
			{
				this._packedValue = (this._packedValue & 0xffffff00) | value;
			}
		}

		/// <summary>
		/// Gets or sets the alpha component of <see cref="Color"/>.
		/// </summary>
		[DataMember]
		public byte A
		{
			get
			{
				return (byte)(this._packedValue >> 24);
			}
			set
			{
				this._packedValue = (this._packedValue & 0x00ffffff) | ((uint)(value << 24));
			}
		}

		/// <summary>
		/// Compares whether two <see cref="Color"/> instances are equal.
		/// </summary>
		/// <param name="a"><see cref="Color"/> instance on the left of the equal sign.</param>
		/// <param name="b"><see cref="Color"/> instance on the right of the equal sign.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public static bool operator ==(Color a, Color b)
		{
			return (a.A == b.A &&
				a.R == b.R &&
				a.G == b.G &&
				a.B == b.B);
		}

		/// <summary>
		/// Compares whether two <see cref="Color"/> instances are not equal.
		/// </summary>
		/// <param name="a"><see cref="Color"/> instance on the left of the not equal sign.</param>
		/// <param name="b"><see cref="Color"/> instance on the right of the not equal sign.</param>
		/// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>	
		public static bool operator !=(Color a, Color b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Gets the hash code for <see cref="Color"/> instance.
		/// </summary>
		/// <returns>Hash code of the object.</returns>
		public override int GetHashCode()
		{
			return this._packedValue.GetHashCode();
		}

		/// <summary>
		/// Compares whether current instance is equal to specified object.
		/// </summary>
		/// <param name="obj">The <see cref="Color"/> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public override bool Equals(object obj)
		{
			return ((obj is Color) && this.Equals((Color)obj));
		}

		/// <summary>
		/// Performs linear interpolation of <see cref="Color"/>.
		/// </summary>
		/// <param name="value1">Source <see cref="Color"/>.</param>
		/// <param name="value2">Destination <see cref="Color"/>.</param>
		/// <param name="amount">Interpolation factor.</param>
		/// <returns>Interpolated <see cref="Color"/>.</returns>
		public static Color Lerp(Color value1, Color value2, Single amount)
		{		
			return new Color(   
				(byte)MathHelper.Lerp(value1.R, value2.R, amount),
				(byte)MathHelper.Lerp(value1.G, value2.G, amount),
				(byte)MathHelper.Lerp(value1.B, value2.B, amount),
				(byte)MathHelper.Lerp(value1.A, value2.A, amount) );
		}

		/// <summary>
		/// Multiply <see cref="Color"/> by value.
		/// </summary>
		/// <param name="value">Source <see cref="Color"/>.</param>
		/// <param name="scale">Multiplicator.</param>
		/// <returns>Multiplication result.</returns>
		public static Color Multiply(Color value, float scale)
		{
			return new Color(
				(byte)(value.R * scale), 
				(byte)(value.G * scale), 
				(byte)(value.B * scale), 
				(byte)(value.A * scale));
		}

		/// <summary>
		/// Multiply <see cref="Color"/> by value.
		/// </summary>
		/// <param name="value">Source <see cref="Color"/>.</param>
		/// <param name="scale">Multiplicator.</param>
		/// <returns>Multiplication result.</returns>
		public static Color operator *(Color value, float scale)
		{
			return new Color(
				(byte)(value.R * scale), 
				(byte)(value.G * scale), 
				(byte)(value.B * scale), 
				(byte)(value.A * scale));
		}		
			
		/// <summary>
		/// Gets or sets packed value of this <see cref="Color"/>.
		/// </summary>
		[CLSCompliant(false)]
		internal UInt32 PackedValue
		{
			get { return _packedValue; }
			set { _packedValue = value; }
		}

		/// <summary>
		/// Converts the color values of this instance to its equivalent string representation.
		/// </summary>
		/// <returns>The string representation of the color value of this instance.</returns>
		public override string ToString ()
		{
			return string.Format("[Color: R={0}, G={1}, B={2}, A={3}, PackedValue={4}]", R, G, B, A, PackedValue);
		}

		#region IEquatable<Color> Members

		/// <summary>
		/// Compares whether current instance is equal to specified <see cref="Color"/>.
		/// </summary>
		/// <param name="other">The <see cref="Color"/> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public bool Equals(Color other)
		{
			return this.PackedValue == other.PackedValue;
		}

		#endregion

		private uint _packedValue;

	}
}
