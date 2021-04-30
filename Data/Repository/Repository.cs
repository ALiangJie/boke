﻿using Bolg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolg.Data.Repository
{
    public class Repository : IRepository
    {

        private AppDbContext _ctx;

        public Repository(AppDbContext ctx)
        {
            _ctx = ctx;
        }


        public Post GetPost(int id)
        {
            return _ctx.Posts.FirstOrDefault(p => p.Id == id);
        }
        public List<Post> GetAllPosts()
        {
            return _ctx.Posts.ToList();
        }

        public List<Post> GetAllPosts(string category)
        {
            Func<Post, bool> InCategory = (post) => { return post.Category.ToLower().Equals(category.ToLower()); };
            //InCategory(a)=2;
            //InCategory(b)=10;
            //var a = 2;
            //F#,Clojure,Haskell

            return _ctx.Posts
                .Where(post => InCategory(post))
                .ToList();
        }

        public void AddPost(Post post)
        {
            _ctx.Posts.Add(post);
            
        }
        public void UpdatePost(Post post)
        {
            _ctx.Posts.Update(post);
        }
        public void RemovePost(int id)
        {
            _ctx.Posts.Remove(GetPost(id));
        }

        public async Task<bool> SaveChangesAsybc()
        {
            if(await _ctx.SaveChangesAsync()>0)
            {
                return true;
            }
            return false;
        }
    }
}
