﻿namespace GuiSO_GUI.Services;
using System.IO;

public static class FileSystemService
{
    public static bool DirectoryExists(string dirPath)
    {
        var dir = new DirectoryInfo(dirPath);
        return dir.Exists;
    }

    public static bool MakeDirectory(string dirPath)
    {
        DirectoryInfo dir = Directory.CreateDirectory(dirPath);
        return dir.Exists;
    }
    
    public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {
        // Get information about the source directory
        var dir = new DirectoryInfo(sourceDir);

        // Check if the source directory exists
        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        // Cache directories before we start copying
        DirectoryInfo[] dirs = dir.GetDirectories();

        // Create the destination directory
        Directory.CreateDirectory(destinationDir);

        // Get the files in the source directory and copy to the destination directory
        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(destinationDir, file.Name);
            try
            {
                file.CopyTo(targetFilePath, true);
            }
            catch (Exception e)
            {
                Shell.Current.DisplayAlert("Error al copiar archivo", e.Message, "OK");
                Console.WriteLine(e);
            }
        }

        // If recursive and copying subdirectories, recursively call this method
        if (recursive)
        {
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir, true);
            }
        }
    }
}