using System;
using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content;
using Android.Provider;
using Android.Database;
using Reintegros.Droid;
using Reintegros.Shared;

namespace POCShared.Droid
{
    [Activity(Label = "Baufest Reintegros", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/AppTheme.Base")]
    public class MainActivity : Activity
    {

        public List<Android.Net.Uri> uriList = new List<Android.Net.Uri>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);           
            SetContentView(Resource.Layout.Main);
           
            LoadDdls();
            LoadEvents();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if ((requestCode == 0) && (resultCode == Result.Ok) && (data != null))
            {
                if (data.Data != null)
                {
                    uriList.Add(data.Data);
                }              
                else
                {
                    ClipData multiplePictures = data.ClipData;
                 
                    for (int i = 0; i < multiplePictures.ItemCount; i++)
                    {
                        uriList.Add(multiplePictures.GetItemAt(i).Uri);
                    }
                }
            }
        }

        private void LoadEvents()
        {
            Button btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            Button btnAttach = FindViewById<Button>(Resource.Id.btnAttach);
            EditText txtFecha = FindViewById<EditText>(Resource.Id.txtFecha);

            btnSubmit.Click += delegate{
                if (ValidateFields())
                {
                    Reintegro newReintegro = CreateReintegro();
                    int submitedID = ServiceHelper.SubmitReintegro(newReintegro);
                    if (submitedID > 0)
                    {
                        ServiceHelper.AttachPicturesToReintegro(submitedID,GetRealPathFromUri(uriList));
                        Toast.MakeText(this, "Reintegro presentado correctamente!", ToastLength.Long).Show();
                        ClearFields();
                    }
                    else
                        Toast.MakeText(this, "Error presentando el reintegro!", ToastLength.Long).Show();                    
                }
            };

            btnCancel.Click += delegate { ClearFields(); };

            txtFecha.Click += delegate { ShowPicker(); };

            btnAttach.Click += delegate {
                var imageIntent = new Intent(Intent.ActionPick);
                imageIntent.SetType("image/*");
                imageIntent.PutExtra(Intent.ExtraAllowMultiple, true);
                imageIntent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(
                    Intent.CreateChooser(imageIntent, "Select photo"), 0);
            };
        }

        private bool ValidateFields()
        {
            bool validateOK = true;
            double outDouble = 0.00;
            DateTime outDate = DateTime.Now;

            EditText txtTitle = FindViewById<EditText>(Resource.Id.txtTitle);
            EditText txtSubproyecto = FindViewById<EditText>(Resource.Id.txtSubproyecto);
            EditText txtImporte = FindViewById<EditText>(Resource.Id.txtImporte);
            EditText txtFecha = FindViewById<EditText>(Resource.Id.txtFecha);
            EditText txtCliente = FindViewById<EditText>(Resource.Id.txtCliente);
            EditText txtAutorizar = FindViewById<EditText>(Resource.Id.txtAutorizar);

            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                txtTitle.Error = "El campo no puede estar vacio";
                validateOK = false;
            }

            if (string.IsNullOrEmpty(txtSubproyecto.Text))
            {
                txtSubproyecto.Error = "El campo no puede estar vacio";
                validateOK = false;
            }

            if (string.IsNullOrEmpty(txtImporte.Text))
            {
                txtImporte.Error = "El campo no puede estar vacio";
                validateOK = false;
            }
            else if (!double.TryParse(txtImporte.Text, out outDouble))
            {
                txtImporte.Error = "El valor debe ser un numero";
                validateOK = false;
            }
            
            if (string.IsNullOrEmpty(txtFecha.Text))
            {
                txtFecha.Error = "El campo no puede estar vacio";
                validateOK = false;
            }
            else if (!DateTime.TryParse(txtFecha.Text, out outDate))
            {
                txtFecha.Error = "No es una fecha valida";
                validateOK = false;
            }

            if (string.IsNullOrEmpty(txtCliente.Text))
            {
                txtCliente.Error = "El campo no puede estar vacio";
                validateOK = false;
            }

            if (string.IsNullOrEmpty(txtAutorizar.Text))
            {
                txtAutorizar.Error = "El campo no puede estar vacio";
                validateOK = false;
            }


            return validateOK;
        }

        private void ClearFields()
        {
            EditText txtTitle = FindViewById<EditText>(Resource.Id.txtTitle);
            Spinner ddlUser = FindViewById<Spinner>(Resource.Id.ddlUser);
            Spinner dllSubunidad = FindViewById<Spinner>(Resource.Id.ddlSubunidad);
            EditText txtSubproyecto = FindViewById<EditText>(Resource.Id.txtSubproyecto);
            Spinner ddlMotivo = FindViewById<Spinner>(Resource.Id.ddlMotivo);
            EditText txtImporte = FindViewById<EditText>(Resource.Id.txtImporte);
            EditText txtFecha = FindViewById<EditText>(Resource.Id.txtFecha);
            EditText txtCliente = FindViewById<EditText>(Resource.Id.txtCliente);
            EditText txtAutorizar = FindViewById<EditText>(Resource.Id.txtAutorizar);
            Spinner ddlPersonaAutorizante = FindViewById<Spinner>(Resource.Id.ddlPersonaAutorizante);

            txtTitle.Text = string.Empty;
            ddlUser.SetSelection(0); 
            dllSubunidad.SetSelection(0);          
            txtSubproyecto.Text = string.Empty;
            ddlMotivo.SetSelection(0);
            txtImporte.Text = string.Empty;
            txtFecha.Text = string.Empty;
            txtCliente.Text = string.Empty;
            txtAutorizar.Text = string.Empty;
            ddlPersonaAutorizante.SetSelection(0);
        }

