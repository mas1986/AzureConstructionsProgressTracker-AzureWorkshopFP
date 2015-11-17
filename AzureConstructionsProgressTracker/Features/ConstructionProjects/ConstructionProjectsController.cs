using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Common;

namespace AzureConstructionsProgressTracker.Features.ConstructionProjects
{
    public class ConstructionProjectsController : Controller
    {
        private ConstructionsProgressTrackerContext db = new ConstructionsProgressTrackerContext();

        // GET: ConstructionProjects
        public async Task<ActionResult> Index()
        {
            return View(await db.ConstructionProjects.ToListAsync());
        }

        // GET: ConstructionProjects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConstructionProject constructionProject = await db.ConstructionProjects.FindAsync(id);
            if (constructionProject == null)
            {
                return HttpNotFound();
            }
            return View(constructionProject);
        }

        // GET: ConstructionProjects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConstructionProjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Notes")] ConstructionProject constructionProject)
        {
            if (ModelState.IsValid)
            {
                db.ConstructionProjects.Add(constructionProject);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(constructionProject);
        }

        // GET: ConstructionProjects/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConstructionProject constructionProject = await db.ConstructionProjects.FindAsync(id);
            if (constructionProject == null)
            {
                return HttpNotFound();
            }
            return View(constructionProject);
        }

        // POST: ConstructionProjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Notes")] ConstructionProject constructionProject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(constructionProject).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(constructionProject);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
