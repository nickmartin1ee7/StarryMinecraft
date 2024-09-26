using System.Collections.ObjectModel;
using System.Text.Json;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace StarryMinecraft.MainApp;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ServerProfileValueModel> _menuItems = new();

    public MainPageViewModel()
    {
        LoadServerProfiles();
    }

    [RelayCommand]
    private void AddServer(ServerProfileValueModel server)
    {
        MenuItems.Add(server);
        SaveServerProfiles();
    }

    [RelayCommand]
    private void DeleteServer(ServerProfileValueModel server)
    {
        MenuItems.Remove(server);
        SaveServerProfiles();
    }

    [RelayCommand]
    private async Task ShowAddServerDialog()
    {
        var address = await App.Current!.MainPage!.DisplayPromptAsync("Add Server", "Enter server address:");
        if (string.IsNullOrWhiteSpace(address)) return;

        var portString = await App.Current.MainPage.DisplayPromptAsync("Add Server", "Enter server port:");
        if (string.IsNullOrWhiteSpace(portString) || !int.TryParse(portString, out var port)) return;

        var password = await App.Current.MainPage.DisplayPromptAsync("Add Server", "Enter server password (optional):");
        var nickname = await App.Current.MainPage.DisplayPromptAsync("Add Server", "Enter server nickname (optional):");

        var optionalPassword = string.IsNullOrWhiteSpace(password) ? null : password;
        var optionalNickname = string.IsNullOrWhiteSpace(nickname) ? null : nickname;

        AddServer(new ServerProfileValueModel(address, port, optionalPassword, optionalNickname));
    }

    private void SaveServerProfiles()
    {
        var serverList = MenuItems.Select(server => new
        {
            server.Address,
            server.Port,
            server.Password,
            server.Nickname
        }).ToList();

        var json = JsonSerializer.Serialize(serverList);
        Preferences.Set("ServerProfiles", json);
    }

    private void LoadServerProfiles()
    {
        var json = Preferences.Get("ServerProfiles", string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            var serverList = JsonSerializer.Deserialize<List<ServerProfileValueModel>>(json);
            if (serverList != null)
            {
                MenuItems = new ObservableCollection<ServerProfileValueModel>(serverList);
            }
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

    public string Title => Nickname ?? $"{Address}:{Port}";

    public ServerProfileValueModel(string address, int port, string? password, string? nickname)
    {
        _address = address;
        _port = port;
        _password = password;
        _nickname = nickname;
    }
}
