/*
 
This is a 2 week assignment/mini project. Goal is to develop a robust and professional Compression application. We use this type of tools q
 * uite a lot in games as one of the main processing
 * engines in asset pipeline. So it is important that it is designed properly and and is console based, not windows and GUI. This way you
 * setup the framework and implement some of the 
 * simpler functionalities. Next week we continue and implement the rest. 
It is very important that do the following design part as it is outlined and with the same steps:

1) The application is called "Compressionator". It is the main tool for compressing / uncompressing / updating and archiving files and 
 * folders. 
     --It is used for compressing a whole folder and all its subfolders. We should be able to provide a filter so for example only do
 * jpg files.
     --It is used for uncompressing a zip fille. We should be able to provide an optional destination folder where the uncompressed files
 * and folders will go. We should also be able to provide 
 * an optional filter, so for example can extract all *.exe files.
    -- It is used for updating a zip file. This means we can add more files to it, or delete certain type of files to it, based on a filter 
 * we provide.

    -- We should be able to provide compression algorithm, means being able to choose between a few algorithms  Zip, gZip. Based on what 
 * algorithms we choose the output file will have different 
 * extension.

  -- We should be able to provide option for logging file. This means if we provide, for example, commandline options -log comLogger.txt , 
 * this means all logging outputs should be directed to this
 * file, instead of being printed to the console by default. We also want to provide optional command line flag  from this list 
 * ( -verbose -warning -error ). We can only choose one of these flags. 
 * By default -error is selected. THis flag specifies how much info we should output to screen or to the log file. For -verbose every 
 * action meaning every file being compressed or decompressed or 
 * updated is logged first, meaning a line is output saying what is happening to what file. 
-- Compressionator should NEVER crash or stop! It means if it encounters errors it should just print a log message and continue. Error 
 * ould mean serious issue for example we can not write current 
 * location, or a folder is not readable. Warning means less serious issues and so on.

2) You should first design the application framework by deciding what command line options you have, and setup the code to handle each 
 * case. For every option there should be a default if possible,
 * so if user doesn't set that option, we choose the default. For example for compression algorithm we should default to Zip format which 
 * we started to study in class. Do some search online to get
 * an idea how similar tools handle various options on command line.  For example there is actually a tool named "compressionator" by 
 * IBM. But this is a bit more advanced. Here's the link: 
 * http://www14.software.ibm.com/webapp/set2/sas/f/comprestimator/home.html

3) Use the link I provided in class and we started implementing the code from as your stating point: Here's the link again bellow. Our 
 * compressionator should cover all the options in this link and more:
https://msdn.microsoft.com/en-us/library/ms404280(v=vs.110).aspx

Again, this is a 2 week project and I will provide more advanced stuff next week to add to your application. This week is all about 
 * designing the interface, implementing the framework, and 
 * implementing the part I provided for Zip format as explained in part 3) above. 
You must create a GItHub project to track your progress and update to it daily as you do more. I expect to see some design document 
 * like text file ReadMe.txt to explain the application and how 
 * to use it, as well as some UML drawing if possible to show graphically the flow of execution based on user's command line input. 
 * Of course all the project file. 
This project is in C Sharp.
You must send me the link to your GitHub project by next class so we can have a look in class together see where you are.

 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace zippingExtras
{
    /*class Program
    {
        //https://msdn.microsoft.com/en-us/library/ms404280(v=vs.110).aspx
        //https://www.codeproject.com/Articles/381661/Creating-Zip-Files-Easily-in-NET
    }*/

    class ZipFileCompress
    {
        public static void simpleZip(string dirToZip, string zipName)
        {
            ZipFile.CreateFromDirectory( dirToZip, zipName );
        }

        /*static void simpleZip(string dirToZip, string zipName, CompressionLevel compression, bool includeRoot)
        {
            ZipFile.CreateFromDirectory( dirToZip, zipName, compression, includeRoot );
        }*/

        public static void updateZipFile(string zipName, string sourceFile, string entryName)
        {
            using(ZipArchive modFile = ZipFile.Open(zipName, ZipArchiveMode.Update))
            {
                modFile.CreateEntryFromFile( sourceFile, entryName ); //if sourceFile doesn't exist = fail. Create first. 
                                                                      //names changed upon zip
                //modFile.CreateEntryFromFile(sourceFile, "name.whatever", CompressionLevel.Fastest);
            }
        }

        public static void simpleUnzip(string zipName, string dirToUnzipTo)
        {
            ZipFile.ExtractToDirectory(zipName, dirToUnzipTo);
        }

        public static void smarterUnzip(string zipName, string dirToUnzipTo)
        {
            string fileUnzipFullPath;
            string fileUnzipFullName;

            using(ZipArchive archive = ZipFile.OpenRead(zipName))
            {
                foreach(ZipArchiveEntry file in archive.Entries)
                {
                    Console.WriteLine( "File Name: {0}", file.Name );
                    Console.WriteLine( "File Size: {0} bytes", file.Length );
                    Console.WriteLine( "Compression Ratio: {0}", ((double)file.CompressedLength / file.Length).ToString( "0.0%" ) );

                    fileUnzipFullName = Path.Combine( dirToUnzipTo, file.FullName );

                    if(!System.IO.File.Exists(fileUnzipFullName))
                    {
                        fileUnzipFullPath = Path.GetDirectoryName( fileUnzipFullName );

                        Directory.CreateDirectory( fileUnzipFullPath );

                        file.ExtractToFile( fileUnzipFullName );
                    }
                }
            }
        }

        public enum Overwrite
        {
            Always, IfNewer, Never
        }

        public enum ArchiveAction
        {
            Merge, Replace, Error, Ignore
        }

        public static void improvedExtractToFile(ZipArchiveEntry file, string destinationPath, Overwrite  overwriteMethod = Overwrite.IfNewer)
        {
            string destinationFileName = Path.Combine( destinationPath, file.FullName );
            string destinationFilePath = Path.GetDirectoryName( destinationFileName );

            Directory.CreateDirectory( destinationFilePath );

            switch(overwriteMethod)
            {
                case Overwrite.Always:
                file.ExtractToFile( destinationFileName, true );
                break;

                case Overwrite.IfNewer:
                if(!File.Exists(destinationFileName) || File.GetLastWriteTime(destinationFileName) < file.LastWriteTime)
                {
                    file.ExtractToFile( destinationFileName, true );
                }
                break;

                case Overwrite.Never:
                if(!File.Exists(destinationFileName))
                {
                    file.ExtractToFile( destinationFileName );
                }
                break;

                default:
                break;
            }
        }

        public static void AddToArchive(string archiveFullName, List<string> files, ArchiveAction action = ArchiveAction.Replace, 
                            Overwrite fileOverwrite = Overwrite.IfNewer, CompressionLevel compression = CompressionLevel.Optimal)
        {
            ZipArchiveMode mode = ZipArchiveMode.Create;

            bool archiveExists = File.Exists( archiveFullName );

            switch(action)
            {
                case ArchiveAction.Merge:
                if(archiveExists)
                {
                    mode = ZipArchiveMode.Update;
                }
                break;
                case ArchiveAction.Replace:
                if(archiveExists)
                {
                    File.Delete( archiveFullName );
                }
                break;
                case ArchiveAction.Error:
                if(archiveExists)
                {
                    throw new IOException( String.Format( "The zip file {0} already exists.", archiveFullName ) );
                }
                break;
                case ArchiveAction.Ignore:
                if(archiveExists)
                {
                    return;
                }
                break;
                default:
                break;
            }

            using(ZipArchive zipFile = ZipFile.Open(archiveFullName, mode))
            {
                if(mode == ZipArchiveMode.Create)
                {
                    foreach(string file in files)
                    {
                        zipFile.CreateEntryFromFile( file, Path.GetFileName( file ), compression );
                    }
                }

                else
                {
                    foreach(string file in files)
                    {
                        var fileInZip = (from f in zipFile.Entries where f.Name == Path.GetFileName(file) select f).FirstOrDefault();

                        switch(fileOverwrite)
                        {
                            case Overwrite.Always:
                            if(fileInZip != null)
                            {
                                fileInZip.Delete();
                            }

                            zipFile.CreateEntryFromFile(file, Path.GetFileName(file), compression);
                            break;
                            case Overwrite.IfNewer:
                            if(fileInZip != null)
                            {
                                if(fileInZip.LastWriteTime < File.GetLastWriteTime(file))
                                {
                                    fileInZip.Delete();
                                    zipFile.CreateEntryFromFile(file, Path.GetFileName(file), compression);
                                }
                            }

                            else
                            {
                                zipFile.CreateEntryFromFile(file, Path.GetFileName(file), compression);
                            }
                            break;
                            case Overwrite.Never:
                            if(fileInZip != null)
                            {
                                zipFile.CreateEntryFromFile(file, Path.GetFileName(file), compression);
                            }
                            break;
                            default:
                            break;
                        }
                    }
                }
            }
        }
    }

    public class GZipCompress
    {
        public static void dirCompress(DirectoryInfo directoryselected, string filter = ".txt")
        {
            foreach(FileInfo fileToCompress in directoryselected.GetFiles())
            {
                using(FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) 
                    != FileAttributes.Hidden && fileToCompress.Extension != filter)
                    {
                        using(FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                        {
                            using(GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);
                            }
                        }
                        FileInfo info = new FileInfo(directoryselected.FullName + "\\" + fileToCompress.Name + "gz");
                        Console.WriteLine("Compressed {0} from {1} to {2} bytes.", fileToCompress.Name, 
                            fileToCompress.Length.ToString(), info.Length.ToString());
                    }
                }
            }
        }

        public static void dirDecompress(FileInfo fileToDecompress)
        {
            using(FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove( currentFileName.Length - fileToDecompress.Extension.Length );

                using(FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using(GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo( decompressedFileStream );
                        Console.WriteLine( "Decompressed: {0}", fileToDecompress.Name );
                    }
                }
            }
        }

        
    }
    class program
    {
        static void Main( string[] args )
        {
            //dirCompress( directorySelected );

            string startPath = @"F:\AIStuff\GameToolsPipelines\Compression\Compression\CompressThis";
            string zipPath = @"F:\AIStuff\GameToolsPipelines\Compression\Compression\CompressThis.zip";
            string extractPath = @"F:\AIStuff\GameToolsPipelines\Compression\Compression\CompressThisUnzipped";
            string tempFolder = @"F:\AIStuff\GameToolsPipelines\Compression\Compression\Compression\temp";
            string filter;
            string userPath;
            string userChoice;
            DirectoryInfo directorySelected = new DirectoryInfo( startPath );

            Console.WriteLine( "Do you want to zip, gzip, decompress zip folder, or decompress gzip folder?" );
            Console.WriteLine( "Enter z for zip, g for gzip, dz to decompress zip, or dgz to decompress gzip:" );
            userChoice = Console.ReadLine();

            if(userChoice == "z")
            {
                Console.WriteLine( "Enter filter. Ex: .jpg, .txt, .pdf, etc. Enter all for everything: " );
                filter = Console.ReadLine();

                if(filter != "all")
                {
                    if( !System.IO.Directory.Exists( tempFolder ) )
                        Directory.CreateDirectory( tempFolder );

                    foreach( var file in Directory.GetFiles( startPath ) )
                    {
                        File.Copy( file, Path.Combine( tempFolder, Path.GetFileName( file ) ), true );
                    }

                    ZipFileCompress.simpleZip( tempFolder, zipPath );
                }

                else
                    ZipFileCompress.simpleZip( startPath, zipPath );
            }

            else if(userChoice == "g")
            {
                Console.WriteLine( "Enter filter. Ex: .jpg, .txt, .pdf, etc. Enter all for everything: " );
                filter = Console.ReadLine();

                if( filter != "all" )
                {
                    if( !System.IO.Directory.Exists( tempFolder ) )
                        Directory.CreateDirectory( tempFolder );

                    foreach( var file in Directory.GetFiles( startPath ) )
                    {
                        File.Copy( file, Path.Combine( tempFolder, Path.GetFileName( file ) ), true );
                    }

                    DirectoryInfo newFolder = new DirectoryInfo( tempFolder );
                    GZipCompress.dirCompress( newFolder );
                }

                else
                    GZipCompress.dirCompress( directorySelected );
            }

            else if(userChoice == "dz")
            {
                Console.WriteLine( "Enter path for decompression, or d for default path: " );
                userPath = Console.ReadLine();

                Console.WriteLine( "Enter filter.Ex: .jpg, .txt, .pdf, etc. Enter all for everything: " );
                filter = Console.ReadLine();

                if( !System.IO.Directory.Exists( extractPath ) )
                    Directory.CreateDirectory( extractPath );

                if( userPath != "d" )
                {
                    if( !System.IO.Directory.Exists( userPath ) )
                        Directory.CreateDirectory( userPath );

                    if( filter != "all" )
                    {
                        using( ZipArchive archive = ZipFile.OpenRead( zipPath ) )
                        {
                            foreach( ZipArchiveEntry entry in archive.Entries )
                            {
                                if( entry.FullName.EndsWith( filter, StringComparison.OrdinalIgnoreCase ) )
                                {
                                    entry.ExtractToFile( Path.Combine( userPath, entry.FullName ), true );
                                }
                            }
                        }
                    }

                    else
                    {
                        if( filter != "all" )
                        {
                            using( ZipArchive archive = ZipFile.OpenRead( zipPath ) )
                            {
                                foreach( ZipArchiveEntry entry in archive.Entries )
                                {
                                    if( entry.FullName.EndsWith( filter, StringComparison.OrdinalIgnoreCase ) )
                                    {
                                        entry.ExtractToFile( Path.Combine( extractPath, entry.FullName ), true );
                                    }
                                }
                            }
                        }

                        else
                        {
                            using( ZipArchive archive = ZipFile.OpenRead( zipPath ) )
                            {
                                foreach( ZipArchiveEntry entry in archive.Entries )
                                {
                                    ZipFile.ExtractToDirectory( zipPath, extractPath );
                                }
                            }
                            
                        }
                    }
                }
            }

            else if(userChoice == "dgz")
            {
                foreach( FileInfo fileToDecompress in directorySelected.GetFiles( "*.gz" ) )
                {
                    GZipCompress.dirDecompress( fileToDecompress );
                }
            } 
        }
    }
}