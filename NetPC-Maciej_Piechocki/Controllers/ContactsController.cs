using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using NetPC_Maciej_Piechocki.Models;
using NetPC_Maciej_Piechocki.Services;
using System.Security.Cryptography;

namespace NetPC_Maciej_Piechocki.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext context;

        public ContactsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            // Adding Contacts to the list
            var contacts = context.Users.ToList();
            return View(contacts);
        }
        
        // Redirect to Create View
		public IActionResult Create()
		{
			return View();
		}

        // Creating and validating new contact
        [HttpPost]
		public IActionResult Create(ContactDto contactDto)
		{
            if (!ModelState.IsValid)
            {
                return View(contactDto);
            }

            Contact contact = new Contact()
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Email = contactDto.Email,
                PhoneNumber = contactDto.PhoneNumber,
                CategoryId = contactDto.CategoryId,
                SubCategoryId = contactDto.SubCategoryId,
                PasswordHash = PasswordHasher(contactDto.Password),
                BirthDate = contactDto.BirthDate,
            };

            context.Users.Add(contact);
            context.SaveChanges();

			return RedirectToAction("Index", "Contacts");
		}

        // Redirecting to Edit View
		public IActionResult Edit(string id)
        {
            var contact = context.Users.Find(id);

            if (contact == null)
            {
                return RedirectToAction("Index", "Contacts");
            }
            var contactDto = new ContactDto()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                BirthDate = contact.BirthDate,
                CategoryId = contact.CategoryId,
				SubCategoryId = contact.SubCategoryId,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber
            };

            ViewData["ContactId"] = contact.Id;

            return View(contactDto);
        }

        // Editing the contact - something doesn't work
        [HttpPost]
        public IActionResult Edit(string id, ContactDto contactDto)
        {
            var contact = context.Users.Find(id);

            if (contact == null)
            {
                return RedirectToAction("Index", "Contacts");
            }

            if (!ModelState.IsValid)
            {
				ViewData["ContactId"] = contact.Id;

				return View(contactDto);
            }

            contact.FirstName = contactDto.FirstName;
            contact.LastName = contactDto.LastName;
            contact.PhoneNumber = contactDto.PhoneNumber;
            contact.Email = contactDto.Email;
            contact.BirthDate = contactDto.BirthDate;
            contact.CategoryId = contactDto.CategoryId;
            contact.SubCategoryId = contactDto.SubCategoryId;

			context.SaveChanges();

            return RedirectToAction("Index", "Contacts");
        }

        // Deleting the contact
        public IActionResult Delete(string id)
        {
            var contact = context.Users.Find(id);

            if (contact == null)
            {
                return RedirectToAction("Index", "Contacts");
            }

            context.Users.Remove(contact);
            context.SaveChanges();

            return RedirectToAction("Index", "Contacts");
        }

        protected string PasswordHasher(string password)
        {
			// Generate a 128-bit salt using a sequence of
			// cryptographically strong random bytes.
			byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
			Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

			// derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
			string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: password!,
				salt: salt,
				prf: KeyDerivationPrf.HMACSHA256,
				iterationCount: 100000,
				numBytesRequested: 256 / 8));

            return hashed;
		}
    }
}
