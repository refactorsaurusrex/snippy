using System;

namespace Snippy.Infrastructure
{
    public class DuplicateWorkspacePackageException : Exception
    {
        private const string Error = "A duplicate WorkspacePackage was detected. All WorkspacePackages must have unique file names.";

        public DuplicateWorkspacePackageException(string packageFileName) 
            : base(Error) => PackageFileName = packageFileName;

        public DuplicateWorkspacePackageException(string message, string packageFileName) 
            : base(message) => PackageFileName = packageFileName;

        public DuplicateWorkspacePackageException(string message, Exception innerException, string packageFileName) 
            : base(message, innerException) => PackageFileName = packageFileName;

        public string PackageFileName { get; }
    }
}