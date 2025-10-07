namespace VendorCctvAPI.Models
{
    public class ResponseBase
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }

        public string DataSavedMessage = "Data has been saved successfully.";
        public string DataUpdatedMessage = "Data has been updated successfully.";
        public string DataDeletedMessage = "Data has been deleted successfully.";
        public string DataSaveFailedMessage = "Failed to save data.";
        public string DataUpdateFailedMessage = "Failed to update data.";
        public string DataDeleteFailedMessage = "Failed to delete data.";
        public string InternalServerErrorMessage = "Internal server error.";
        public string DataFoundMessage = "Data Found.";
        public string DataNotFoundMessage = "Data Not Found.";

        public ResponseBase ToDataSaved(object? data = null) => new ResponseBase
        {
            Status = true,
            Message = DataSavedMessage,
            Data = data
        };

        public ResponseBase ToDataUpdated(object? data = null) => new ResponseBase
        {
            Status = true,
            Message = DataUpdatedMessage,
            Data = data
        };

        public ResponseBase ToDataDeleted(object? data = null) => new ResponseBase
        {
            Status = true,
            Message = DataDeletedMessage,
            Data = data
        };

        public ResponseBase ToDataSaveFailed(string m = "", object? data = null) => new ResponseBase
        {
            Status = false,
            Message = $"{DataSaveFailedMessage} {m}".Trim(),
            Data = data
        };

        public ResponseBase ToDataUpdateFailed(string m = "", object? data = null) => new ResponseBase
        {
            Status = false,
            Message = $"{DataUpdateFailedMessage} {m}".Trim(),
            Data = data
        };

        public ResponseBase ToDataDeleteFailed(string m = "", object? data = null) => new ResponseBase
        {
            Status = false,
            Message = $"{DataDeleteFailedMessage} {m}".Trim(),
            Data = data
        };

        public ResponseBase ToInternalServerError(string m = "", object? data = null) => new ResponseBase
        {
            Status = false,
            Message = $"{InternalServerErrorMessage} {m}".Trim(),
            Data = data
        };

        public ResponseBase ToDataFound(object? data = null) => new ResponseBase
        {
            Status = true,
            Message = DataFoundMessage,
            Data = data
        };

        public ResponseBase ToDataNotFound(object? data = null) => new ResponseBase
        {
            Status = false,
            Message = DataNotFoundMessage,
            Data = data
        };

        public ResponseBase ToSuccessInfoMessage(string m = "", object? data = null) => new ResponseBase
        {
            Status = true,
            Message = $"{m}".Trim(),
            Data = data
        };

        public ResponseBase ToErrorInfoMessage(string m = "", object? data = null) => new ResponseBase
        {
            Status = false,
            Message = $"{m}".Trim(),
            Data = data
        };

        public enum StatusEnum
        {
            Error,
            Success
        }
    }

}
