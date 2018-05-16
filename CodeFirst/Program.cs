using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using MySql.Data.Entity;

namespace CodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BloggingContext())
            {
                // Create and save a new Blog 
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                var blog = new Blog { Name = name };
                db.Blogs.Add(blog);
                db.SaveChanges();

                // Display all Blogs from the database 
                var query = from b in db.Blogs
                            orderby b.Name
                            select b;

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }
                var post = new Post();
                post.BlogId = 1;
                post.Title = "Hello World!";
                post.Content = "Welcome to beijing!";
                db.Posts.Add(post);
                db.SaveChanges();
                var q = from b in db.Posts
                            orderby b.Title
                            select b;
                Console.WriteLine("All posts in the database:");
                foreach(var item in q)
                {
                    Console.WriteLine(item.Content);
                }
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            } 
        }
    }
    public class Blog
    {  
       [Key]
        public int BlogId { get; set; }
       [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
       public string AddUrl { get; set; }
        public string user { get; set; }
        public virtual List<Post> Posts { get; set; } 
    }

    public class Post
    {
        [Key]
        public int PostId { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
       [MaxLength(100)]
        public string Content { get; set; }
       
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; } 
        
    }
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }

}
