using Microsoft.AspNetCore.Http;

namespace Application.Common;

public class BaseResponse<T> where T : class
{

    public required string Message { get; set; }
    public required bool Success { get; set; }
    public int? Code { get; set; } = StatusCodes.Status200OK;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public Dictionary<string, string[]>? FieldErrors { get; set; }
}

public class BaseResponseModel
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    //public string? CreatedBy { get; set; }
    //public DateTime Created { get; set; }
}