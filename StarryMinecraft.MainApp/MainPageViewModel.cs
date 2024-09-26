﻿using System.Collections.ObjectModel;
using System.Net;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace StarryMinecraft.MainApp;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ServerProfileValueModel> _menuItems = [];

    [RelayCommand]
    private void AddServer(ServerProfileValueModel server)
    {
        MenuItems.Add(server);
    }

    [RelayCommand]
    private void DeleteServer(ServerProfileValueModel server)
    {
        MenuItems.Remove(server);
    }

    [RelayCommand]
    private async Task ShowAddServerDialog()
    {
        // Show dialog to get server details
        var address = await App.Current.MainPage!.DisplayPromptAsync("Add Server", "Enter server address:");
        var port = int.Parse(await App.Current.MainPage.DisplayPromptAsync("Add Server", "Enter server port:"));
        var password = await App.Current.MainPage.DisplayPromptAsync("Add Server", "Enter server password (optional):");
        var nickname = await App.Current.MainPage.DisplayPromptAsync("Add Server", "Enter server nickname (optional):");

        if (!string.IsNullOrWhiteSpace(address) && port > 0)
        {
            AddServer(new ServerProfileValueModel(address, port, password, nickname));
        }
    }
}

public partial class ServerProfileValueModel : ObservableObject
{
    [ObservableProperty]
    private string _address;

    [ObservableProperty]
    private int _port;

    [ObservableProperty]
    private string? _password;

    [ObservableProperty]
    private string? _nickname;

    public string Title => Nickname
        ?? $"{Address}:{Port}";

    public ServerProfileValueModel(
        string address,
        int port,
        string? password,
        string? nickname)
    {
        _address = address;
        _port = port;
        _password = password;
        _nickname = nickname;
    }
}