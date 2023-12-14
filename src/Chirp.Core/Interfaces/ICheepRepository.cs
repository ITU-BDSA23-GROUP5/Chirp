namespace Chirp.Core;
public interface ICheepRepository
{
	public List<CheepDTO> GetCheeps(int page);
	public int GetPageAmount();
	public int GetPageAmount(string author);
	public List<CheepDTO> GetCheepsFromAuthor(int page, string author);
	public List<CheepDTO> GetCheepsFromAuthor(string author);
	public void CreateNewCheep(CreateCheepDTO createCheepDTO);
	public void LikeCheep(Guid cheepId, string author);
	public void UnlikeCheep(Guid CheepId, string author);
	public List<CheepDTO> GetCheepsFromAuthorAndFollowings(int page, string author, List<String> following);
	public List<CheepDTO> GetMostLikedCheeps(int page);
}