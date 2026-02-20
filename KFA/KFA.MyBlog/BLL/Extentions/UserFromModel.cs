using KFA.MyBlog.BLL.ViewModels.User;
using KFA.MyBlog.DAL.Entities;

namespace KFA.MyBlog.BLL.Extentions
{
    public static class UserFromModel
    {
        public static User Convert(this User user, UserViewModel usereditvm)
        {
            user.Last_Name = usereditvm.Last_Name;
            user.Middle_Name = usereditvm.Middle_Name;
            user.First_Name = usereditvm.First_Name;
            user.Email = usereditvm.Email;
            //user.BirthDate = usereditvm.BirthDate;
            user.BirthDate = new System.DateTime((int)usereditvm.Year, (int)usereditvm.Month, (int)usereditvm.Day);

            return user;
        }
    }
}
