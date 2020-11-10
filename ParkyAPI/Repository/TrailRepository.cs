using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;
        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            // Lazy Loading for NationalPark
            Trail trail = _db.Trails.Include(t => t.NationalPark).FirstOrDefault(t => t.Id == trailId);
            return trail;
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.OrderBy(n => n.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            return _db.Trails.Any(n => n.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool TrailExists(int trailId)
        {
            if (_db.Trails.Find(trailId) != null)
            {
                return true;
            }
            return false;
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int nationalParkId)
        {
            return _db.Trails.Include(t => t.NationalPark).Where(n => n.Id == nationalParkId).ToList();
        }
    }
}
