namespace MonPDReborn.Lib.General
{
    public class ResponseBase
    {
        public StatusEnum Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Tag { get; set; } = new();


        public string DataSavedMessage = "Data has been saved successfully.";
        public string DataUpdatedMessage = "Data has been updated successfully.";
        public string DataDeletedMessage = "Data has been deleted successfully.";
        public string DataSaveFailedMessage = $"Failed to save data.";
        public string DataUpdateFailedMessage = $"Failed to update data.";
        public string DataDeleteFailedMessage = $"Failed to delete data.";
        public string InternalServerErrorMessage = $"Internal server error.";
        public string DataFoundMessage = $"Data Found.";
        public string DataNotFoundMessage = $"Data Not Found.";


        public ResponseBase ToDataSaved() => new ResponseBase
        {
            Status = StatusEnum.Success,
            Message = DataSavedMessage
        };

        public ResponseBase ToDataUpdated() => new ResponseBase
        {
            Status = StatusEnum.Success,
            Message = DataUpdatedMessage
        };

        public ResponseBase ToDataDeleted() => new ResponseBase
        {
            Status = StatusEnum.Success,
            Message = DataDeletedMessage
        };

        public ResponseBase ToDataSaveFailed(string m = "") => new ResponseBase
        {
            Status = StatusEnum.Error,
            Message = $"{DataSaveFailedMessage} {m}".Trim()
        };

        public ResponseBase ToDataUpdateFailed(string m = "") => new ResponseBase
        {
            Status = StatusEnum.Error,
            Message = $"{DataUpdateFailedMessage} {m}".Trim()
        };

        public ResponseBase ToDataDeleteFailed(string m = "") => new ResponseBase
        {
            Status = StatusEnum.Error,
            Message = $"{DataDeleteFailedMessage} {m}".Trim()
        };

        public ResponseBase ToInternalServerError(string m = "") => new ResponseBase
        {
            Status = StatusEnum.Error,
            Message = $"{InternalServerErrorMessage} {m}".Trim()
        };

        public ResponseBase ToDataFound(string m = "") => new ResponseBase
        {
            Status = StatusEnum.Error,
            Message = $"{DataFoundMessage} {m}".Trim()
        };

        public ResponseBase ToDataNotFound(string m = "") => new ResponseBase
        {
            Status = StatusEnum.Error,
            Message = $"{DataNotFoundMessage} {m}".Trim()
        };

        public ResponseBase ToSuccessInfoMessage(string m = "") => new ResponseBase
        {
            Status = StatusEnum.Success,
            Message = $"Info. {m}".Trim()
        };

        public ResponseBase ToErrorInfoMessage(string m = "") => new ResponseBase
        {
            Status = StatusEnum.Error,
            Message = $"Info. {m}".Trim()
        };

        public enum StatusEnum
        {
            Error,
            Success
        }
    }
}
