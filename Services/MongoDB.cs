using MongoDB.Driver;
using YappingAPI.Models;

namespace YappingAPI.Services
{
    public class MongoDB
    {
        private readonly IMongoCollection<Models.User> _users;
        private readonly IMongoCollection<Models.Post> _posts;
        private readonly IMongoCollection<Models.Category> _categories;
        private readonly IMongoCollection<Models.Comment> _comments;
        private readonly IMongoCollection<Models.Likes> _likes;
        private readonly IMongoCollection<Models.Message> _Messages;
        private readonly IMongoCollection<Models.Report> _reports;
        private readonly IMongoCollection<Models.ChatMessages> _chatMessages;
        private readonly IMongoCollection<Models.GroupChat> _groupChat;
        public MongoDB(IConfiguration config)
        {
            var settings = MongoClientSettings.FromConnectionString(config["MongoDb:ConnectionString"]);
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(10);
            var client = new MongoClient(settings);
            var db = client.GetDatabase(config["MongoDb:Database"]);
            _users = db.GetCollection<Models.User>("Users");
            _posts = db.GetCollection<Models.Post>("Posts");
            _categories = db.GetCollection<Models.Category>("Categories");
            _comments = db.GetCollection<Models.Comment>("Comments");
            _likes = db.GetCollection<Models.Likes>("Likes");
            _Messages = db.GetCollection<Models.Message>("Message");
            _reports = db.GetCollection<Models.Report>("Report");
            _groupChat = db.GetCollection<Models.GroupChat>("GroupChat");
            _chatMessages = db.GetCollection<Models.ChatMessages>("ChatMessage");
        }
        //-------------------------Chat Controller------------------------------||

        public async Task CreateChatMessage(Models.ChatMessages chatMessage)
        {
            await _chatMessages.InsertOneAsync(chatMessage);
        }

        public async Task<List<Models.ChatMessages>> GetChatMessages(string chatId)
        {
            return await _chatMessages.Find(m => m.ChatId == chatId).ToListAsync();
        }
        public async Task CreateGroupChat(Models.GroupChat chat)
        {
            await _groupChat.InsertOneAsync(chat);
        }
        public async Task<List<Models.GroupChat>> GetUsersChats(string userId)
        {
            return await _groupChat.Find(c => c.UserIds.Contains(userId)).ToListAsync();
        }
        public async Task AddUserToChat(string userId, string chatId)
        {
            var chat = await _groupChat.Find(c => c.Id == chatId).FirstOrDefaultAsync();

            if (chat != null)
            {
                chat.UserIds.Add(userId);
                await _groupChat.ReplaceOneAsync(c => c.Id == chat.Id, chat);
            }
        }
        public async Task RemoveUserFromChat(string userId, string chatId)
        {
            var chat = await _groupChat.Find(c => c.Id == chatId).FirstOrDefaultAsync();
            
            if(chat != null)
            {
                chat.UserIds.Remove(userId);
                await _groupChat.ReplaceOneAsync(c => c.Id == chatId, chat);
            }
        }

        //----------------------------Report Controller----------------------||
        public async Task DeleteRaport(string id)
        {
            await _reports.DeleteOneAsync(r => r.Id == id);
        }

        public async Task CreateReport(Models.Report report)
        {
            await _reports.InsertOneAsync(report);
        }
        public async Task MarkReportAsRead(string id)
        {
            var filter = Builders<Models.Report>.Filter.Eq(R => R.Id, id); 

            var update = Builders<Models.Report>.Update.Set(R => R.IsRead, true);

            await _reports.UpdateOneAsync(filter, update);
        }

        public async Task<List<Models.Report>> GetAllUnreadReports()
        {
            return await _reports.Find(R => R.IsRead == false).ToListAsync();
        }

        public async Task<List<Models.Report>> GetReports()
        {
            return await _reports.Find(_ => true).ToListAsync();
        }

        //---------------------------Messages Controller-------------------||

        public async Task<List<Models.Message>> GetResivedMessages(string id)
        {
            return await _Messages.Find(M => M.ResiveId == id).ToListAsync();
        }

        public async Task<bool> AnyUnRead(string id)
        {
            bool hasUnreadMessages = await _Messages
                .Find(m => m.ResiveId == id && !m.IsRead)
                .AnyAsync(); 
            return hasUnreadMessages;
        }

        public async Task SendMessage(Models.Message message)
        {
            await _Messages.InsertOneAsync(message);
        }
        
        public async Task MarkAsRead(string id)
        {
            var filter = Builders<Models.Message>.Filter.Eq(M => M.ResiveId, id) &
                         Builders<Models.Message>.Filter.Eq(M => M.IsRead, false);

            var update = Builders<Models.Message>.Update.Set(M => M.IsRead, true);

            await _Messages.UpdateManyAsync(filter, update);
        }

        //--------------------Likes Controller-----------------------------||

