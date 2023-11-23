namespace Chirp.Core;
public interface ICheepRepository
{
	public List<CheepDTO> GetCheeps(int page);
	public int GetPageAmount();
	public int GetPageAmount(string author);
	public List<CheepDTO> GetCheepsFromAuthor(int page, string author);
	public void CreateNewCheep(CreateCheepDTO createCheepDTO);

}