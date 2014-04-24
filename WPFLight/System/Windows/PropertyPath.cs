using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace System.Windows {
	// Zusammenfassung:
	//     Implementiert eine Datenstruktur zum Beschreiben einer Eigenschaft als Pfad
	//     unter einer anderen Eigenschaft oder unter einem besitzenden Typ. Eigenschaftenpfade
	//     werden in der Datenbindung an Objekte sowie in Storyboards und Zeitachsen
	//     für Animationen verwendet.
	public sealed class PropertyPath {
		public PropertyPath (object parameter) : this (SingleStepPath, parameter) {
		}

		public PropertyPath (string path, params object[] pathParameters) {
			this.Path = path;
			if (pathParameters != null)
				this.PathParameters = new Collection<object> (pathParameters);
		}

		#region Eigenschaften

		public string Path { get; set; }

		public Collection<object> PathParameters { get; private set; }

		#endregion

		const string SingleStepPath = "(0)";
	}
}
