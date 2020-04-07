﻿using System.Diagnostics;
using System.Windows;
using PRM.Core;
using PRM.Core.DB;
using PRM.Core.Model;
using PRM.Core.Protocol;
using PRM.Core.Protocol.RDP;
using PRM.Core.UI.VM;
using PRM.View;
using Shawn.Ulits.PageHost;

namespace PRM.ViewModel
{
    public class VmServerCard : NotifyPropertyChangedBase
    {
        private ProtocolServerBase _server = null;
        public ProtocolServerBase Server
        {
            get => _server;
            private set => SetAndNotifyIfChanged(nameof(Server), ref _server, value);
        }

        public readonly VmServerListPage Host;

        public VmServerCard(ProtocolServerBase server, VmServerListPage host)
        {
            Server = server;
            Host = host;
        }



        private RelayCommand _connServer;
        public RelayCommand ConnServer
        {
            get
            {
                if (_connServer == null)
                    _connServer = new RelayCommand((o) =>
                    {

                        this.Server.Conn();
                    });
                return _connServer;
            }
        }

        private RelayCommand _editServer;
        public RelayCommand CmdEditServer
        {
            get
            {
                if (_editServer == null)
                {
                    _editServer = new RelayCommand((o) =>
                    {
                        Host.Host.DispPage = new AnimationPage()
                        {
                            InAnimationType = AnimationPage.InOutAnimationType.SlideFromRight,
                            OutAnimationType = AnimationPage.InOutAnimationType.SlideToRight,
                            Page = new ServerEditorPage(new VmServerEditorPage((ProtocolServerBase)this.Server.Clone(),
                                this.Host)),
                        };
                    });
                }

                return _editServer;
            }
        }


        private RelayCommand _deleteServer;
        public RelayCommand DeleteServer
        {
            get
            {
                if (_deleteServer == null)
                    _deleteServer = new RelayCommand((o) =>
                    {
                        if (MessageBox.Show(
                                Global.GetInstance().GetText("server_card_operate_confirm_delete"), 
                                Global.GetInstance().GetText("messagebox_title_warning"), 
                                MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                            MessageBoxResult.Yes)
                        {
                            Global.GetInstance().ServerListRemove(Server);
                        }
                    });
                return _deleteServer;
            }
        }
    }
}
