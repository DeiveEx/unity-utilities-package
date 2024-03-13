using System;
using System.Collections.Generic;
using System.IO;

namespace DeiveEx.Utilities
{
    public static class FileUtility
    {
        public static IList<string> GetFilePathsRecursive(string startPath, Func<string, bool> isFileValid)
        {
            List<string> paths = new();

            if (Directory.Exists(startPath))
            {
                var files = Directory.GetFiles(startPath);

                foreach (var filePath in files)
                {
                    if(isFileValid(filePath))
                        paths.Add(filePath);
                }
				
                var directories = Directory.GetDirectories(startPath);

                
                foreach (var directoryPath in directories)
                {
                    paths.AddRange(GetFilePathsRecursive(directoryPath, isFileValid));
                }
            }
			
            return paths;
        }
        
        public static IList<string> GetFilesWithExtensionRecursive(string startPath, string fileExtension)
        {
            return GetFilePathsRecursive(startPath, filePath => Path.GetExtension(filePath) == fileExtension);
        }
        
        public static IList<string> GetFilesWithNameRecursive(string startPath, string fileName)
        {
            return GetFilePathsRecursive(startPath, filePath => Path.GetFileName(filePath) == fileName);
        }
    }
}
