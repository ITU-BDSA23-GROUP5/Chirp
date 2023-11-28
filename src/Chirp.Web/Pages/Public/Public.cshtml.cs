﻿﻿using Chirp.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
	private readonly IAuthorRepository AuthorRepository;
	private readonly ICheepRepository CheepRepository;
	private readonly IValidator<CreateCheepDTO> CheepValidator;
	public required List<CheepDTO> Cheeps { get; set; }

	public required List<AuthorDTO> Following { get; set; }

	[BindProperty]
	public string? CheepMessage { get; set; }

	public bool InvalidCheep { get; set; }
	public string? ErrorMessage { get; set; }
	public int PageNumber { get; set; }
	public int LastPageNumber { get; set; }
	public string? PageUrl { get; set; }

	public PublicModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository, IValidator<CreateCheepDTO> _cheepValidator)
	{
		AuthorRepository = authorRepository;
		CheepRepository = cheepRepository;
		CheepValidator = _cheepValidator;
	}

	public ActionResult OnGet([FromQuery(Name = "page")] int page = 1)
	{
		Cheeps = CheepRepository.GetCheeps(page);
		string name = (User.Identity?.Name) ?? throw new Exception("Name is null!");
		Following = AuthorRepository.GetFollowing(name);
		PageNumber = page;
		LastPageNumber = CheepRepository.GetPageAmount();
		PageUrl = HttpContext.Request.GetEncodedUrl().Split("?")[0];

		return Page();
	}

	public IActionResult OnPost()
	{
		InvalidCheep = false;
		//Console.WriteLine("OnPost called!");
		try
		{
			if (CheepMessage == null)
			{
				throw new Exception("Cheep is empty!");
			}

			string email = User.Claims.Where(a => a.Type == "emails").Select(e => e.Value).Single();
			string name = (User.Identity?.Name) ?? throw new Exception("Error in getting username!");

			if (AuthorRepository.GetAuthorByEmail(email).SingleOrDefault() == null)
			{
				AuthorRepository.CreateNewAuthor(name, email);
			}

			CreateCheepDTO cheep = new CreateCheepDTO()
			{
				Text = CheepMessage,
				Name = name,
				Email = email
			};

			CheepValidator.ValidateAndThrow(cheep);

			CheepRepository.CreateNewCheep(cheep);
			CheepMessage = null;
		}
		catch (Exception e)
		{
			ErrorMessage = e.Message;
			InvalidCheep = true;
		}

		return OnGet();
	}

	public IActionResult OnPostFollow(string followeeName, string followerName)
	{
		string name = (User.Identity?.Name) ?? throw new Exception("Name is null!");

		if (AuthorRepository.GetAuthorByName(name).FirstOrDefault() == null)
		{
			AuthorRepository.CreateNewAuthor(name, name + "@gmail.com");
		}

		AuthorRepository.FollowAuthor(followerName ?? throw new Exception("Name is null!"), followeeName);
		return Redirect("/");
	}

	public async Task<IActionResult> OnPostUnfollow(string followeeName, string followerName)
	{
		await AuthorRepository.UnfollowAuthor(followerName ?? throw new Exception("Name is null!"), followeeName);
		return Redirect("/");
	}
}
