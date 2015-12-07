using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Helper.Util;

namespace NHibernate.Helper.Generics
{
    [Serializable]
    public abstract class GenericEntity<TID> : IDisposable
    {
        /// <summary>
        /// ID may be of type string, int, custom type, etc.
        /// Setter is protected to allow unit tests to set this property via reflection and to allow 
        /// domain objects more flexibility in setting this for those objects with assigned IDs.
        /// </summary>
        public virtual TID Id { get; set; }

        public virtual new bool Equals(object obj)
        {
            GenericEntity<TID> compareTo = obj as GenericEntity<TID>;

            return (compareTo != null) &&
                   (HasSameNonDefaultIdAs(compareTo) ||
                // Since the IDs aren't the same, either of them must be transient to 
                // compare business value signatures
                    (((IsTransient()) || compareTo.IsTransient()) &&
                     HasSameBusinessSignatureAs(compareTo)));
        }

        /// <summary>
        /// Transient objects are not associated with an item already in storage.  For instance,
        /// a <see cref="Customer" /> is transient if its ID is 0.
        /// </summary>
        public virtual bool IsTransient()
        {
            return Id == null || Id.Equals(default(TID));
        }

        /// <summary>
        /// Must be provided to properly compare two objects
        /// </summary>
        //public virtual abstract override int GetHashCode();

        private bool HasSameBusinessSignatureAs(GenericEntity<TID> compareTo)
        {
            Check.Require(compareTo != null, "compareTo may not be null");

            return GetHashCode().Equals(compareTo.GetHashCode());
        }

        /// <summary>
        /// Returns true if self and the provided persistent object have the same ID values 
        /// and the IDs are not of the default ID value
        /// </summary>
        private bool HasSameNonDefaultIdAs(GenericEntity<TID> compareTo)
        {
            Check.Require(compareTo != null, "compareTo may not be null");

            return (Id != null && !Id.Equals(default(TID))) &&
                   (compareTo.Id != null && !compareTo.Id.Equals(default(TID))) &&
                   Id.Equals(compareTo.Id);
        }

        //private TID Id = default(TID);


        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
