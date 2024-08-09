using Renci.SshNet;
using TransferFiles_SFTP.Interfaces;
using TransferFiles_SFTP.Entities;

namespace TransferFiles_SFTP.Services
{
    public class TransferDocumentService : ITransferDocumentService
    {
        private string host = "host";
        private string username = "username";
        private string password = "password";
        private string remoteFilePath = "/test/file_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss_fff") + ".pdf";  // Use Unix format in the path and create an unique file name
        private string localFilePath = "C:/Test/Test.pdf"; // Local file path

        public async Task<object> TransferFromLocal()
        {
            var mainValidator = TryReadLocalFile(localFilePath);
            if (mainValidator.IsSuccess)
            {
                using (var sftp = new SftpClient(host, username, password))
                {
                    try
                    {
                        // Conect to SFTP server
                        sftp.Connect();
                        Console.WriteLine("Conected to the SFTP server.");

                        // Upload any file
                        using (var fileStream = new FileStream(localFilePath, FileMode.Open))
                        {
                            await Task.Run(() => sftp.UploadFile(fileStream, remoteFilePath));
                        }
                        Console.WriteLine("File uploaded succesfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                    finally
                    {
                        // Disconect
                        if (sftp.IsConnected)
                        {
                            sftp.Disconnect();
                            Console.WriteLine("Disconnected from the SFTP server.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(mainValidator.Message);
            }
            return mainValidator;
        }

        private Result<string> TryReadLocalFile(string localPath)
        { 
            if(string.IsNullOrEmpty(localPath) || string.IsNullOrWhiteSpace(localPath))
            {
                return Result<string>.Failure("Path is empty.");
            }

            if(!File.Exists(localPath))
            {
                return Result<string>.Failure("File does not exist.");
            }

            FileInfo fileInfo = new FileInfo(localPath);
            if (fileInfo.Length == 0)
            {
                return Result<string>.Failure("File is empty.");
            }

            // Comprobar si el archivo es accesible para lectura sin intentar abrirlo
            bool hasReadPermission = false;

            // Verificar los atributos del archivo
            if ((fileInfo.Attributes & FileAttributes.ReadOnly) == 0)
            {
                hasReadPermission = true;
            }

            if (!hasReadPermission)
            {
                return Result<string>.Failure("File is read-only or inaccessible.");
            }

            return Result<string>.Success(localPath);
        }
    }
}
