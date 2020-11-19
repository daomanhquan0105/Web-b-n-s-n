using System.Collections.Generic;
using System.Web.Mvc;
using PagedList;
using TongkhosonVn.Models;

namespace TongkhosonVn.ViewModel
{
    public class ListPostViewModel
    {
        public IPagedList<Post> Posts { get; set; }
        public PostCategory PostCategory { get; set; }
        public IEnumerable<PostCategory> ChildCategories { get; set; }
    }
    public class ListPostAdminViewModel
    {
        public IPagedList<Post> Posts { get; set; }
        public SelectList CategoryList { get; set; }
        public SelectList ChildList { get; set; }
        public TypeCategory? Type { get; set; }
        public int? CatId { get; set; }
        public int? ChildId { get; set; }
        public string Name { get; set; }

        public ListPostAdminViewModel()
        {
            CategoryList = new SelectList(new List<PostCategory>(), "Id", "CategoryName");
            ChildList = new SelectList(new List<PostCategory>(), "Id", "CategoryName");
        }
    }

    public class AddPostViewModel
    {
        public Post Post { get; set; }
        public int CategoryId { get; set; }
        public int? ChildId { get; set; }

        public SelectList CategoryList { get; set; }
        public SelectList ChildList { get; set; }
        public TypeCategory TypeCategory { get; set; }

        public AddPostViewModel()
        {
            CategoryList = new SelectList(new List<PostCategory>(), "Id", "CategoryName");
            ChildList = new SelectList(new List<PostCategory>(), "Id", "CategoryName");
        }
    }

    public class PostDetailViewModel
    {
        public Post Post { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}