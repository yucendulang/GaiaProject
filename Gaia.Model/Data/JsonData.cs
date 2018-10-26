// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaiaProject.Models.Data
{
    public partial class UserFriendController
    {
        public class Info
        {
            public int state { get; set; }
            public string message { get; set; }
        }

        public class JsonData
        {
            public JsonData()
            {
                this.info=new Info();
            }
            public Info info { get; set; }
            public object data;
        }
    }
}
