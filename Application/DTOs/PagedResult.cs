namespace Application.DTOs
{
  public record PagedResult<T>(ICollection<T> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext);
}
