using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerClasses.Services
{
    public partial class SessionService : ObservableObject
    {
        [ObservableProperty]
        private string name;

        public void SetUser(string user)
        {
            Name = user;
        }

        public void ClearSession()
        {
            Name = "";
        }
    }
}
