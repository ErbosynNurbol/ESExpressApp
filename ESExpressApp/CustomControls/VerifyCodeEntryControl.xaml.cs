using ESExpressApp.Handlers;

namespace ESExpressApp.CustomControls;

public partial class VerifyCodeEntryControl : Grid
{
	public VerifyCodeEntryControl()
	{
		InitializeComponent();
        entryVerifyCode_1.Focus();
    }

    private void entryVerifyCode_Focused(object sender, FocusEventArgs e)
    {
        BorderlessEntry entryVerifyCode = sender as BorderlessEntry;
        if (!entryVerifyCode_1.IsFocused &&
            string.IsNullOrEmpty(entryVerifyCode_1.Text) &&
            string.IsNullOrEmpty(entryVerifyCode_2.Text) &&
            string.IsNullOrEmpty(entryVerifyCode_3.Text) &&
            string.IsNullOrEmpty(entryVerifyCode_4.Text) &&
            string.IsNullOrEmpty(entryVerifyCode_5.Text))
        {
            entryVerifyCode_1.Focus();
        }

        if (!entryVerifyCode_5.IsFocused &&
          !string.IsNullOrEmpty(entryVerifyCode_1.Text) &&
          !string.IsNullOrEmpty(entryVerifyCode_2.Text) &&
          !string.IsNullOrEmpty(entryVerifyCode_3.Text) &&
          !string.IsNullOrEmpty(entryVerifyCode_4.Text) &&
          !string.IsNullOrEmpty(entryVerifyCode_5.Text))
        {
            entryVerifyCode_5.Focus();
        }
        if (!string.IsNullOrEmpty(entryVerifyCode.Text))
        {
            entryVerifyCode.CursorPosition = entryVerifyCode.Text.Length;
        }
    }

    private void entryVerifyCode_1_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            if (e.NewTextValue.Length > 1)
            {
                entryVerifyCode_1.Text = e.NewTextValue.Substring(0, 1);
                entryVerifyCode_2.Text = e.NewTextValue.Substring(1);
            }
            bvVerifyCode_1.BackgroundColor = Color.FromArgb("#ced4da");
            entryVerifyCode_2.Focus();
        }
        else
        {
            bvVerifyCode_1.BackgroundColor = Color.FromArgb("#737373");
        }
    }
    private void entryVerifyCode_2_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            if (e.NewTextValue.Length > 1)
            {
                entryVerifyCode_2.Text = e.NewTextValue.Substring(0, 1);
                entryVerifyCode_3.Text = e.NewTextValue.Substring(1);
            }
            bvVerifyCode_2.BackgroundColor = Color.FromArgb("#ced4da");
            entryVerifyCode_3.Focus();
        }
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            entryVerifyCode_1.Focus();
            bvVerifyCode_2.BackgroundColor = Color.FromArgb("#737373");
        }
    }
    private void entryVerifyCode_3_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            if (e.NewTextValue.Length > 1)
            {
                entryVerifyCode_3.Text = e.NewTextValue.Substring(0, 1);
                entryVerifyCode_4.Text = e.NewTextValue.Substring(1);
            }
            bvVerifyCode_3.BackgroundColor = Color.FromArgb("#ced4da");
            entryVerifyCode_4.Focus();
        }
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            entryVerifyCode_2.Focus();
            bvVerifyCode_3.BackgroundColor = Color.FromArgb("#737373");
        }
    }
    private void entryVerifyCode_4_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            if (e.NewTextValue.Length > 1)
            {
                entryVerifyCode_4.Text = e.NewTextValue.Substring(0, 1);
                entryVerifyCode_5.Text = e.NewTextValue.Substring(1);
            }
            bvVerifyCode_4.BackgroundColor = Color.FromArgb("#ced4da");
            entryVerifyCode_5.Focus();
        }
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            entryVerifyCode_3.Focus();
            bvVerifyCode_4.BackgroundColor = Color.FromArgb("#737373");
        }
    }
    private void entryVerifyCode_5_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            if (e.NewTextValue.Length > 1)
            {
                entryVerifyCode_5.Text = e.NewTextValue.Substring(0, 1);
            }
            bvVerifyCode_5.BackgroundColor = Color.FromArgb("#ced4da");
        }
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            entryVerifyCode_4.Focus();
            bvVerifyCode_5.BackgroundColor = Color.FromArgb("#737373");
        }
    }
}
