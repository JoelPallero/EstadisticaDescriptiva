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
                    HabilitarDGVcoleccion();
                    coleccionY = Convert.ToInt32(dgvDatos.Rows.Count - 1);
                    coleccionX = Convert.ToInt32(dgvDatos.Columns.Count);

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
                    HabilitarDGVcoleccion();


                    #endregion
                    break;

                case 2:
                    #region Bidimensional
                    HabilitarDGVBidi();
                    string coleccionBid;
                    int r = 0;
                    coleccionY = Convert.ToInt32(dgvDatos.Rows.Count - 1);
                    coleccionX = Convert.ToInt32(dgvDatos.Columns.Count);
                    dgvBidimensional.Rows.Add(coleccionX + 1);
                    N = coleccionX;
                    txtEne.Text = N.ToString();

                    for (int i = 0; i < coleccionY; i++)
                    {
                        for (int j = 0; j < coleccionX; j++)
                        {
                            coleccionBid = dgvDatos.Rows[i].Cells[j].Value.ToString();
                            dgvBidimensional.Rows[j].Cells[i].Value = coleccionBid;
                            r += Convert.ToInt32(coleccionBid);
                        }
                        dgvBidimensional.Rows[coleccionX].Cells[i].Value = r.ToString();
                        r = 0;
                        coleccionBid = string.Empty;
                    }
                    #endregion
                    break;
            }
        }
        private void FrecuenciaAbsoluta()
        {
            switch (calculo)
            {
                case 0:
                    #region Simples
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
                    #endregion
                    break;
                case 1:
                    break;
                case 2:
                    #region Bidimensional

                    #endregion
                    break;
            }
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
            switch (calculo)
            {
                case 0:
                    #region Simples
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
                    #endregion
                    break;
                case 1:
                    break;
                case 2:
                    #region Bidimensional
                    int x = 0;
                    int y = 0;

                    x = Convert.ToInt32(dgvBidimensional.Rows[coleccionX].Cells[0].Value);
                    y = Convert.ToInt32(dgvBidimensional.Rows[coleccionX].Cells[1].Value);

                    mediaX = Convert.ToDouble(x) / N;
                    mediaY = Convert.ToDouble(y) / N;

                    txtMediaX.Text = mediaX.ToString("N2");
                    txtMediaY.Text = mediaY.ToString("N2");

                    #endregion
                    break;
            }            
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
            desviacionTipica = Math.Sqrt(varianza);
            txtDesviacionTipica.Text = desviacionTipica.ToString("N3");
        }
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
            int x = 0;
            int y = 0;
            int multi = 0;
            int sum = 0;
            for (int i = 0; i < coleccionX; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    x = Convert.ToInt32(dgvBidimensional.Rows[i].Cells[j].Value);
                    y = Convert.ToInt32(dgvBidimensional.Rows[i].Cells[1].Value);
                    multi = x * y;
                    sum += multi;
                    dgvBidimensional.Rows[i].Cells[2].Value = multi.ToString();
                    y = 0;
                    x = 0;
                    multi = 0;
                }
            }
            dgvBidimensional.Rows[coleccionX].Cells[2].Value = sum.ToString();

            double xy = mediaX * mediaY;
            double rs = sum / N;
            covarianza = rs - xy;
            txtCovarianza.Text = (covarianza).ToString("N3");

            rs = 0;
            xy = 0;
            sum = 0;
        }
        private void CoeficienteCorrelacion()
        {
            int x = 0;
            int multi = 0;
            int sum = 0;
            double desvTipX = 0;
            double desvTipY = 0;
            int col = 3;

            //X al cuadrado y su sumatoria
            for (int i = 0; i < coleccionX; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    x = Convert.ToInt32(dgvBidimensional.Rows[i].Cells[j].Value);
                    multi = x * x;
                    sum += multi;
                    dgvBidimensional.Rows[i].Cells[col].Value = multi.ToString();
                    x = 0;
                    multi = 0;
                }
            }
            dgvBidimensional.Rows[coleccionX].Cells[col].Value = sum.ToString();
            col++;

            desvTipX = Convert.ToDouble((sum / N) - (mediaX * mediaX)); 

            x = 0;
            multi = 0;
            sum = 0;

            //Y al cuadrado y su sumatoria.
            for (int i = 0; i < coleccionX; i++)
            {
                for (int j = 1; j < 2; j++)
                {
                    x = Convert.ToInt32(dgvBidimensional.Rows[i].Cells[j].Value);
                    multi = x * x;
                    sum += multi;
                    dgvBidimensional.Rows[i].Cells[col].Value = multi.ToString();
                    x = 0;
                    multi = 0;
                }
            }
            dgvBidimensional.Rows[coleccionX].Cells[col].Value = sum.ToString();

            desvTipY = Convert.ToDouble((sum / N) - (mediaY * mediaY));

            desvTipX = Math.Sqrt(desvTipX);
            desvTipY = Math.Sqrt(desvTipY);

            //Coeficiente correlación:
            coefcorrelacion = covarianza / (desvTipX * desvTipY);
            txtCoefCorre.Text = coefcorrelacion.ToString("N3");

            x = 0;
            multi = 0;
            sum = 0;
            col = 3;

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

        #region Cambiar de DGV (Porque uso 1 para los calculos bidimensionales y otro para los intervalos simples)
        private void HabilitarDGVcoleccion()
        {
            dgvBidimensional.Visible = false;
            dgvColeccion.Visible = true;
        }
        private void HabilitarDGVBidi()
        {
            dgvBidimensional.Visible = true;
            dgvColeccion.Visible = false;
        }

        #endregion

        #region Reset de controles

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
        private void ResetVariables()
        {
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
            desviacionMedia = 0;
            desviacionTipica = 0;
            varianza = 0;
            covarianza = 0;
            coefcorrelacion = 0;
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

        #region Eventos
        private void btnCalcularOrdenar_Click(object sender, EventArgs e)
        {
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
            if (chkSimple.Checked)
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
            else
            {
                Media();
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
            calculo = 2;
        }
        private void chkCompuesto_CheckedChanged(object sender, EventArgs e)
        {
            calculo = 1;
            OrdenarDatos();
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
        private void btnCovarianza_CheckedChanged(object sender, EventArgs e)
        {
            Covarianza();
        }
        private void btnCoefCor_CheckedChanged(object sender, EventArgs e)
        {
            CoeficienteCorrelacion();
        }
        #endregion
    }
}