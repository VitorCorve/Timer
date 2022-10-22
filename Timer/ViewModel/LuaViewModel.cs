using Timer.Internal;
using Timer.Internal.Models;
using Timer.Internal.Services;

namespace Timer.ViewModel
{
    public class LuaViewModel : PropertyChangedAbstract
    {
        private Command _accept, _decline;

        public Command Accept => _accept ??= new Command(obj =>
        {
            Messenger.CallCommand("register", "MainViewModel");
        });

        public Command Decline => _decline ??= new Command(obj =>
        {
            Messenger.CallCommand("auth", "MainViewModel");
        });
        public LuaViewModel()
        {
            Messenger.Register(new MessengerEntity(this, "LuaViewModel"));
        }
    }
}
