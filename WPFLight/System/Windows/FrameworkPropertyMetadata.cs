using System.Collections.Generic;
using System.Windows.Data;
using WPFLight.Extensions;
namespace System.Windows {
	public class FrameworkPropertyMetadata : PropertyMetadata {
		public FrameworkPropertyMetadata ( ) {

        }

		public FrameworkPropertyMetadata ( object defaultValue ) : base ( defaultValue ) { 
        
        }

        public FrameworkPropertyMetadata (object defaultValue, FrameworkPropertyMetadataOptions options ) : base(defaultValue) {
            SetOptions(options);
        }

        public FrameworkPropertyMetadata (object defaultValue, PropertyChangedCallback callback, FrameworkPropertyMetadataOptions options ) : base(defaultValue, callback) {
            SetOptions(options);
        }

		public FrameworkPropertyMetadata (
			object defaultValue, PropertyChangedCallback callback, FrameworkPropertyMetadataOptions options, UpdateSourceTrigger defaultUpdateSourceTrigger ) : this (defaultValue, callback, options) {
			this.DefaultUpdateSourceTrigger = defaultUpdateSourceTrigger;
        }
		
		public FrameworkPropertyMetadata ( object defaultValue, PropertyChangedCallback callback ) : base ( defaultValue, callback ) { }

		public FrameworkPropertyMetadata ( PropertyChangedCallback callback ) : base ( callback ) { }

        #region Properties

        /// <summary>
        ///    The default UpdateSourceTrigger for two-way data bindings on this property.
        /// </summary>
        public UpdateSourceTrigger DefaultUpdateSourceTrigger { get; set; }

        /// <summary>
        /// Property is inheritable
        /// </summary>
		public bool Inherits { get; set; }

        /// <summary>
        ///     Property affects measurement
        /// </summary>
        public bool AffectsMeasure { get; set; }

        /// <summary>
        ///     Property affects arragement
        /// </summary>
        public bool AffectsArrange {
            get;
            set;
        }

        /// <summary>
        ///     Property affects parent's measurement
        /// </summary>
        public bool AffectsParentMeasure {
            get;
            set;
        }

        /// <summary>
        ///     Property affects parent's arrangement
        /// </summary>
        public bool AffectsParentArrange {
            get;
            set;
        }

        /// <summary>
        ///     Property affects rendering
        /// </summary>
        public bool AffectsRender {
            get;
            set;
        }

        /// <summary>
        ///     Property evaluation must span separated trees
        /// </summary>
        public bool OverridesInheritanceBehavior {
            get;
            set;
        }


        /// <summary>
        ///     Property cannot be data-bound
        /// </summary>
        public bool IsNotDataBindable {
            get;
            set;
        }

        /// <summary>
        ///     Data bindings on this property default to two-way
        /// </summary>
        public bool BindsTwoWayByDefault {
            get;
            set;
        }

        /// <summary>
        ///     The value of this property should be saved/restored when journaling by URI
        /// </summary>
        public bool Journal {
            get;
            set;
        }

        /// <summary>
        ///     This property's subproperties do not affect rendering.
        ///     For instance, a property X may have a subproperty Y.
        ///     Changing X.Y does not require rendering to be updated.
        /// </summary>
        public bool SubPropertiesDoNotAffectRender {
            get;
            set;
        }
 
        #endregion

        void SetOptions (FrameworkPropertyMetadataOptions options) {
            this.AffectsArrange = options.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsArrange);
            this.AffectsMeasure = options.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsMeasure);
            this.AffectsParentArrange = options.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsParentArrange);
            this.AffectsParentMeasure = options.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsParentMeasure);
            this.AffectsRender = options.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsRender);
            this.BindsTwoWayByDefault = options.IsFlagSet(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            this.Inherits = options.IsFlagSet(FrameworkPropertyMetadataOptions.Inherits);
            this.Journal = options.IsFlagSet(FrameworkPropertyMetadataOptions.Journal);
            this.IsNotDataBindable = options.IsFlagSet(FrameworkPropertyMetadataOptions.NotDataBindable);
            this.OverridesInheritanceBehavior = options.IsFlagSet(FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior);
            this.SubPropertiesDoNotAffectRender = options.IsFlagSet(FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender);
        }
    }

    [Flags]
    public enum FrameworkPropertyMetadataOptions : int {
        /// <summary>No flags</summary>
        None = 0x000,

        /// <summary>This property affects measurement</summary>
        AffectsMeasure = 0x001,

        /// <summary>This property affects arragement</summary>
        AffectsArrange = 0x002,

        /// <summary>This property affects parent's measurement</summary>
        AffectsParentMeasure = 0x004,

        /// <summary>This property affects parent's arrangement</summary>
        AffectsParentArrange = 0x008,

        /// <summary>This property affects rendering</summary>
        AffectsRender = 0x010,

        /// <summary>This property inherits to children</summary>
        Inherits = 0x020,

        /// <summary>
        /// This property causes inheritance and resource lookup to override values 
        /// of InheritanceBehavior that may be set on any FE in the path of lookup
        /// </summary>
        OverridesInheritanceBehavior = 0x040,

        /// <summary>This property does not support data binding</summary>
        NotDataBindable = 0x080,

        /// <summary>Data bindings on this property default to two-way</summary>
        BindsTwoWayByDefault = 0x100,

        /// <summary>This property should be saved/restored when journaling/navigating by URI</summary>
        Journal = 0x400,

        /// <summary>
        ///     This property's subproperties do not affect rendering.
        ///     For instance, a property X may have a subproperty Y.
        ///     Changing X.Y does not require rendering to be updated.
        /// </summary>
        SubPropertiesDoNotAffectRender = 0x800,
    }
}