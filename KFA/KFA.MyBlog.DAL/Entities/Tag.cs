using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFA.MyBlog.DAL.Entities
{
    public class Tag
    {
        public int ID { get; set; }
        public string Tag_Name { get; set; }
        public List<Article> Articles { get; set; }
    }
}
