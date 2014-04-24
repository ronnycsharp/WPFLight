using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections;

public class UIElementCollection : IList<UIElement> {
    public UIElementCollection () {
        elements = new List<UIElement>();
    }

	public UIElementCollection (UIElement owner) : this( ) {
        collectionOwner = owner;
    }

    #region Eigenschaften

    public int Count {
        get { return elements.Count; }
    }

    public bool IsReadOnly {
        get { return false; }
    }

    #endregion

    public int IndexOf (UIElement item) {
        return elements.IndexOf(item);
    }

    public void Insert (int index, UIElement item) {
        if (collectionOwner != null)
            item.Parent = collectionOwner;

        elements.Insert(index, item);
    }

    public void RemoveAt (int index) {
        elements.RemoveAt(index);
    }

    public UIElement this[int index] {
        get {
            return elements[index];
        }
        set {
            if (collectionOwner != null)
                value.Parent = collectionOwner;

            elements[index] = value;
        }
    }

    public void Add (UIElement item) {
        if (collectionOwner != null)
            item.Parent = collectionOwner;

        elements.Add(item);
    }

    public void Clear () {
        elements.Clear();
    }

    public bool Contains (UIElement item) {
        return elements.Contains(item);
    }

    public void CopyTo (UIElement[] array, int arrayIndex) {
        elements.CopyTo(array, arrayIndex);
    }

    public bool Remove (UIElement item) {
        return elements.Remove(item);
    }

    public IEnumerator<UIElement> GetEnumerator () {
        return elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator () {
        return elements.GetEnumerator();
    }

    private UIElement       collectionOwner;
    private List<UIElement> elements;
}