        private Reintegro CreateReintegro()
        {
            EditText txtTitle = FindViewById<EditText>(Resource.Id.txtTitle);
            Spinner ddlUser = FindViewById<Spinner>(Resource.Id.ddlUser);
            Spinner dllSubunidad = FindViewById<Spinner>(Resource.Id.ddlSubunidad);
            EditText txtSubproyecto = FindViewById<EditText>(Resource.Id.txtSubproyecto);
            Spinner ddlMotivo = FindViewById<Spinner>(Resource.Id.ddlMotivo);
            EditText txtImporte = FindViewById<EditText>(Resource.Id.txtImporte);
            EditText txtFecha = FindViewById<EditText>(Resource.Id.txtFecha);
            EditText txtCliente = FindViewById<EditText>(Resource.Id.txtCliente);
            EditText txtAutorizar = FindViewById<EditText>(Resource.Id.txtAutorizar);
            Spinner ddlPersonaAutorizante = FindViewById<Spinner>(Resource.Id.ddlPersonaAutorizante);

            Reintegro newReintegro = new Reintegro();

            User myPersona = ServiceHelper.usersList.Find(x => x.Name == ddlUser.SelectedItem.ToString());
            User myPersonaAutorizante = ServiceHelper.usersList.Find(x => x.Name == ddlPersonaAutorizante.SelectedItem.ToString());

            var fecha = txtFecha.Text.Split('/');

            newReintegro.Title = txtTitle.Text;
            newReintegro.Persona = myPersona.ID + ";#" + myPersona.LoginName;
            newReintegro.Subunidad = dllSubunidad.SelectedItem.ToString();
            newReintegro.Subproyecto = txtSubproyecto.Text;
            newReintegro.Motivo = ddlMotivo.SelectedItem.ToString();
            newReintegro.Importe = txtImporte.Text;
            newReintegro.Fecha = fecha[2] + "-" + fecha[0] + "-" + fecha[1];
            newReintegro.ClienteProyecto = txtCliente.Text;
            newReintegro.DebeAutorizar = txtAutorizar.Text;
            newReintegro.PersonaAutorizante = myPersonaAutorizante.ID + ";#" + myPersonaAutorizante.LoginName;

            return newReintegro;
        }

        private void LoadDdls()
        {
            /*Get Users from list and load Persona dll*/
            UserContainer users = ServiceHelper.GetListUsers();
            Spinner ddlUser = FindViewById<Spinner>(Resource.Id.ddlUser);
            ArrayAdapter userAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, users.User);
            userAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            ddlUser.Adapter = userAdapter;

            /*Get combo choices for Subunidad and load Subunidad dll*/
            List<string> combo = ServiceHelper.GetComboChoices("Subunidad");
            Spinner ddlSubunidad = FindViewById<Spinner>(Resource.Id.ddlSubunidad);
            ArrayAdapter subunidadAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, combo.ToArray());
            subunidadAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            ddlSubunidad.Adapter = subunidadAdapter;

            combo.Clear();

            /*Get combo choices for Motivo and load Motivo dll*/
            combo = ServiceHelper.GetComboChoices("Motivo_x0020_de_x0020_Solicitud");
            Spinner ddlMotivo = FindViewById<Spinner>(Resource.Id.ddlMotivo);
            ArrayAdapter motivoAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, combo.ToArray());
            motivoAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            ddlMotivo.Adapter = motivoAdapter;

            /*Get Users from list and load Persona dll*/
            Spinner ddlPersonaAutorizante = FindViewById<Spinner>(Resource.Id.ddlPersonaAutorizante);
            ArrayAdapter personaAutorizanteAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, users.User);
            personaAutorizanteAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            ddlPersonaAutorizante.Adapter = personaAutorizanteAdapter;
        }

        private void ShowPicker()
        {
            TextView txtFecha = FindViewById<TextView>(Resource.Id.txtFecha);
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                txtFecha.Error = null;
                txtFecha.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private List<string> GetRealPathFromUri(List<Android.Net.Uri> uris)
        {
            List<string> paths = new List<string>();
            foreach (Android.Net.Uri item in uris)
            {
                ICursor cursor = ContentResolver.Query(item, null, null, null, null);
                cursor.MoveToFirst();
                string documentId = cursor.GetString(0);
                documentId = documentId.Split(':')[1];
                cursor.Close();

                cursor = ContentResolver.Query(
                Android.Provider.MediaStore.Images.Media.ExternalContentUri,
                null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new[] { documentId }, null);
                cursor.MoveToFirst();
                paths.Add(cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data)));
                cursor.Close();
            }

            return paths;
        }

    }   
}



