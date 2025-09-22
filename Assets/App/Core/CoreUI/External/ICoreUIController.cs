using App.Common.Utilities.Utility.Runtime;
using App.Core.Core.External.View;

namespace App.Core.Core.External
{
    public interface ICoreUIController
    {
        Optional<CoreView> GetView();
    }
}