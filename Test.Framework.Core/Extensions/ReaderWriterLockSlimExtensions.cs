using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Framework.Extensions
{
    public static class ReaderWriterLockSlimExtensions
    {
        #region Sealed Classes

        private sealed class ReadLockToken : Disposable
        {
            private ReaderWriterLockSlim sync;
            public ReadLockToken(ReaderWriterLockSlim sync)
            {
                this.sync = sync;
                sync.EnterReadLock();
            }

            protected override void Dispose(bool disposing)
            {
                if (sync != null && sync.IsReadLockHeld)
                {
                    sync.ExitReadLock();
                    sync = null;
                }
                base.Dispose(disposing);
            }
        }

        private sealed class WriteLockToken : Disposable
        {
            private ReaderWriterLockSlim sync;
            public WriteLockToken(ReaderWriterLockSlim sync)
            {
                this.sync = sync;
                sync.EnterWriteLock();
            }
            protected override void Dispose(bool disposing)
            {
                if (sync != null && sync.IsWriteLockHeld)
                {
                    sync.ExitWriteLock();
                    sync = null;
                }
                base.Dispose(disposing);
            }
        }

        private sealed class UpgradeableLockToken : Disposable
        {
            private ReaderWriterLockSlim sync;
            public UpgradeableLockToken(ReaderWriterLockSlim sync)
            {
                this.sync = sync;
                sync.EnterUpgradeableReadLock();
            }
            protected override void Dispose(bool disposing)
            {
                if (sync != null && sync.IsUpgradeableReadLockHeld)
                {
                    sync.ExitUpgradeableReadLock();
                    sync = null;
                }
                base.Dispose(disposing);
            }
        }

        #endregion

        #region Extension Methods

        public static IDisposable AcquireReadLock(this ReaderWriterLockSlim rwLockSlim)
        {
            return new ReadLockToken(rwLockSlim);
        }

        public static IDisposable AcquireWriteLock(this ReaderWriterLockSlim rwLockSlim)
        {
            return new WriteLockToken(rwLockSlim);
        }

        public static IDisposable AcquireUpgradeableReadLock(this ReaderWriterLockSlim rwLockSlim)
        {
            return new UpgradeableLockToken(rwLockSlim);
        }

        #endregion
    }
}
