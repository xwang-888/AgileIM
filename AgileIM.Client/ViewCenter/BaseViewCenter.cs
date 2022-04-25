using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AgileIM.Client.ViewCenter
{
    public class BaseViewCenter<TView, TViewModel> : IBaseViewCenter where TView : ContentControl, new() where TViewModel : class
    {
    
    }
}
