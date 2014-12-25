namespace System.ComponentModel {
    public class TypeConverterAttribute : Attribute {
        public TypeConverterAttribute ( ) {

        }

        public TypeConverterAttribute ( Type converterType ) {
            this.ConverterTypeName = converterType.Name;
        }

        public TypeConverterAttribute ( string name ) {
            if ( name == null || name == String.Empty )
                throw new ArgumentNullException ( "name" );

            this.ConverterTypeName = name;
        }

        #region Properties 

        public string ConverterTypeName { get; set; }

        #endregion
    }
}