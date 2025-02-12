using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SharpIpp.Protocol.Models
{
    public class IppCollection : IReadOnlyList<IppAttribute>
    {
        public IppAttribute BegCollection { get; set; }

        public IppAttribute EndCollection { get; set; }

        public List<IppAttribute> ChildAttributes { get; }

        public int Count => ChildAttributes.Count;

        public IppAttribute this[int index] => ChildAttributes[index];

        public IppCollection(IppAttribute begCollection, IppAttribute endCollection, List<IppAttribute> childAttributes)
        {
            BegCollection = begCollection;
            EndCollection = endCollection;
            ChildAttributes = childAttributes;
        }

        public override string ToString()
        {
            return $"Collection {BegCollection.Name}: \nChild Attributes:\n{string.Join("\n", ChildAttributes.Select(s => s.ToString()))}";
        }

        public IEnumerator<IppAttribute> GetEnumerator() => ChildAttributes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
