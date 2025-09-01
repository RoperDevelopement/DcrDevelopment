using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.InterFaces;
namespace BMRMobileApp.Utilites
{
#if ANDROID
using Android.App;

public class AppExitService : IAppExitService
{
    public Task RequestExitAsync()
    {
        Platform.CurrentActivity?.FinishAffinity();
        return Task.CompletedTask;
    }
}
#endif

#if IOS
    public class AppExitService : IAppExitService
    {
        public Task RequestExitAsync()
        {
            // iOS doesn't allow force-closing apps. Consider navigating to root or showing a message.
            return Task.CompletedTask;
        }
    }
#endif
#if WINDOWS
 public class AppExitService : IAppExitService
    {
        public Task RequestExitAsync()
        {
            // iOS doesn't allow force-closing apps. Consider navigating to root or showing a message.
            return Task.CompletedTask;
        }
    }
#endif
}