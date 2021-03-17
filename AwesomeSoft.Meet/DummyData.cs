﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeSoft.Meet.Models;

namespace AwesomeSoft.Meet
{
    /// <summary>
    /// A singleton class to store dummy data, instead of using a live database.
    /// </summary>
    public class DummyData
    {
        private static DummyData _instance;

        public List<Meeting> Meetings { get; set; }

        public List<User> Users { get; set; }

        public static DummyData Instance {
            get
            {
                if (_instance is null)
                {
                    _instance = new DummyData();
                }
                return _instance;
            }
        }

        private DummyData()
        {
            Users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Name = "User1"
                },
                new User()
                {
                    Id = 2,
                    Name = "User2"
                }
            };
            Meetings = new List<Meeting>()
            {
                new Meeting() {
                    Id = 1,
                    Title = "Test meeting 1",
                    Description = "This is just a test meeting.\nNothing to see here.",
                    Owner = Users.First(),
                },
                new Meeting() {
                    Id = 2,
                    Title = "Test meeting 2",
                    Description = "This is just a another test meeting.\nNothing to see here.",
                    Owner = Users.Last(),
                    Participants = new List<User>()
                    {
                        Users.First()
                    }
                }
            };
        }
    }
}
