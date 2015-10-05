using System;
using Lucene.Net.Store;

namespace NBlast.Rest.Tests.Services.Write
{
    class FakeRamDirectory: Directory
    {
        private readonly RAMDirectory _delegateDirectory = new RAMDirectory();
        public override void Sync(string name)
        {
            _delegateDirectory.Sync(name);
        }

        public override IndexInput OpenInput(string name, int bufferSize)
        {
            return _delegateDirectory.OpenInput(name, bufferSize);
        }

        public override Lock MakeLock(string name)
        {
            return _delegateDirectory.MakeLock(name);
        }

        public override void ClearLock(string name)
        {
            _delegateDirectory.ClearLock(name);
        }

        protected override void Dispose(bool disposing)
        {
            //_delegateDirectory.Dispose();
        }

        public void DisposeIt()
        {
            _delegateDirectory.Dispose();
        }

        public override void SetLockFactory(LockFactory lockFactory)
        {
            _delegateDirectory.SetLockFactory(lockFactory);
        }

        public override string GetLockId()
        {
            return _delegateDirectory.GetLockId();
        }

        public new void EnsureOpen()
        {
            _delegateDirectory.EnsureOpen();
        }

        public override LockFactory LockFactory
        {
            get { return _delegateDirectory.LockFactory; }
        }

        public new bool isOpen_ForNUnit
        {
            get { return _delegateDirectory.isOpen_ForNUnit; }
        }

        public override string[] ListAll()
        {
            return _delegateDirectory.ListAll();
        }

        public override bool FileExists(string name)
        {
            return _delegateDirectory.FileExists(name);
        }

        public override long FileModified(string name)
        {
            return _delegateDirectory.FileModified(name);
        }

        public override void TouchFile(string name)
        {
            _delegateDirectory.TouchFile(name);
        }

        public override long FileLength(string name)
        {
            return _delegateDirectory.FileLength(name);
        }

        public long SizeInBytes()
        {
            return _delegateDirectory.SizeInBytes();
        }

        public override void DeleteFile(string name)
        {
            _delegateDirectory.DeleteFile(name);
        }

        public override IndexOutput CreateOutput(string name)
        {
            return _delegateDirectory.CreateOutput(name);
        }

        public override IndexInput OpenInput(string name)
        {
            try
            {
                return _delegateDirectory.OpenInput(name);
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}