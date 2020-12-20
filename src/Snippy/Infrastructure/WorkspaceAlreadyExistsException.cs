using System;

namespace Snippy.Infrastructure
{
    public class WorkspaceAlreadyExistsException : Exception
    {
        private const string Error = "Workspace file with the specified name already exists.";

        public WorkspaceAlreadyExistsException(string workspaceFilePath) 
            : base($"{Error}: {workspaceFilePath}") => WorkspaceFilePath = workspaceFilePath;

        public WorkspaceAlreadyExistsException(string message, string workspaceFilePath) : 
            base(message) => WorkspaceFilePath = workspaceFilePath;

        public WorkspaceAlreadyExistsException(string message, Exception innerException, string workspaceFilePath) : 
            base(message, innerException) => WorkspaceFilePath = workspaceFilePath;

        public string WorkspaceFilePath { get; }
    }
}