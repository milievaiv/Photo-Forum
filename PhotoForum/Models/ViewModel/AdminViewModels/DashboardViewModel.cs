﻿namespace PhotoForum.Models.ViewModel.AdminViewModels
{
    public class DashboardViewModel
    {
        public List<Log> Logs;
        public List<Post> MostRecentPosts;
        public int TotalUsers;
        public int TotalPosts;
        public Admin Admin;
        public User TopUser;
    }
}
