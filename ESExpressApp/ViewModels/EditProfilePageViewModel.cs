using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalizationResourceManager.Maui;
using SkiaSharp;

namespace ESExpressApp.ViewModels
{
    public partial class EditProfilePageViewModel : BaseViewModel
    {
        [ObservableProperty]
        PersonModel person;

        [ObservableProperty]
        ImageSource avatarSource;

        [ObservableProperty]
        string localFilePath;

        private readonly ILocalizationResourceManager resourceManager;
        public EditProfilePageViewModel(ILocalizationResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
            LoadData();
        }

        [RelayCommand]
        private void Retry()
        {
            CurrentState = States.Loading;
            LoadData();
        }

        [RelayCommand]
        public void Refresh()
        {
            IsRefreshing = true;
              LoadData();
            IsRefreshing = false;
        }

        [RelayCommand]
        public void DeleteAccount()
        {
            App.AlertSvc.ShowConfirmation(this.resourceManager["ls_Deleteaccount"], this.resourceManager["ls_Deleteafyd"], (result =>
            {
                if (result)
                {
                    AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/user/deleteaccount", null);
                    if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
                    {
                        if (ajaxMsg.Status.Equals("success", StringComparison.OrdinalIgnoreCase))
                        {
                            AppSingleton.GetInstance().SetLoginInfo(null);
                            Preferences.Set("localLoginInfo", "");
                            App.Current.MainPage = new AppShell();
                        }
                        else
                        {
                            App.AlertSvc.ShowAlert(this.resourceManager["ls_Warning"], $"{ajaxMsg?.Message}", this.resourceManager["ls_Close"]);
                        }
                    }
                }
            }), this.resourceManager["ls_Delete"], this.resourceManager["ls_Close"]);
        }
        
