using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EstadisticaDescriptiva
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        #region Variables
        bool empty = true;
        int ordenadorCol = 0;
        int ordenadorRow = 0;
        int contador = 0;
        int repetidor = 0;
        int mayor = 0;
        int coleccionDato = 0;
        int coleccionY = 0;
        int coleccionX = 0;
        double N = 0;
        double comparacionFrecAcum = 0;
        double mediana = 0;
        double PartialMe = 0;
        double mediaX = 0;
        double mediaY= 0;
        double desviacionMedia = 0;
        double desviacionTipica = 0;
        double varianza = 0;
        
        double covarianza = 0;
        double coefcorrelacion = 0;

        int calculo;

        #endregion

        #region Metodos

        private void OrdenarDatos()
        {
            switch (calculo)
            {
                case 0:
                    #region intervalo simple
                    HabilitarCheks();
                    coleccionY = Convert.ToInt32(dgvDatos.Rows.Count - 1);
                    coleccionX = dgvDatos.Columns.Count;

                    for (int i = 0; i < coleccionY; i++)
                    {
                        for (int j = 0; j < coleccionX; j++)
                        {
                            //Acá cuento los datos para ir ordenando.
                            coleccionDato = Convert.ToInt32(dgvDatos.Rows[i].Cells[j].Value);

                            if (coleccionDato > mayor)
                            {
                                //los guardo en esta variable "mayor" y así se cuántas filas voy a utilizar
                                mayor = coleccionDato;
                            }
                        }
                    }

                    mayor++; //Esto me sirve para color las sumatorias de cada dato en este N° de fila que queda guardado
                    dgvColeccion.Rows.Add(mayor);
                    FrecuenciaAbsoluta();
                    #endregion
                    break;

                case 1:

                    #region Intervalos compuestos



                    #endregion

                    break;

                case 2:

                    #region Bidimensional
                    HabilitarChkBid();
                    string coleccionBi;
                    int r = 0;
                    coleccionY = Convert.ToInt32(dgvDatos.Rows.Count - 1);
                    coleccionX = dgvDatos.Columns.Count;
                    dgvBidimensional.Rows.Add(coleccionX + 1);

                    for (int j = 0; j < coleccionX; j++)
                    {
                        for (int i = 0; i < coleccionY; i++)
                        {
                            coleccionBi = dgvDatos.Rows[i].Cells[j].Value.ToString();
                            dgvBidimensional.Rows[j].Cells[i].Value = coleccionBi;
                            r += Convert.ToInt32(coleccionBi);
                        }
                        dgvBidimensional.Rows[j].Cells[coleccionX + 1].Value = r.ToString();
                        r = 0;
                    }
                    #endregion

                    break;
            }
        }

        private void FrecuenciaAbsoluta()
        {
            for (int x = 0; contador < mayor; x++)
            {
                for (int i = 0; i < coleccionY; i++)
                {
                    for (int j = 0; j < coleccionX; j++)
                    {
                        coleccionDato = Convert.ToInt32(dgvDatos.Rows[i].Cells[j].Value);

                        if (contador == coleccionDato)
                        {
                            repetidor++;
                        }
                    }
                }
                //agregar este dato en el otro dgv
                // y al lado su repeticion

                dgvColeccion.Rows[ordenadorRow].Cells[1].Value = contador.ToString();
                dgvColeccion.Rows[ordenadorRow].Cells[2].Value = repetidor.ToString();
                //sumatoria de las frecuencias
                N += repetidor;
                repetidor = 0;
                contador++;
                ordenadorRow++;
            }
            coleccionY = 0;
            coleccionX = 0;
            coleccionDato = 0;
            txtEne.Text = N.ToString();
            dgvColeccion.Rows[ordenadorRow].Cells[2].Value = N.ToString();
            ordenadorRow = 0;
            FrecuenciaAcumulada();
        }

        private void Mediana()
        {
            if ((N % 2) == 0)
            {
                mediana = N / 2;
            }
            else
            {
                mediana = (N + 1) / 2;
            }
            CompararDatosMediana();
            txtMediana.Text = mediana.ToString();
        }

        private void CompararDatosMediana()
        {
            int i = 0;
            do
            {
                comparacionFrecAcum = Convert.ToDouble(dgvColeccion.Rows[i].Cells[3].Value);
                PartialMe = comparacionFrecAcum - mediana;
                i++;
            } while (PartialMe < 0);

            mediana = Convert.ToDouble(dgvColeccion.Rows[i - 1].Cells[1].Value);
            mediana += Convert.ToDouble(dgvColeccion.Rows[i].Cells[1].Value);
            mediana = mediana / 2;
        }

        private void FrecuenciaAcumulada()
        {
            int reemplazo;
            int reemplazo2;
            for (int i = 0; i < mayor; i++)
            {
                if (ordenadorRow == 0)
                {
                    dgvColeccion.Rows[ordenadorRow].Cells[3].Value = dgvColeccion.Rows[ordenadorRow].Cells[2].Value;
                }
                else
                {
                    reemplazo = Convert.ToInt32(dgvColeccion.Rows[ordenadorRow - 1].Cells[3].Value);
                    reemplazo2 = Convert.ToInt32(dgvColeccion.Rows[ordenadorRow].Cells[2].Value);
                    dgvColeccion.Rows[ordenadorRow].Cells[3].Value = (reemplazo + reemplazo2).ToString();
                }
                ordenadorRow++;
            }
            ordenadorRow = 0;
        }

        private void Media()
        {
            int resAcum = 0;
            int resMulti = 0;
            int resSumatoria = 0;
            for (int i = 0; i < mayor; i++)
            {
                for (int j = 1; j < 2; j++)
                {
                    resMulti = Convert.ToInt32(dgvColeccion.Rows[i].Cells[j].Value);
                    resAcum = Convert.ToInt32(dgvColeccion.Rows[i].Cells[2].Value);
                    resAcum = resAcum * resMulti; //Esto es la multiplicación x por f. A cada variable le asigno un dato y multiplico
                    resSumatoria += resAcum; //Acá está la sumatoria
                    dgvColeccion.Rows[i].Cells[4].Value = resAcum.ToString(); //y acá muestro el resultado
                    resMulti = 0;
                    resAcum = 0;
                }
            }
            dgvColeccion.Rows[mayor].Cells[4].Value = resSumatoria.ToString();

            mediaX = resSumatoria / N;
            txtMedia.Text = mediaX.ToString("N3");
        }

        private void Moda()
        {
            //El que se repite más veces
            int frecuenciaModa = 0;
            int datoModa = 0;
            int moda = 0;
            for (int i = 0; i < mayor; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    //Acá es sencillo. Reviso toda la columna de frecuencias
                    //(por eso el 1 porque ese N° no cambia es la misma columna)
                    //esta variable la inicio en 0 y voy comparando si el dato que veo de la frec
                    //es mayor.
                    frecuenciaModa = Convert.ToInt32(dgvColeccion.Rows[i].Cells[2].Value); 
                    if (frecuenciaModa > moda) //Si la frecuencia que voy viendo es mayor que el N° que tengo almacenada en "moda"
                    {
                        moda = frecuenciaModa; // entonces lo reemplazo.
                        datoModa = Convert.ToInt32(dgvColeccion.Rows[i].Cells[1].Value); //y me guardo el dato x de esa fila.
                    }
                }                
            }                           //dato x                                                   //frecuencia
            txtModa.Text = "El dato " + datoModa.ToString() + " tiene la mayor frecuencia con: " + moda.ToString();
        }

        private void DesviacionMedia()
        {
            //restar x a la media y luego multiplicar por f. La sumatoria se divide por N
            double x = 0;
            double f = 0;
            double r = 0;
            double sum = 0;
            for (int i = 0; i < mayor; i++)
            {
                for (int j = 1; j < 2; j++)
                {
                    x = Convert.ToInt32(dgvColeccion.Rows[i].Cells[j].Value);
                    f = Convert.ToInt32(dgvColeccion.Rows[i].Cells[2].Value);
                    r = (x - mediaX) * f;
                    sum += r;
                    dgvColeccion.Rows[i].Cells[6].Value = r.ToString("N2");
                    x = 0;
                    f = 0;
                    r = 0;
                }
            }
            dgvColeccion.Rows[mayor].Cells[6].Value = sum.ToString("N2");

            desviacionMedia = (sum / N); //es un tema hacer potencias acá así que solo la multiplicamos así.

            txtDesvMedia.Text = desviacionMedia.ToString("N2");
        }

        private void Varianza()
        {
            int resAcum = 0;
            int resMulti = 0;
            int resSumatoria = 0;
            for (int i = 0; i < mayor; i++)
            {
                for (int j = 1; j < 2; j++)
                {
                    resMulti = Convert.ToInt32(dgvColeccion.Rows[i].Cells[j].Value); //Esto es x de cada fila
                    resAcum = Convert.ToInt32(dgvColeccion.Rows[i].Cells[2].Value);  //Esto es f de cada fila
                    resAcum = (resMulti * resMulti) * resAcum;  //Acá está x al cuadrado por f
                    resSumatoria += resAcum; //Esto es la sumatora de x al cuadrado por f
                    dgvColeccion.Rows[i].Cells[5].Value = resAcum.ToString();
                    resMulti = 0;
                    resAcum = 0;
                }
            }
            dgvColeccion.Rows[mayor].Cells[5].Value = resSumatoria.ToString();

            varianza = (resSumatoria / N) - (mediaX * mediaX); //es un tema hacer potencias acá así que solo la multiplicamos así.

            txtVarianza.Text = varianza.ToString("N3");
        }
        private void DesviacionTipica()
        {
            desviacionTipica = Math.Sqrt(varianza); //raíz cuadrada de la varianza
            txtDesviacionTipica.Text = desviacionTipica.ToString("N3"); //Convierto a string pero le paso un parametro 
        }                                                               //que es para tener 3 decimales(N3) despues de la coma

        private void CoefVariacion()
        {
            txtCoefVariacion.Text = (desviacionTipica / mediaX).ToString("N3");
        }


        private void Percentil()
        {
            double p = Convert.ToInt32(txtPercentil.Text);
            txtPerRespuesta.Text = ((p * N) / 100).ToString();
        }

        private void Quartil()
        {
            int q = Convert.ToInt32(txtQuartil.Text);
            txtQuarRespuesta.Text = ((q * N) / 4).ToString();
        }

        private void Decil()
        {
            int d = Convert.ToInt32(txtDecil.Text);
            txtDecilRespuesta.Text = ((d * N) / 10).ToString();
        }

        private void Covarianza()
        {

        }

        private void CoeficienteCorelacion()
        {

        }

        private void ChequearDGV()
        {
            if (dgvDatos.Rows.Count > 0 && chkSimple.Checked == true)
            {
                empty = false;
            }
            else
            {
                if (dgvColeccion.Rows.Count > 0 && chkCompuesto.Checked == true)
                {
                    empty = false;
                }
                else
                {
                    if (dgvDatos.Rows.Count > 0 && btnBidimensional.Checked == true)
                    {
                        empty = false;
                    }
                    else
                    {
                        empty = true;
                    }
                }
            }
        }

        #endregion

        #region Configuraciones

        private void HabilitarCheks()
        {
            //habilito los checkbox para poder hacer los calculos requeridos
            btnMediana.Enabled = true;
            btnMedia.Enabled = true;
            btnModa.Enabled = true;
        }

        private void HabilitarChkBid() //Esto está pensado para los bidimensionales. Aún no se usa.
        {
            btnRegresion.Enabled = true;
        }

        private void HabilitarCompuestos()
        {
            dgvDatos.Enabled = false;
        }
        #endregion

        #region Limpieza de controles

        private void LimpiarDGV()
        {
            foreach (var dgv in this.Controls.OfType<DataGridView>())
            {
                dgv.Rows.Clear();
            }
        }

        private void LimpiarTxt()
        {
            foreach (var txt in this.Controls.OfType<TextBox>()) //Acá son todos los textbox
            {
                txt.Text = string.Empty; //los vacío
            }
        }

        private void LimpiarChk()
        {
            foreach (var chk in this.Controls.OfType<CheckBox>()) // acá los cheackbox
            {
                chk.Checked = false; //Les quito el check
                chk.Enabled = false; //los inhabilito
            }
        }

        private void ResetVariables() //Cero simplicidad pero practicidad para lo que necesito
        {                             //que es resetear todas las variables.
            ordenadorCol = 0;
            ordenadorRow = 0;
            contador = 0;
            repetidor = 0;
            mayor = 0;
            coleccionDato = 0;
            coleccionY = 0;
            coleccionX = 0;
            N = 0;
            comparacionFrecAcum = 0;
            mediana = 0;
            PartialMe = 0;
            mediaX = 0;
            mediaY = 0;
            desviacionTipica = 0;
            varianza = 0;
            covarianza = 0;
            coefcorrelacion = 0;
            desviacionMedia = 0;
        }

        private void QuitarChecks()
        {
            foreach (var chk in gpdispersion.Controls.OfType<CheckBox>())
            {
                chk.Checked = false;
                chk.Enabled = false;
            }
        }

        #endregion


        #region checkearluego

        private void HabilitarBidimension() //Al igual que acá, es mucho reseteo de controles, pero se puede hacer esto:
        {                                   //Seccionar ciertos controles en un groupbox, y luego recoorerlos con un foreach
            if (!chkSimple.Checked)  //y así ir escondiendolos o mostrandolos, dependiendo de cada sección
            {                               // solo que yo no tenía ganas de hacer eso xq ya había hecho todo
                dgvBidimensional.Visible = false; //Pero si hacemos eso, luego en los eventos de cada checkbox tenemos que 
                dgvColeccion.Visible = true;      //asignarle el respectivo checked_change y ese era el viaje xD
                txtMediana.Visible = true;
                lblMediana.Visible = true;
                txtMedia.Visible = true;
                lblMedia.Visible = true;
                txtModa.Visible = true;
                lblModa.Visible = true;
                txtEne.Visible = true;
                lblEne.Visible = true;
                txtVarianza.Visible = true;
                lblVarianza.Visible = true;
                txtDesviacionTipica.Visible = true;
                lblTipica.Visible = true;
                lblValor.Visible = true;
                lblRespuesta.Visible = true;
                lblPercentil.Visible = true;
                txtPercentil.Visible = true;
                txtPerRespuesta.Visible = true;
                lblQuartil.Visible = true;
                txtQuartil.Visible = true;
                txtQuarRespuesta.Visible = true;
                lblDecil.Visible = true;
                txtDecil.Visible = true;
                txtDecilRespuesta.Visible = true;

                txtMediaX.Visible = false;
                lblMediaX.Visible = false;
                txtMediaY.Visible = false;
                lblMediaY.Visible = false;
                txtCovarianza.Visible = false;
                lblCovarianza.Visible = false;
                txtCoeficiente.Visible = false;
                lblCoef.Visible = false;
            }
            else
            {
                dgvBidimensional.Visible = true;
                dgvColeccion.Visible = false;
                txtMediana.Visible = false;
                lblMediana.Visible = false;
                txtMedia.Visible = false;
                lblMedia.Visible = false;
                txtModa.Visible = false;
                lblModa.Visible = false;
                txtEne.Visible = false;
                lblEne.Visible = false;
                txtVarianza.Visible = false;
                lblVarianza.Visible = false;
                txtDesviacionTipica.Visible = false;
                lblTipica.Visible = false;
                lblValor.Visible = false;
                lblRespuesta.Visible = false;
                lblPercentil.Visible = false;
                txtPercentil.Visible = false;
                txtPerRespuesta.Visible = false;
                lblQuartil.Visible = false;
                txtQuartil.Visible = false;
                txtQuarRespuesta.Visible = false;
                lblDecil.Visible = false;
                txtDecil.Visible = false;
                txtDecilRespuesta.Visible = false;

                txtMediaX.Visible = true;
                lblMediaX.Visible = true;
                txtMediaY.Visible = true;
                lblMediaY.Visible = true;
                txtCovarianza.Visible = true;
                lblCovarianza.Visible = true;
                txtCoeficiente.Visible = true;
                lblCoef.Visible = true;
            }
        }

        #endregion
        

        #region Eventos

        private void btnCalcularOrdenar_Click(object sender, EventArgs e)
        {
            //checkeoButton(); Cuando se quite el comentario, el método de abajo tiene que dejarse encapsulado en el metodo anterior.
            ChequearDGV();
            if (empty)
            {
                MessageBox.Show("No hay datos ingresados","Grillas vacías", MessageBoxButtons.OK);
            }
            else
            {
                if (btnCalcularOrdenar.Text == "Limpiar")
                {
                    LimpiarDGV();
                    LimpiarTxt();
                    LimpiarChk();
                    ResetVariables();
                    btnCalcularOrdenar.Text = "Obtener datos";
                }
                else
                {
                    OrdenarDatos();
                    btnCalcularOrdenar.Text = "Limpiar";
                }
            }            
        }

        private void btnMediana_CheckedChanged(object sender, EventArgs e)
        {
            
            if (!btnMediana.Checked)
            {
                txtMediana.Text = string.Empty;
            }
            else
            {
                Mediana(); //Acá llamo al método encapsulado para calcular la mediana
            }
        }

        private void btnMedia_CheckedChanged(object sender, EventArgs e)
        {
            if (!btnMedia.Checked)
            {
                QuitarChecks();
                txtVarianza.Text = string.Empty;
                txtDesvMedia.Text = string.Empty;
                txtMedia.Text = string.Empty;//Así que si no está habilitada y calculada la Media, el chk de la varianza se quita
            }                                //y al txt de la Media se le quita el contenido.
            else
            {
                Media();//Acá llamo al método encapsulado para calcular la media
                btnVarianza.Enabled = true; //habilito el chk de la varianza
                chkDesviacionMedia.Enabled = true;
            }
        }

        private void btnModa_CheckedChanged(object sender, EventArgs e)
        {
            if (!btnModa.Checked) //el signo ! significa la negación: Si btnModa no esta con el check entonces ..
            {
                txtModa.Text = string.Empty;
            }
            else
            {
                //por si alguien ignoraba esta opción de VS.
                Moda(); //llamo al método de la moda (Ctrl + click izquierdo) y los lleva al médoto en cuestión)
            }
        }           


        private void btnVarianza_CheckedChanged(object sender, EventArgs e)
        {
            if (!btnVarianza.Checked)
            {
                btnTipica.Enabled = false;   //Esto es lo mismo que con la media. Para calcular la tipica, tengo que tener la varianza
                txtVarianza.Text = string.Empty; //sino la saqué, entonces no se habilita el chk de tipica
                btnTipica.Checked = false;
                txtDesviacionTipica.Text = string.Empty;
            }
            else
            {
                Varianza(); //si tiene la varianza, hacemos la misma
                btnTipica.Enabled = true; // y habilitamos la tipica para calcularla.
            }
        }

        private void btnTipica_CheckedChanged(object sender, EventArgs e)
        {
            if (!btnTipica.Checked)
            {
                txtDesviacionTipica.Text = string.Empty;
                chkCoefVariación.Enabled = false;
                txtCoefVariacion.Text = string.Empty;
            }
            else
            {
                chkCoefVariación.Enabled = true;
                DesviacionTipica(); //calculo la desv tipica.
            }
        }

        //Estos 3 métodos siguientes, obviamente quie no los voy a explicar. (son una papa)
        private void txtPercentil_Leave(object sender, EventArgs e)
        {
            Percentil();
        }

        private void txtQuartil_Leave(object sender, EventArgs e)
        {
            Quartil();
        }

        private void txtDecil_Leave(object sender, EventArgs e)
        {
            Decil();
        }

        private void btnBidimensional_CheckedChanged(object sender, EventArgs e)
        {
            if (!btnBidimensional.Checked)
            {
                calculo = 0;
                dgvBidimensional.Visible = false;
                dgvColeccion.Visible = true;
            }
            else
            {
                calculo = 2;
                dgvBidimensional.Visible = true;
                dgvColeccion.Visible = false;
            }
            //HabilitarBidimension(); //Este es el chk arriba de boton para habilitar las bidimensionales y el otro dgv. Pero lo tengo deshabilitado.
        }

        #endregion

        private void chkCompuesto_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkCompuesto.Checked)
            {
                chkSimple.Enabled = true;
                chkSimple.Checked = true;
                btnBidimensional.Checked = false;
                btnBidimensional.Enabled = true;
                calculo = 0; //conesto vamos a switchear entre las 3 opciones de cálculos.
                dgvDatos.Enabled = true;
            }
            else
            {
                chkSimple.Enabled = false;
                chkSimple.Checked = false;
                btnBidimensional.Checked = false;
                btnBidimensional.Enabled = false;
                HabilitarCompuestos();
                calculo = 1;
                dgvDatos.Enabled = false;
                OrdenarDatos();
            }

        }

        private void chkSimple_CheckedChanged(object sender, EventArgs e)
        {
            calculo = 0;
        }

        private void chkDesviacionMedia_CheckedChanged(object sender, EventArgs e)
        {
            DesviacionMedia();
        }

        private void chkCoefVariación_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkDesviacionMedia.Checked)
            {
                txtCoefVariacion.Text = string.Empty;
            }
            else
            {
                CoefVariacion();
            }
        }
    }
}