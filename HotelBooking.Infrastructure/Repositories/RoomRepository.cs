﻿using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;

namespace HotelBooking.Infrastructure.Repositories
{
    public class RoomRepository : IRepository<Room>
    {
        private readonly HotelBookingContext db;

        public RoomRepository(HotelBookingContext context)
        {
            db = context;
        }

        public void Add(Room entity)
        {
            db.Room.Add(entity);
            db.SaveChanges();
        }

        public void Edit(Room entity)
        {
            throw new NotImplementedException();
        }

        public Room Get(int id)
        {
            // The FirstOrDefault method below returns null
            // if there is no room with the specified Id.
            return db.Room.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Room> GetAll()
        {
            return db.Room.ToList();
        }

        public void Remove(int id)
        {
            //retro fitted to remove all rooms for the sake of testing
            var list = db.Room.ToList();
            db.Room.RemoveRange(list);
            db.SaveChanges();
        }
    }
}
