using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ATGHR.Code;
using ATGHR.Models.Database;
using ATGHR.Models.Dtos;
using ATGHR.Models.EntityFramework.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SO = System.IO.File;

namespace ATGHR.Controllers
{
	public class ATGProcessesController : Controller
	{
		private readonly string[] _permittedDocExtensions = { ".pdf", ".doc", ".docx", ".xlsx", ".dotx", ".pptx" };
		private readonly long _docFileSizeLimit;
		private readonly ApplicationDbContext _context;

		public ATGProcessesController(ApplicationDbContext context)
		{
			_context = context;
			_docFileSizeLimit = 20485760; // 2Mb
		}

		public async Task<IActionResult> Index()
		{
			var model = new ResourcesFileListDto
			{
				Files = await _context.CommonFiles
					.OrderByDescending(x => x.AddedDate)
					.ToListAsync(),
				UserI = Util.GetUser(HttpContext)
			};

			return View(model);
		}

		public async Task<IActionResult> StrategicPlan()
		{
			var model = new ResourcesFileListDto
			{
				Files = await _context.CommonFiles
					.OrderByDescending(x => x.AddedDate)
					.ToListAsync(),
				UserI = Util.GetUser(HttpContext)
			};

			return View(model);
		}

		public async Task<IActionResult> AnnualBusinessPlan()
		{
			var model = new ResourcesFileListDto
			{
				Files = await _context.CommonFiles
					.OrderByDescending(x => x.AddedDate)
					.ToListAsync(),
				UserI = Util.GetUser(HttpContext)
			};

			return View(model);
		}

		public async Task<IActionResult> BusinessDevelopment()
		{
			var model = new ResourcesFileListDto
			{
				Files = await _context.CommonFiles
					.OrderByDescending(x => x.AddedDate)
					.ToListAsync(),
				UserI = Util.GetUser(HttpContext)
			};

			return View(model);
		}

		public async Task<IActionResult> Contracts()
		{
			var model = new ResourcesFileListDto
			{
				Files = await _context.CommonFiles
					.OrderByDescending(x => x.AddedDate)
					.ToListAsync(),
				UserI = Util.GetUser(HttpContext)
			};

			return View(model);
		}

		public async Task<IActionResult> ProjectExecution()
		{
			var model = new ResourcesFileListDto
			{
				Files = await _context.CommonFiles
					.OrderByDescending(x => x.AddedDate)
					.ToListAsync(),
				UserI = Util.GetUser(HttpContext)
			};

			return View(model);
		}

		public async Task<IActionResult> Resources()
		{
			var model = new ResourcesFileListDto
			{
				Files = await _context.CommonFiles
					.OrderByDescending(x => x.AddedDate)
					.ToListAsync(),
				UserI = Util.GetUser(HttpContext)
			};

			return View(model);
		}

		public async Task<IActionResult> SOPs()
		{
			var model = new ResourcesFileListDto
			{
				Files = await _context.CommonFiles
					.OrderByDescending(x => x.AddedDate)
					.ToListAsync(),
				UserI = Util.GetUser(HttpContext)
			};

			return View(model);
		}

		public async Task<IActionResult> TxDOTContract()
		{
			var model = new ResourcesFileListDto
			{
				Files = await _context.CommonFiles
					.OrderByDescending(x => x.AddedDate)
					.ToListAsync(),
				UserI = Util.GetUser(HttpContext)
			};

			return View(model);
		}

		public async Task<IActionResult> StandardContract()
		{
			var model = new ResourcesFileListDto
			{
				Files = await _context.CommonFiles
					.OrderByDescending(x => x.AddedDate)
					.ToListAsync(),
				UserI = Util.GetUser(HttpContext)
			};

			return View(model);
		}

		public async Task<IActionResult> ProjectCloseout()
		{
			var model = new ResourcesFileListDto
			{
				Files = await _context.CommonFiles
					.OrderByDescending(x => x.AddedDate)
					.ToListAsync(),
				UserI = Util.GetUser(HttpContext)
			};

			return View(model);
		}

		public IActionResult RecordsRetention()
		{
			return View();
		}

		public IActionResult UploadFile(Guid fileId, string returnUrl)
		{
			var model = new CommonFileDto { IsFromPost = false, ReturnUrl = returnUrl };
			var commonFile = _context.CommonFiles.FirstOrDefault(x => x.Id == fileId);
			if (commonFile == null)
			{
				model.IsError = true;
				model.ErrorDescription = "Requested file not found";

				return View(model);
			}

			model.CommonFile = commonFile;

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> UploadFile(CommonFileDto model)
		{
			model.IsFromPost = true;

			var commonFile = await _context.CommonFiles.FirstOrDefaultAsync(x => x.Id == model.CommonFile.Id);
			if (commonFile == null)
			{
				model.IsError = true;
				model.ErrorDescription = "File not found";

				return View(model);
			}

			if (string.IsNullOrWhiteSpace(model.CommonFile.Title))
			{
				model.IsError = true;
				model.ErrorDescription = "Please give a Title";

				return View(model);
			}

			commonFile.Title = model.CommonFile.Title;
			commonFile.Description = model.CommonFile.Description;
			commonFile.SopCategory = model.CommonFile.SopCategory;
			commonFile.AddedDate = DateTime.UtcNow;

			var file = Request.Form.Files.FirstOrDefault();

			if (file == null || string.IsNullOrWhiteSpace(file.FileName))
			{
				if (string.IsNullOrEmpty(commonFile.FileName)) // error if no file included before
				{
					model.IsError = true;
					model.ErrorDescription = "File is empty";

					return View(model);
				}

				// a file included before, update only oder fileds
				await _context.SaveChangesAsync();

				model.IsError = false;

				return View(model);
			}

			var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
			if (string.IsNullOrEmpty(ext))
			{
				model.IsError = true;
				model.ErrorDescription = "File is empty.";

				return View(model);
			}

			if (!_permittedDocExtensions.Contains(ext))
			{
				model.IsError = true;
				model.ErrorDescription = "Not a valid file format. Valid files: .docx, .doc, .pdf, .xlsx, .dotx, .pptx";
				return View(model);
			}

			var uniqueFileName = commonFile.Id + ext;

			// delete old file
			string filePath = Path.Combine($"wwwroot/documents/commonfiles", commonFile.FileName ?? "");
			if (SO.Exists(filePath))
			{
				SO.Delete(filePath);
			}

			filePath = Path.Combine($"wwwroot/documents/commonfiles", uniqueFileName);
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}

			commonFile.Extension = ext;
			commonFile.FileName = uniqueFileName;
			commonFile.OriginalFileName = file?.FileName;

			await _context.SaveChangesAsync();

			model.IsError = false;

			return View(model);
		}
	}
}
