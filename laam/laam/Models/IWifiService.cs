using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laam.Models
{
    public interface IWifiService
    {
        string GetDefaultGateway();
        void Connect(string id, string password);
        void SetMobileDataEnabled(bool enable);
    }
}
