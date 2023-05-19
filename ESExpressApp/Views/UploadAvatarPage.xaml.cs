using Camera.MAUI;

namespace ESExpressApp.Views;

public partial class UploadAvatarPage : ContentPage
{
	public UploadAvatarPage()
	{
		InitializeComponent();
        cameraView.CamerasLoaded += CameraView_CamerasLoaded;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
       // cameraView.HeightRequest = cameraView.Width;
    }

    private void CameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.NumCamerasDetected > 0)
        {
            if (cameraView.NumMicrophonesDetected > 0)
                cameraView.Microphone = cameraView.Microphones.First();
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (await cameraView.StartCameraAsync() == CameraResult.Success)
                {
                    controlButton.Text = "Stop";
                   // playing = true;
                }
            });
        }
    }
}
