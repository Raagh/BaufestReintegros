using System.Net;
using Android.App;
using Android.OS;
using Android.Widget;
using POCShared.Droid;
using Reintegros.Shared;

namespace Reintegros.Droid
{
    [Activity(Label = "Baufest Reintegros", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/AppTheme.Base")]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);

            Button btnLogin = FindViewById<Button>(Resource.Id.btnIngresar);

            btnLogin.Click += delegate { Login(); };
        }

        private void Login()
        {
            TextView txtUser = FindViewById<TextView>(Resource.Id.txtUsuario);
            TextView txtPassword = FindViewById<TextView>(Resource.Id.txtPassword);

            NetworkCredential credential = new NetworkCredential(txtUser.Text, txtPassword.Text, "baunet");

            bool isValid = ServiceHelper.AuthenticateCredentials(credential);

            if (isValid)
            {
                Toast.MakeText(this, "Login correcto!", ToastLength.Long).Show();
                StartActivity(typeof(MainActivity));
            }
            else if (!isValid)
            {
                txtPassword.Error = "El usuario y/o contrase�a no es valido";               
            }               
        }
    }
}