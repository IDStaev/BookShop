using System.Text;
using BookShop.Common;
using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

[Route("[controller]/[action]")]
public class BookController : Controller
{
    private readonly UserRepository _userRepository;
    private readonly BookRepository _bookRepository;

    public BookController(UserRepository userRepository, BookRepository bookRepository)
    {
        _userRepository = userRepository;
        _bookRepository = bookRepository;
    }
    
    [HttpGet]
    public IActionResult CreateBook()
    {
        if (CurrentUser.CurrentUserId == null)
        {
            return RedirectToAction("Index", "Home");
        }
        
        var model = new CreateBookViewModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult CreateBook(CreateBookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.Image != null &&
            model.Image.ContentType != "image/png" &&
            model.Image.ContentType != "image/jpeg")
        {
            ModelState.AddModelError(nameof(model.Image), "File must be in .png or in .jpeg format.");
            return View(model);
        }
        
        if (CurrentUser.CurrentUserId == null)
        {
            return NotFound();
        }

        var user = _userRepository.GetById((Guid)CurrentUser.CurrentUserId);

        if (user == null)
        {
            return NotFound();
        }

        var imageBase64 = string.Empty;
        
        if (model.Image != null)
        {
            using var memoryStream = new MemoryStream();
            model.Image.CopyTo(memoryStream);
            var bytes = memoryStream.ToArray();
            imageBase64 = Convert.ToBase64String(bytes);
        }
        
        var book = new Book
        {
            Name = model.Name,
            Content = model.Content,
            IsPublic = model.IsPublic,
            User = user,
            ImageBase64 = imageBase64
        };

        _bookRepository.Create(book);
        
        return RedirectToAction("MyBooks");
    }

    [HttpGet]
    public IActionResult MyBooks()
    {
        if (CurrentUser.CurrentUserId == null)
        {
            return RedirectToAction("Index", "Home");
        }
        
        var books = _bookRepository.GetAllFromUser((Guid)CurrentUser.CurrentUserId);

        var model = books.Select(b => new MyBookViewModel
        {
            Id = b.Id,
            Name = b.Name,
            IsPublic = b.IsPublic,
            Content = b.Content,
            ImageBase64 = b.ImageBase64
                // .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                // .Aggregate(new StringBuilder(), (sb, word) =>
                // {
                //     if (sb.Length + word.Length > 100)
                //     {
                //         sb.AppendLine();
                //     }
                //
                //     sb.Append(word).Append(' ');
                //     return sb;
                // })
                // .ToString()
                // .Trim()
        }).ToList();

        return View(model);
    }

    [HttpGet]
    public IActionResult PublicBooks()
    {
        if (CurrentUser.CurrentUserId == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var books = _bookRepository.GetPublic();

        var model = books.Select(b => new PublicBookViewModel
        {
            Name = b.Name,
            Content = b.Content,
            Author = $"{b.User.FirstName} {b.User.LastName}",
            ImageBase64 = b.ImageBase64
        }).ToList();

        return View(model);
    }

    [HttpGet("{id:guid}")]
    public IActionResult UpdateBook([FromRoute] Guid id)
    {
        if (CurrentUser.CurrentUserId == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var book = _bookRepository.GetById(id);

        if (book == null)
        {
            return NotFound();
        }

        var model = new UpdateBookViewModel
        {
            Name = book.Name,
            Content = book.Content,
            IsPublic = book.IsPublic
        };

        return View(model);
    }

    [HttpPost("{id:guid}")]
    public IActionResult UpdateBook([FromRoute] Guid id, UpdateBookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (CurrentUser.CurrentUserId == null)
        {
            return NotFound();
        }

        var book = _bookRepository.GetById(id);

        if (book == null)
        {
            return NotFound();
        }
        
        book.Name = model.Name;
        book.Content = model.Content;
        book.IsPublic = model.IsPublic;

        _bookRepository.Update(book);

        return RedirectToAction("MyBooks");
    }

    [HttpPost("{id:guid}")]
    public IActionResult DeleteBook([FromRoute] Guid id)
    {
        if (CurrentUser.CurrentUserId == null)
        {
            return NotFound();
        }

        var book = _bookRepository.GetById(id);

        if (book == null)
        {
            return NotFound();
        }
        
        _bookRepository.Delete(book);

        return RedirectToAction("MyBooks");
    }
}