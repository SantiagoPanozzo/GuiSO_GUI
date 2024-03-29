﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using GuiSO_GUI.GUIHandlers;
using GuiSO_GUI.Model;
using GuiSO_GUI.Services;

namespace GuiSO_GUI.ViewModel;

public partial class RespaldoViewModel : BaseViewModel
{
    public ObservableCollection<UsuarioModel> UsuarioModels { get; } = new();
    private UsuariosService _usuariosService;

    public RespaldoViewModel(UsuariosService usuarioService){
        _usuariosService = usuarioService;
    }

    [RelayCommand]
    async Task GetBackedUpUsuarios()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var usuarios = await _usuariosService.GetBackedUpUsuarios();

            if(UsuarioModels.Count != 0)
                UsuarioModels.Clear();
                
            foreach(var monkey in usuarios)
                UsuarioModels.Add(monkey);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"No se pudo obtener los usuarios: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task BackUpUsuarios(UsuarioModel model)
    {
        if(IsBusy)
            return;

        try
        {
            IsBusy = true;
            foreach (var usuarioModel in UsuarioModels)
            {
                if (usuarioModel.IsBackUpQueued)
                {
                    usuarioModel.IsBackedUp = BackUpHandlerGUI.MakeBackUp(usuarioModel.Nombre);
                    usuarioModel.IsBackUpQueued = false;
                }
            }

            IsBusy = false;
            await GetBackedUpUsuarios();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"No se pudo hacer el respaldo: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}