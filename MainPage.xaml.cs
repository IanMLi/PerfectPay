namespace PerfectPay
{
    public partial class MainPage : ContentPage
    {
        private double totalCuenta = 0;
        private int personas = 1;
        private double propina = 0.05;
        
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.WidthRequest = Application.Current.MainPage.Width;
            this.HeightRequest = Application.Current.MainPage.Height;
        }

        private async void OnTotalTapped(object sender, EventArgs e)
        {
            string resultado = await DisplayPromptAsync("Total de la cuenta", "Ingresa el total:", initialValue: totalCuenta.ToString(), keyboard: Keyboard.Numeric);
            if (double.TryParse(resultado, out double total) && total >= 0)
            {
                totalCuenta = total;
                lblTotalCuenta.Text = "$" + totalCuenta.ToString("0.00");
                CalcularTotalPorPersona();
            }
        }

        private async void OnPersonasTapped(object sender, EventArgs e)
        {
            string resultado = await DisplayPromptAsync("Número de personas", "Ingresa la cantidad:", initialValue: personas.ToString(), keyboard: Keyboard.Numeric);
            if (int.TryParse(resultado, out int num) && num > 0)
            {
                personas = num;
                // Solo se actualiza el slider si el número es menor o igual a 10
                if (num <= 10)
                {
                    sliderPersonas.Value = num;
                }
                entryPersonas.Text = personas.ToString();
                CalcularTotalPorPersona();
            }
        }

        private void OnSliderChanged(object sender, ValueChangedEventArgs e)
        {
            // El slider solo se usa para valores entre 1 y 10
            personas = (int)e.NewValue;
            entryPersonas.Text = personas.ToString();
            CalcularTotalPorPersona();
        }

        private void OnPropinaChanged(object sender, EventArgs e)
        {
            if (sender is Button btn && double.TryParse(btn.CommandParameter.ToString(), out double porcentaje))
            {
                propina = porcentaje;
                btnPropinaOtro.Text = "Otro";
                CalcularTotalPorPersona();
            }
        }

        private async void OnPropinaOtro(object sender, EventArgs e)
        {
            string resultado = await DisplayPromptAsync("Propina personalizada", "Ingresa un porcentaje:", initialValue: (propina * 100).ToString(), keyboard: Keyboard.Numeric);
            if (double.TryParse(resultado, out double porcentaje) && porcentaje >= 0)
            {
                if (porcentaje > 50)
                {
                    await DisplayAlert("Error", "El porcentaje de propina no puede ser mayor al 50%", "OK");
                    return;
                }
                propina = porcentaje / 100;
                btnPropinaOtro.Text = porcentaje + "%";
                CalcularTotalPorPersona();
            }
        }

        private void CalcularTotalPorPersona()
        {
            double totalConPropina = totalCuenta * (1 + propina);
            double totalPorPersona = totalConPropina / personas;
            lblTotalPorPersona.Text = "$" + totalPorPersona.ToString("0.00");
        }
        private void OnBackgroundTapped(object sender, EventArgs e)
        {
            entryPersonas.Unfocus(); 
        }
        private void OnEntryCompleted(object sender, EventArgs e)
        {
            entryPersonas.Unfocus(); 
        }

    }
}
