public class CreateCheepDTO
{
	public Guid CheepGuid { get; set; }
	public required string Text { get; set; }
	public required int AuthorId { get; set; }
	public required string Name { get; set; }
	public required string Email { get; set; }
}