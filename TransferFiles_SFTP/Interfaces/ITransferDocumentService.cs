namespace TransferFiles_SFTP.Interfaces
{
    public interface ITransferDocumentService
    {
        public Task<object> TransferFromLocal();
    }
}
