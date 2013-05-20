using System;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace PanoramaSite
{
    public class MongoDBProjectDriver
    {
        private class Counter
        {
            public string Id { get; set; }
            public int NextId { get; set; }
        }

        private class CounterHelper
        {
            MongoDatabase _db;
            MongoCollection<Counter> _counters;

            public CounterHelper(MongoDatabase db)
            {
                _db = db;
                _counters = _db.GetCollection<Counter>("counters");
            }

            public int GetNextId<TT>()
            {
                string className = typeof(TT).FullName;
                try
                {
                    var nextIdDoc = _counters.FindAndModify(Query<Counter>.EQ(c => c.Id, className), SortBy.Null, Update<Counter>.Inc(c => c.NextId, 1), true);
                    return BsonSerializer.Deserialize<Counter>(nextIdDoc.ModifiedDocument).NextId;
                }
                catch
                {
                    _counters.Insert(new Counter { Id = className, NextId = 1 });
                }

                return 1;
            }
        }

        MongoServer _server;
        MongoDatabase _db;
        CounterHelper _counterHelper;
        MongoCollection<Project> _projects;

        void InitConnection()
        {
            if (_server != null)
                return;

            //var connectionString = "mongodb://localhost";
            var connectionString = "mongodb://mobilltestuser:pass123@ds041167.mongolab.com:41167/moBillMongoLab";
            var client = new MongoClient(connectionString);
            _server = client.GetServer();
            _db = _server.GetDatabase("moBillMongoLab");

            _counterHelper = new CounterHelper(_db);

            _projects = _db.GetCollection<Project>("projects");
        }

        public void Reset()
        {
            InitConnection();
            _projects.Drop();
        }

        public Project AddProject(Project project)
        {
            InitConnection();

            project.Id = _counterHelper.GetNextId<Project>();
            _projects.Insert(project);

            return project;
        }

        public List AddListToProject(Project project, List list)
        {
            InitConnection();

            list.Id = _counterHelper.GetNextId<List>();
            IMongoQuery query = Query<Project>.Where(p => p.Id == project.Id);
            var foundProject = _projects.FindOneAs<Project>(query);

            foundProject.Lists.Add(list);

            _projects.Update(query, Update<Project>.Replace(foundProject));

            return list;
        }

        public Card AddCardToListOfProject(Project project, List list, Card card)
        {
            InitConnection();

            card.Id = _counterHelper.GetNextId<Card>();
            IMongoQuery query = Query<Project>.Where(p => p.Id == project.Id);
            var foundProject = _projects.FindOneAs<Project>(query);

            foundProject.Lists.Find(l => l.Id == list.Id).Cards.Add(card);

            _projects.Update(query, Update<Project>.Replace(foundProject));

            return card;
        }

        public Project GetProject(Project project)
        {
            InitConnection();
            return _projects.FindOneAs<Project>(Query<Project>.EQ(p => p.Id, project.Id));
        }

        internal void MoveCardToList(Project project, List list, Card card, int index)
        {
            InitConnection();

            IMongoQuery query = Query<Project>.Where(p => p.Id == project.Id);
            var foundProject = _projects.FindOneAs<Project>(query);

            Card foundCard = null;

            foundProject.Lists.Where(li =>
            {
                foundCard = li.Cards.Where(ci => ci.Id == card.Id).FirstOrDefault();
                if (foundCard != null)
                {
                    li.Cards.Remove(foundCard);
                    return true;
                }

                return false;
            }).First();

            foundProject.Lists.Find(li => li.Id == list.Id).Cards.Insert(index, foundCard);

            _projects.Update(query, Update<Project>.Replace(foundProject));
        }
    }
}