using System;

namespace System.Windows
{
	public struct GridLength
	{
		static GridLength ()
		{
			Auto = new GridLength (0, GridUnitType.Auto);
			Star = new GridLength (1, GridUnitType.Star);
		}

		public GridLength (float value, GridUnitType unitType)
		{
			_value = value;
			_unitType = unitType;
		}

		#region Eigenschaften

		public float Value { get { return _value; } }

		public GridUnitType UnitType { get { return _unitType; } }

		public bool IsStar {
			get {
				return _unitType == GridUnitType.Star;
			}
		}

		public bool IsAuto {
			get {
				return _unitType == GridUnitType.Auto;
			}
		}

		public bool IsAbsolute {
			get {
				return _unitType == GridUnitType.Pixel;
			}
		}

		#endregion

		private float _value;
		private GridUnitType _unitType;
		public static readonly GridLength Auto;
		public static readonly GridLength Star;
	}

	public enum GridUnitType
	{
		Star,
		Auto,
		Pixel,
	}
}

