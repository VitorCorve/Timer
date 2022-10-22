using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System;
using Timer.Internal.Models;
using Timer.ViewModel;

namespace Timer.Internal.Services
{
    /// <summary>
    /// �������� ������ ����������� ����� ���������� WPF ����������. <para></para>
    /// ������ ��������� ������������ ����� ������ <see cref="ViewModelBasic"/>, 
    /// <see cref="UserControl"/>, <see cref="Guid"/>, � <see cref="String"/> 
    /// <see cref="MessengerEntity.Name"/> ����-������� WPF ����������.
    /// </summary>
    public class Messenger
    {
        private static readonly List<MessengerEntity> entities = new List<MessengerEntity>();
        /// <summary>
        /// ����������� ������������ ������� ����������� <see cref="MessengerEntity"/>, 
        /// ���������� ������ �������� <see cref="ViewModelBasic"/>, <see cref="UserControl"/>, 
        /// <see cref="Guid"/>, <see cref="string"/> <paramref name="entityName"/>
        /// </summary>
        /// <param name="entityName"></param>
        public static void Register(PropertyChangedAbstract viewModel, UserControl userControl, Guid guid, string entityName)
        {
            if (IsRegistered(guid))
                return;

            MessengerEntity entity = Create(viewModel, userControl, guid, entityName);
            entities.Add(entity);
        }
        /// <summary>
        /// ����������� ������������ ������� ����������� <see cref="MessengerEntity"/> .
        /// </summary>
        public static void Register(MessengerEntity entity)
        {
            if (IsRegistered(entity.ID))
                return;
            entities.Add(entity);
        }
        /// <summary>
        ///  ������ � ����������� ������� ���������� ����� ����� �� <see cref="String"/> <paramref name="entityName"/>.
        /// </summary>
        /// <param name="entityName"></param>
        public static void WriteOff(string entityName)
        {
            MessengerEntity entity = entities.Where(x => x.Name == entityName).FirstOrDefault();
            if (entity != null)
                entities.Remove(entity);
        }
        /// <summary>
        ///  ������ � ����������� ������� ���������� ����� ����� �� <see cref="Guid"/> <paramref name="guid"/>.
        /// </summary>
        /// <param name="guid"></param>
        public static void WriteOff(Guid guid)
        {
            MessengerEntity entity = entities.Where(x => x.ID == guid).FirstOrDefault();
            if (entity != null)
                entities.Remove(entity);
        }
        private static bool IsRegistered(Guid guid) => entities.Where(x => x.ID == guid).FirstOrDefault() != null;
        private static bool IsRegistered(string name) => entities.Where(x => x.Name == name).FirstOrDefault() != null;
        private static MessengerEntity Create(PropertyChangedAbstract viewModel, UserControl userControl, Guid guid, string entityName)
        {
            return new MessengerEntity(viewModel, userControl, guid, entityName);
        }
        private static MessengerEntity Find(string name) => entities.Where(x => x.Name == name).FirstOrDefault();
        private static MessengerEntity Find(Guid guid) => entities.Where(x => x.ID == guid).FirstOrDefault();
        /// <summary>
        /// �������� ������� <paramref name="message"/> �������� <see cref="String"/> <paramref name="acceptor"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="acceptor"></param>
        public static void DeliverObject(object message, string acceptor)
        {
            if (IsRegistered(acceptor))
            {
                MessengerEntity acceptorEntity = Find(acceptor);
                acceptorEntity.ViewModel.AcceptMessage(message);
            }
        }
        /// <summary>
        /// �������� ������� <paramref name="message"/> �������� <see cref="Guid"/> <paramref name="acceptorId"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="acceptorId"></param>
        public static void DeliverObject(object message, Guid acceptorId)
        {
            if (IsRegistered(acceptorId))
            {
                MessengerEntity acceptorEntity = Find(acceptorId);
                acceptorEntity.ViewModel.AcceptMessage(message);
            }
        }
        /// <summary>
        /// ����� ������� <paramref name="commandName"/> �������� <see cref="String"/> <paramref name="acceptor"/>.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="acceptor"></param>
        public static void CallCommand(string commandName, string acceptor)
        {
            if (IsRegistered(acceptor))
            {
                MessengerEntity acceptorEntity = Find(acceptor);
                acceptorEntity.ViewModel.ExecuteExternalCommand(commandName);
            }
        }
        /// <summary>
        /// ����� ������� <paramref name="commandName"/> � ���������� <paramref name="commandParameter"/>, �������� <see cref="String"/> <paramref name="acceptor"/>.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="acceptor"></param>
        public static void CallCommand(string commandName, string commandParameter, string acceptor)
        {
            if (IsRegistered(acceptor))
            {
                MessengerEntity acceptorEntity = Find(acceptor);
                acceptorEntity.ViewModel.ExecuteExternalCommand(commandName, commandParameter);
            }
        }
        /// <summary>
        /// ����� ������� <paramref name="commandName"/> �������� <see cref="Guid"/> <paramref name="acceptorId"/>.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="acceptorId"></param>
        public static void CallCommand(string commandName, Guid acceptorId)
        {
            if (IsRegistered(acceptorId))
            {
                MessengerEntity acceptorEntity = Find(acceptorId);
                acceptorEntity.ViewModel.ExecuteExternalCommand(commandName);
            }
        }
        /// <summary>
        /// ����� ������� <paramref name="commandName"/> � ���������� <paramref name="commandParameter"/>, �������� <see cref="Guid"/> <paramref name="acceptorId"/>.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="acceptorId"></param>
        public static void CallCommand(string commandName, string commandParameter, Guid acceptorId)
        {
            if (IsRegistered(acceptorId))
            {
                MessengerEntity acceptorEntity = Find(acceptorId);
                acceptorEntity.ViewModel.ExecuteExternalCommand(commandName, commandParameter);
            }
        }
    }
}
