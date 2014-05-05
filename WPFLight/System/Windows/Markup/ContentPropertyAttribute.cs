using System;
namespace System.Windows.Markup {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ContentPropertyAttribute : Attribute {
        public ContentPropertyAttribute () { }

        public ContentPropertyAttribute (string name) {
            this.Name = name;
        }

        #region Eigenschaften

        public string Name { get; private set; }

        #endregion
    }
}