        public async Task<List<Models.Likes>> GetLikes(string objid)
        {
            return await _likes.Find(like => like.ObjId == objid && like.IsLiked == true).ToListAsync();
        }
        public async Task Like(string objid, string userid)
        {
            var obj = _likes.Find(obj => obj.ObjId == objid).FirstOrDefault();

            if (obj == null)
            {
                var like = new Models.Likes() { ObjId = objid, UserId = userid, IsLiked = true};
                await _likes.InsertOneAsync(like);
            }
            else
            {
                var filter = Builders<Models.Likes>.Filter.Eq(obj => obj.ObjId, objid);

                var isLiked = false;
                if (!obj.IsLiked) 
                    isLiked = true;

                var update = Builders<Models.Likes>.Update.Set(obj => obj.IsLiked, isLiked);

                await _likes.UpdateOneAsync(filter, update);
            }
        }

        //------------------ Category Controller --------------------------||
        public async Task CreateCategory(Models.Category category)
        {
            await _categories.InsertOneAsync(category);
        }

        public async Task<List<Models.Category>> GetCategories()
        {
            return await _categories.Find(_ => true).ToListAsync();
        }
        public async Task<Models.Category> GetCategoryById(string id)
        {
            return await _categories.Find(cat => cat.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Models.Category?> GetCategory(string name)
        {
            return await _categories.Find(cat => cat.Name == name).FirstOrDefaultAsync();
        }

        //---------------- Post Controller --------------------------------||

        public async Task DeletePost(string id)
        {
            await _posts.DeleteOneAsync(P => P.Id == id);
        }

        public async Task CreatePost(Models.Post post)
        {
            await _posts.InsertOneAsync(post);
        }
        public async Task AddPostLike(string postid)
        {
            var filter = Builders<Models.Post>.Filter.Eq(post => post.Id, postid);
            var update = Builders<Models.Post>.Update.Inc(post => post.Like, 1);

            await _posts.UpdateOneAsync(filter, update);
        }
        public async Task<Models.Post> GetPost(string postid)
        {
            return await _posts.Find(post => post.Id == postid).FirstOrDefaultAsync();
        }
        public async Task<List<Models.Post>> GetLatestPosts(DateTime lastPost)
        {
            try
            {

                    var filter = Builders<Post>.Filter.Lt(p => p.CreatedAt, lastPost);
                    return await _posts.Find(filter)
                        .SortByDescending(p => p.CreatedAt)
                        .Limit(10)
                        .ToListAsync();
            }
            catch
            {
                return [];
            }
        }

        public async Task<List<Models.Post>> GetPosts()
        {
             return await _posts.Find(_ => true)
                        .SortByDescending(p => p.CreatedAt)
                        .Limit(10)
                        .ToListAsync();
        }

        public async Task<List<Models.Post>> GetPostsInCategory(Models.Category category)
        {
            return await _posts.Find(post => post.CategoryId == category.Id).ToListAsync();
        }

        public async Task<List<Models.Post>> GetUserPosts(Models.User user)
        {
            return await _posts.Find(Post => Post.UserId == user.Id).ToListAsync();
        }

        //------------- Comment Controller --------------------------------||

        public async Task DeleteComment(string id)
        {
            await _comments.DeleteOneAsync(C => C.Id == id);
        }
        public async Task CreateComment(Models.Comment comment)
        {
            await _comments.InsertOneAsync(comment);
        }

        public async Task<Models.Comment> GetComment(string id)
        {
            return await _comments.Find(C => C.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Models.Comment>> GetCommentsOnPost(string postid)
        {
            return await _comments.Find(comment => comment.PostId == postid).ToListAsync();
        }

        public async Task<List<Models.Comment>> GetUserComments(Models.User user)
        {
            return await _comments.Find(comment => comment.UserId == user.Id).ToListAsync();
        }

        //----------- User Controller -------------------------------------||


        public async Task UpdateProfilePic(string userId, string filePath)
        {
                var filter = Builders<Models.User>.Filter.Eq(user => user.Id, userId);
                var update = Builders<Models.User>.Update.Set(user => user.ProfileImg, filePath);

                await _users.UpdateOneAsync(filter, update);
        }
        public async Task<bool> LogIn(string username, string password)
        {
            var user = await _users.Find(user => user.Username == username).FirstOrDefaultAsync();
            if (user == null)
                return false;

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                return true;
            else
                return false;           
        }
        public async Task<string> GetUserId(string username)
        {
            var user = await _users.Find(user =>user.Username == username).FirstOrDefaultAsync();
            Console.WriteLine(user.Id + " i mongo");
            return user.Id;
        }

        public async Task<List<Models.User>> GetUsers()
        {
            return await _users.Find(user => true).ToListAsync();
        }

        public async Task<Models.User> GetUserFromId(string id)
        {
            return await _users.Find(user =>user.Id == id).FirstOrDefaultAsync();
        }

        public async Task RegisterUser(Models.User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task<bool> IsUsernameTaken(string username)
        {
            var existingUser = await _users.Find(user => user.Username == username).FirstOrDefaultAsync();
            return existingUser != null;
        }

        public async Task<bool> IsEmailTaken(string email)
        {
            var existingUser = await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
            return existingUser != null;
        }
    }
}
