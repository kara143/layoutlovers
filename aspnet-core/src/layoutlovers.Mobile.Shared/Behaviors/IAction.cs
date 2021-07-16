using Xamarin.Forms.Internals;

namespace layoutlovers.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}