using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scanquire.Public
{
    /// <summary>Used to lock an SQImage to allow multiple edit operations.</summary>
    public class SQImageEditLock : IDisposable
    {
        private readonly Guid _Id;
        /// <summary>Unique identifier for this lock instance.</summary>
        public Guid Id
        { get { return _Id; } }

        private readonly SQImage _Owner;
        /// <summary>The SQImage that the lock applies to.</summary>
        public SQImage Owner
        { get { return _Owner; } }

        /// <summary>Create a new Lock agains the specified SQImage.</summary>
        /// <param name="owner"></param>
        public SQImageEditLock(SQImage owner)
        {
            _Owner = owner;
            _Id = Guid.NewGuid(); 
        }

        ~SQImageEditLock()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>Discards any pending edits and unlocks the SQImage.</summary>
        /// <param name="disposing"></param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            { Owner.DiscardEdit(this); }
        }
    }
}