        [RelayCommand]
        private void LoadData()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                CurrentState = States.Offline;
                return;
            }
            if (IsBusy) return;
            IsBusy = true;
            AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/query/personinfo", null);
            if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
            {
                Person = JsonHelper.DeserializeObject<PersonModel>(ajaxMsg.Data.ToString());
                AvatarSource = ImageSource.FromUri(new Uri(Person.AvatarUrl));
                CurrentState = States.Success;
            }
            else
            {
                CurrentState = States.Error;
            }
            IsBusy = false;
        }

        [RelayCommand]
        private void ChangePhoneNumber()
        {
            GoToAsync($"{nameof(ChangePhoneNumberPage)}");
        }

        #region Файлды сақтау кеңейтімін алу  GetSaveFormat(string filePath)
        private static SKEncodedImageFormat GetSaveFormat(string filePath)
        {
            string fileFormat = Path.GetExtension(filePath).ToLower();
            SKEncodedImageFormat saveFormat = SKEncodedImageFormat.Png;
            switch (fileFormat)
            {
                case ".png":
                    {
                        saveFormat = SKEncodedImageFormat.Png;
                    }
                    break;
                case ".jpg":
                case ".jpeg":
                    {
                        saveFormat = SKEncodedImageFormat.Jpeg;
                    }
                    break;
                case ".gif":
                    {
                        saveFormat = SKEncodedImageFormat.Gif;
                    }
                    break;
            }
            return saveFormat;
        }
        #endregion

        [RelayCommand]
        private async Task UploadAvatarAsync()
        {
            //if (MediaPicker.Default.IsCaptureSupported)
            //{

            //}
            FileResult photo = await MediaPicker.Default.PickPhotoAsync();
            if (photo != null)
            {
                // save the file into local storage
                //LocalFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                //using Stream sourceStream = await photo.OpenReadAsync();
                //using FileStream localFileStream = File.OpenWrite(LocalFilePath);
                //await sourceStream.CopyToAsync(localFileStream);
                //AvatarSource = ImageSource.FromFile(LocalFilePath);


                using Stream sourceStream = await photo.OpenReadAsync();
                //using Stream sourceStream = await photo.OpenReadAsync();
                using (MemoryStream stream = new MemoryStream())
                {
                    await sourceStream.CopyToAsync(stream);
                    var fileBytes = stream.ToArray();
                    SKBitmap skBitmap = SkiaSharp.SKBitmap.Decode(fileBytes);
                    //Resize
                    int thumbnailWidth = skBitmap.Width > 1000 ? 1000 : skBitmap.Width;
                    int thumbnailHeight = skBitmap.Height * 1000 / skBitmap.Width;
                    if (thumbnailHeight > skBitmap.Height)
                        thumbnailHeight = skBitmap.Height;
                    SKBitmap thumbnailBitmap = skBitmap.Resize(new SKImageInfo(thumbnailWidth, thumbnailHeight), SKFilterQuality.High);
                    SKRect dest = new SKRect(0, 0, thumbnailWidth, thumbnailHeight);
                    float pointX = thumbnailWidth > thumbnailHeight ? (thumbnailWidth - thumbnailHeight) / 2 : 0;
                    float pointY = thumbnailHeight > thumbnailWidth ? (thumbnailHeight - thumbnailWidth) / 2 : 0;
                    int croppedWidth, croppedHeight;
                    croppedWidth = croppedHeight = thumbnailWidth > thumbnailHeight ? thumbnailHeight : thumbnailWidth;
                    SKRect source = new SKRect(pointX, pointY,
                                               pointX + croppedWidth, pointY + croppedHeight);
                    SKBitmap croppedBitmap = new SKBitmap(croppedWidth, croppedHeight);
                    using (SKCanvas canvas = new SKCanvas(croppedBitmap))
                    {
                        canvas.DrawBitmap(thumbnailBitmap, source, dest);
                    }

                    SKEncodedImageFormat saveFormat = GetSaveFormat(photo.FileName);
                    LocalFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    using (var image = SKImage.FromBitmap(croppedBitmap))
                    using (var data = image.Encode(saveFormat, 100))
                    using (var streamLocalFile = File.OpenWrite(LocalFilePath))
                    {
                        // save the data to a stream
                        data.SaveTo(streamLocalFile);
                    }
                    AvatarSource = ImageSource.FromFile(LocalFilePath);
                }
            }
        }
        [RelayCommand]
        private void Save()
        {
            AjaxMsgModel ajaxMsg = null;
            if (!string.IsNullOrEmpty(LocalFilePath) && File.Exists(LocalFilePath))
            {
                using (Stream stream = File.OpenRead(LocalFilePath))
                {
                    var skData = SKData.Create(stream);
                    SKBitmap skBitmap = SKBitmap.Decode(skData);
                    Stream streamData = SKImage.FromBitmap(skBitmap).Encode().AsStream();
                    ajaxMsg = APIHelper.UploadAvatar(streamData);
                    if (ajaxMsg != null && ajaxMsg.Status.Equals("success"))
                    {
                        LoginInfoModel loginInfo = AppSingleton.GetInstance().GetLoginInfo();
                        loginInfo.UserInfo.AvatarUrl = ajaxMsg.Data.ToString();
                        AppSingleton.GetInstance().SetLoginInfo(loginInfo);
                        Toast.Make(ajaxMsg?.Message).Show();
                    }
                }
            }
            var dicParameters = new Dictionary<string, string>
                    {
                      {"realName", Person.RealName??string.Empty},
                      {"city", Person.City??string.Empty},
                      {"whatsApp", Person.WhatsApp??string.Empty},
                      {"secondaryPhone", Person.SecondaryPhone??string.Empty} 
                    };
            ajaxMsg = APIHelper.Query($"/api/send/PersonInfo", dicParameters);
            if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
            {
                if (ajaxMsg.Status.Equals("success", StringComparison.OrdinalIgnoreCase))
                {
                    LoginInfoModel loginInfo = AppSingleton.GetInstance().GetLoginInfo();
                    loginInfo.UserInfo.RealName = Person.RealName;
                    loginInfo.UserInfo.WhatsApp = Person.WhatsApp;
                    loginInfo.UserInfo.SecondaryPhone = Person.SecondaryPhone;
                    AppSingleton.GetInstance().SetLoginInfo(loginInfo);
                    Toast.Make(this.resourceManager["ls_SavedSuccessfully"]).Show();
                }
                else
                {
                     App.AlertSvc.ShowAlert(this.resourceManager["ls_Warning"], $"{ajaxMsg?.Message}", this.resourceManager["ls_Close"]);
                }
            }
            else
            {
                string entryName = string.Empty;
                switch (ajaxMsg?.Data)
                {
                    case "userName":
                        {
                            entryName = $"({this.resourceManager["ls_Yourname"]})";
                        }
                        break;
                    case "city":
                        {
                            entryName = $"({this.resourceManager["ls_District"]})";
                        }
                        break;
                }
                App.AlertSvc.ShowAlert(this.resourceManager["ls_Warning"], $"{ajaxMsg?.Message}{entryName}", this.resourceManager["ls_Close"]);
            }
        }
    }
}

