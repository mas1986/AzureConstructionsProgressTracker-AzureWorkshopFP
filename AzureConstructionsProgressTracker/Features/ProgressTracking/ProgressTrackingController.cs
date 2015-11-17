using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AzureConstructionsProgressTracker.Common;
using Common;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace AzureConstructionsProgressTracker.Features.ProgressTracking
{
    public class ProgressTrackingController : Controller
    {
        private ConstructionsProgressTrackerContext _db = new ConstructionsProgressTrackerContext();
        private readonly FilesStorageService _filesStorageService;
        private readonly ServiceBusManager _serviceBusManager = new ServiceBusManager(ConfigurationManager.ConnectionStrings["AzureWebJobsServiceBus"].ConnectionString);

        public ProgressTrackingController()
        {
            CloudStorageAccount cloudStorageAccount;
            if (CloudStorageAccount.TryParse(
                ConfigurationManager.ConnectionStrings["AzureStorage"].ConnectionString,
                out cloudStorageAccount))
            {
                _filesStorageService = new FilesStorageService(cloudStorageAccount);
            }
        }

        // GET: ProgressTracking
        public async Task<ActionResult> Index()
        {
            var progressTrackingEntries = _db.ProgressTrackingEntries.Include(p => p.ConstructionProject).OrderByDescending(p => p.EntryDate);
            return View(await progressTrackingEntries.ToListAsync());
        }

        // GET: ProgressTracking/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgressTrackingEntry progressTrackingEntry = await _db.ProgressTrackingEntries.FindAsync(id);
            if (progressTrackingEntry == null)
            {
                return HttpNotFound();
            }
            return View(progressTrackingEntry);
        }

        // GET: ProgressTracking/Create
        public ActionResult Create()
        {
            ViewBag.ConstructionProjectId = new SelectList(_db.ConstructionProjects, "Id", "Name");
            return View();
        }

        // POST: ProgressTracking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Notes,PictureReference,ConstructionProjectId")] ProgressTrackingEntry progressTrackingEntry)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = await _filesStorageService.UploadFile(fileName, file.InputStream);
                        progressTrackingEntry.PictureReference = filePath;
                    }
                }

                progressTrackingEntry.EntryDate = DateTime.Now;
                _db.ProgressTrackingEntries.Add(progressTrackingEntry);
                await _db.SaveChangesAsync();

                await _serviceBusManager.Enqueue(new ResizePictureMessage
                {
                    Id = progressTrackingEntry.Id,
                    PictureReference = progressTrackingEntry.PictureReference
                });

                return RedirectToAction("Index");
            }

            ViewBag.ConstructionProjectId = new SelectList(_db.ConstructionProjects, "Id", "Name", progressTrackingEntry.ConstructionProjectId);
            return View(progressTrackingEntry);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
